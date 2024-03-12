using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    [EditorGroup("Halloween|Tutorial|Traps")]

    public class EDD : Device, ISequenceItem
    {
        public SpriteMap _sprite;
        private float setting = 0f;
        public DoorFrame d;
        public float heigt;

        public EDD(float xval, float yval) : base(xval, yval)
        {
            sequence = new SequenceItem(this);
            _sequence.type = SequenceItemType.Target;
            _sprite = new SpriteMap(GetPath("Sprites/EDD.png"), 12, 12, false);
            graphic = _sprite;
            setTime = 0.9f;
            center = new Vec2(6f, 6f);
            collisionOffset = new Vec2(-6f, -6f);
            collisionSize = new Vec2(12f, 12f);
            weight = 0.9f;
            thickness = 0.1f;
            placeable = true;
            zeroSpeed = false;
            overridePlacement = true; 
            damage = 30;
            lethal = true;
            name = LanguageManager.GetPhrase("EDD");
            description = LanguageManager.GetPhrase("H EDD");
        }
        public override void Set()
        {
            base.Set();
            setted = true;
            if (mainer != null && main != null)
            {
                main.points += 15;
                if (mainer.profile.localPlayer)
                {
                    Level.Add(new PointsGetManager(15, "Deploy"));                    
                }
            }
        }
        public override void PreSet()
        {
            DoorFrame door = Level.CheckRect<DoorFrame>(topLeft, bottomRight);
            if (door != null)
            {
                alpha = 0.5f;
                heigt = 7;
                d = door;
                gravMultiplier = 0;
                if (d != null)
                {
                    position = d.position + new Vec2(0f, height);
                }
            }
            base.PreSet();
        }
        public override void Update()
        {
            text = "Hold @SHOOT@ near door frame to deploy device";
            DoorFrame door = Level.CheckRect<DoorFrame>(topLeft, bottomRight);
            canPlace = true;
            if(door == null)
            {
                canPlace = false;
            }
            if(door != null)
            {
                if(setted == false)
                {
                    
                }
                else
                {
                    alpha = 0.5f;
                    if(prevOwner!= null)
                    {
                        Duck prev = prevOwner as Duck;
                        if(prev.crouch)
                        {
                            heigt = 3;
                            
                        }
                        if(prev.sliding)
                        {
                            heigt = 7;
                        }
                    }
                    d = door;
                    gravMultiplier = 0;
                    if (d != null)
                    {
                        position = d.position + new Vec2(0f, height);
                        foreach (Duck duck in Level.CheckCircleAll<Duck>(position, 6))
                        {
                            if (duck != null)
                            {
                                if (duck.HasEquipment(typeof(Fuse_armor)) && setted == true)
                                {
                                    Fuse_armor armor = duck.GetEquipment(typeof(Fuse_armor)) as Fuse_armor;
                                    if (armor.team != team)
                                    {
                                        Explode();
                                        if(armor.health <= 0f)
                                        {
                                            if (main != null && mainer != null)
                                            {
                                                main.points += 75;
                                                if (mainer.profile.localPlayer)
                                                {
                                                    Level.Add(new PointsGetManager(75, "Kill"));
                                                    if (Halloween.upd.challengeID == 10)
                                                    {
                                                        if (Halloween.upd.challengeProgression < Halloween.upd.challengeTarget)
                                                        {
                                                            Halloween.upd.challengeProgression += 1;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        Level.Remove(this);
                                    }
                                }
                            }
                        }
                    }

                }
            }
            base.Update();
        }
        public virtual void Explode()
        {
            if (main != null && mainer != null)
            {
                main.points += 25;
                if (mainer.profile.localPlayer)
                {
                    Level.Add(new PointsGetManager(25, "Trap"));                   
                }
            }
            foreach (Duck duck in Level.CheckCircleAll<Duck>(position, 12))
            {
                if (duck.HasEquipment(typeof(Fuse_armor)))
                {
                    Fuse_armor armor = duck.GetEquipment(typeof(Fuse_armor)) as Fuse_armor;
                    if (armor.team != 1 && armor._equippedDuck.profile.localPlayer)
                    {
                        DuckNetwork.SendToEveryone(new NMActivateDevice(armor, damage, 1));
                        armor.health -= damage;
                        armor.bleeding = true;
                        DuckNetwork.SendToEveryone(new NMApplyBleed(armor));
                    }
                }
            }
            Level.Add(new ExplosionPart(position.x, position.y, true));
            int num = 6;
            if (Graphics.effectsLevel < 2)
            {
                num = 3;
            }
            for (int i = 0; i < num; i++)
            {
                float dir = i * 60f + Rando.Float(-10f, 10f);
                float dist = Rando.Float(20f, 20f);
                ExplosionPart ins = new ExplosionPart(position.x + (float)(Math.Cos((double)Maths.DegToRad(dir)) * (double)dist), position.y - (float)(Math.Sin((double)Maths.DegToRad(dir)) * (double)dist), true);
                Level.Add(ins);
            }
            Graphics.FlashScreen();
            SFX.Play("explode", 1f, 0f, 0f, false);
        }
    }
}
