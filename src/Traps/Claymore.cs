using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    [EditorGroup("Halloween|Tutorial|Traps")]

    public class Claymore : Device, ISequenceItem
    {
        public SpriteMap _sprite;
        private float setting = 0f;
        private float _shakeVal;
        private Tex2D _laserTex;
        public List<Bullet> firedBullets = new List<Bullet>();
        protected Sprite _sightHit;
        public Claymore(float xpos, float ypos) : base(xpos, ypos)
        {
            base.sequence = new SequenceItem(this);
            _sequence.type = SequenceItemType.Target;
            _sprite = new SpriteMap(Mod.GetPath<Halloween>("Sprites/Claymore.png"), 12, 8, false);
            graphic = _sprite;
            _sprite.frame = 1;
            center = new Vec2(6f, 4f);
            collisionSize = new Vec2(10f, 6f);
            collisionOffset = new Vec2(-5f, -4f);
            _sightHit = new Sprite("laserSightHit", 0f, 0f);
            _sightHit.CenterOrigin();
            setTime = 0.5f;
            CheckRect = new Vec2(8f, 0f);
            damage = 35;
            lethal = false;
            name = LanguageManager.GetPhrase("CL");
            description = LanguageManager.GetPhrase("H CL");
        }
        public override void PreSet()
        {
            base.PreSet();
        }
        public virtual void Explode()
        {
            if (main != null && mainer != null)
            {
                main.points += 35;
                if (mainer.profile.localPlayer)
                {
                    Level.Add(new PointsGetManager(35, "Trap"));                    
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
                ExplosionPart ins = new ExplosionPart(position.x + (float)(Math.Cos(Maths.DegToRad(dir)) * dist), position.y - (float)(Math.Sin(Maths.DegToRad(dir)) * dist), true);
                Level.Add(ins);
            }
            Graphics.FlashScreen();
            SFX.Play("explode", 1f, 0f, 0f, false);

            foreach(Murder m in Level.CheckCircleAll<Murder>(position, 8))
            {
                if(Level.CheckLine<Block>(position, m.position) == null)
                {
                    DuckNetwork.SendToEveryone(new NMActivateDevice(m, damage, 2));
                    m.health -= damage;
                }
            }
        }
        public override void Set()
        {
            base.Set();
            if(mainer != null && main != null)
            {
                main.points += 10;
                if (mainer.profile.localPlayer)
                {
                    Level.Add(new PointsGetManager(10, "Deploy"));
                    
                }
            }
            foreach (Duck d in Level.current.things[typeof(Duck)])
            {
                DinamicSFX.PlayDef(d.position, position, 1.5f, 32f, "SFX/Claymore.wav");
            }
        }
        public override void Update()
        {
            base.Update();
            _sightHit.scale = new Vec2(0.5f, 0.5f);
            if(!setted && owner != null)
            {
                text = "hold @SHOOT@ to place device";
            }
            if (setted == true)
            {
                foreach (Duck d in Level.CheckRectAll<Duck>(topLeft + new Vec2(0f, -16f), bottomRight))
                {
                    if (d != null)
                    {
                        if (d.HasEquipment(typeof(Fuse_armor)))
                        {
                            Fuse_armor armor = d.GetEquipment(typeof(Fuse_armor)) as Fuse_armor;
                            if (armor.team != team)
                            {
                                Explode();
                                if (armor.health <= 0)
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
                                Break();
                            }
                        }
                    }
                }
            }
        }
        public override void Draw()
        {
            base.Draw();
            if(setted == true)
            {
                for (int i = -1; i < 2; i++)
                {
                    ATTracer tracer = new ATTracer();
                    tracer.range = 18f;
                    float a = 90f - i*35f;
                    Vec2 pos = Offset(default(Vec2));
                    tracer.penetration = 0.4f;
                    Bullet b = new Bullet(pos.x, pos.y, tracer, a, owner, false, -1f, true, true);
                    _sightHit.alpha = 0.3f;
                    _sightHit.color = Color.Red;
                    Graphics.Draw(_sightHit, b.end.x, b.end.y);
                }
            }
        }
    }
}
