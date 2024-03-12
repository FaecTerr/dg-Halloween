using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;

namespace DuckGame.Halloween
{
    [EditorGroup("Halloween|Tutorial|Traps")]
    [BaggedProperty("isFatal", false)]
    public class Grzmot : Device, ISequenceItem
    {
        public bool HasExploded;
        protected SpriteMap sprite;
        public int DeployTime = 60;

        public Grzmot(float xval, float yval) : base(xval, yval)
        {
            base.sequence = new SequenceItem(this);
            _sequence.type = SequenceItemType.Target;
            sprite = new SpriteMap(GetPath("Sprites/Grzmot.png"), 16, 16, false);
            graphic = sprite;
            sprite.frame = 1;
            center = new Vec2(8f, 8f);
            collisionOffset = new Vec2(-4f, -6f);
            collisionSize = new Vec2(8f, 12f);
            _editorName = "Grzmot target";
            weight = 0.9f;
            thickness = 0.1f;
            placeable = false;
            zeroSpeed = false; 
            damage = 5;
            lethal = false;
            name = LanguageManager.GetPhrase("GR");
            description = LanguageManager.GetPhrase("H GR");
        }
        public override bool DoHit(Bullet bullet, Vec2 hitPos)
        {
            Level.Remove(this);
            return base.DoHit(bullet, hitPos);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public virtual void Flash()
        {
            for (int i = 0; i < 5; i++)
            {
                Level.Add(new Stunlight(position.x, position.y, 2f, 96f, 0.8f));
            }
        }

        public override void Explode()
        {
            Flash();

            Level.Add(new OverlayMarker(position.x, position.y, 6) { existTime = 1f, outTime = 0.5f, scaleIncrease = 0.1f, alphaIncrease = -0.04f });
            foreach (Duck d in Level.current.things[typeof(Duck)])
            {
                DinamicSFX.Play(d.position, position, 2f, 1f, "SFX/flashGrenadeExplode.wav", 108f);
            }
            Level.Remove(this);
            HasExploded = true;
        }

        public override void Update()
        {
            base.Update();
            text = "Press 'FIRE' to remove pin";
            if (owner != null)
            {
                Duck own = owner as Duck;
            }
            text = LanguageManager.GetPhrase("Throw");

            if (!setted && grounded)
            {
                _enablePhysics = false;
                grounded = true;
                _hSpeed = 0f;
                _vSpeed = 0f;
                gravMultiplier = 0f;
                setted = true;

                if (mainer != null && main != null)
                {
                    if (mainer.profile.localPlayer)
                    {
                        main.points += 5;
                        Level.Add(new PointsGetManager(5, "Deploy"));
                    }
                }

                canPickUp = false;
                frame = 2;
            }

            foreach (Duck d in Level.CheckCircleAll<Duck>(position, 60))
            {
                if(d != null)
                {
                    if (d.HasEquipment(typeof(Fuse_armor)) && setted == true && Level.CheckLine<Block>(position, d.position) == null)
                    {
                        Fuse_armor armor = d.GetEquipment(typeof(Fuse_armor)) as Fuse_armor;
                        if (armor.team == 2 && DeployTime <= 0)
                        {
                            if (armor.team == 2)
                            {
                                Explode();
                                if (d.profile.localPlayer)
                                {
                                    armor.health -= damage;
                                    if (main != null && mainer != null)
                                    {
                                        DuckNetwork.SendToEveryone(new NMActivateDevice(armor, damage, 0));
                                        main.points += 20;
                                        if (mainer.profile.localPlayer)
                                        {
                                            Level.Add(new PointsGetManager(20, "Trap"));
                                        }
                                    }
                                }
                                if (armor.health <= 0)
                                {
                                    if (main != null && mainer != null)
                                    {
                                        main.points += 75;
                                        if (mainer.profile.localPlayer)
                                        {
                                            Level.Add(new PointsGetManager(75, "Kill"));
                                            if (Halloween.upd.challengeID == 13)
                                            {
                                                if (Halloween.upd.challengeProgression < Halloween.upd.challengeTarget)
                                                {
                                                    Halloween.upd.challengeProgression += 1;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public override void OnSolidImpact(MaterialThing with, ImpactedFrom from)
        {
            base.OnSolidImpact(with, from);
            if (with != null)
            {
                if (with is Block && !(with is Door) && setted == false)
                {
                    _enablePhysics = false;
                    grounded = true;
                    _hSpeed = 0f;
                    _vSpeed = 0f;
                    gravMultiplier = 0f;
                    setted = true;
                    if (mainer != null && main != null)
                    {
                        main.points += 5;
                        if (mainer.profile.localPlayer)
                        {
                            Level.Add(new PointsGetManager(5, "Deploy"));
                            
                        }
                    }
                    canPickUp = false;
                    frame = 2;
                    if(from == ImpactedFrom.Right)
                    {
                        angleDegrees = -90;
                    }
                    if (from == ImpactedFrom.Left)
                    {
                        angleDegrees = 90;
                    }
                    if (from == ImpactedFrom.Top)
                    {
                        angleDegrees = 180;
                    }
                    if (from == ImpactedFrom.Bottom)
                    {
                        angleDegrees = 0;
                    }
                }
            }
        }

        public override void Draw()
        {
            if(setted == true && DeployTime > 0 && prevOwner != null)
            {
                DeployTime -= 2;
                Duck d = prevOwner as Duck;
                if (d.profile.localPlayer)
                {
                    Graphics.DrawCircle(position, 60-DeployTime, Color.White, 1.5f, 1f, 32);
                }
            }
            base.Draw();
        }

        protected virtual void CreatePinParticle()
        {
            GrenadePin grenadePin = new GrenadePin(x, y);
            grenadePin.hSpeed = -offDir * Rando.Float(1.5f, 2f);
            grenadePin.vSpeed = -2f;
            Level.Add(grenadePin);
            SFX.Play("pullPin", 1f, 0.0f, 0.0f, false);
        }
    }
}