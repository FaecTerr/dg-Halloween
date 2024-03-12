using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    [EditorGroup("Halloween|Tutorial|Traps")]
    public class Kapkan : Device
    {
        public SpriteMap _sprite;
        private float setting = 0f;
        private float _shakeVal;
        public Duck catched;
        public bool triggered;
        public bool deactivate;

        public Kapkan(float xpos, float ypos) : base(xpos, ypos)
        {
            _sprite = new SpriteMap(Mod.GetPath<Halloween>("Sprites/Kapkan.png"), 32, 16, false);
            graphic = _sprite;
            _sprite.frame = 0;
            center = new Vec2(16f, 8f);
            collisionSize = new Vec2(26f, 14f);
            collisionOffset = new Vec2(-13f, -7f);
            setTime = 1.2f;
            CheckRect = new Vec2(24f, 0f);
            damage = 5;
            lethal = true;
            name = LanguageManager.GetPhrase("KAP");
            description = LanguageManager.GetPhrase("H KAP");
        }
        public override void Break()
        {
            base.Break();
        }

        public virtual void Catch(Duck d)
        {
            if (d != null)
            {
                catched = d;
            }
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

        public override void Update()
        {
            if(!setted && owner != null)
            {
                text = "hold @SHOOT@ to place device";
            }
            if(catched != null)
            {
                _sprite.frame = 0;
                if (catched.HasEquipment(typeof(Fuse_armor)))
                {
                    Fuse_armor armor = catched.GetEquipment(typeof(Fuse_armor)) as Fuse_armor;
                    if (armor.damageFromKapkan <= 0 && !armor.dead)
                    {
                        armor.damageFromKapkan = 66;
                        armor.health -= damage;

                        if (triggered == false)
                        {
                            if (main != null && mainer != null)
                            {
                                main.points += 15;
                                if (mainer.profile.localPlayer)
                                {
                                    Level.Add(new PointsGetManager(15, "Trap"));
                                }
                            }
                            triggered = true;
                        }
                        if(armor.health <= 0)
                        {
                            if (main != null && mainer != null)
                            {
                                main.points += 75;
                                if (mainer.profile.localPlayer)
                                {
                                    Level.Add(new PointsGetManager(75, "Kill"));
                                    if (Halloween.upd.challengeID == 12)
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
                if(catched != null)
                {
                    if (catched.ragdoll == null)
                    {
                        if (!catched.dead)
                        {
                            catched.GoRagdoll();
                            catched.framesSinceRagdoll = 0;
                        }
                    }
                    if (catched.ragdoll != null)
                    {
                        catched.ragdoll.part1.position = position;
                        /*if (catched.dead)
                        {
                            if (main != null && mainer != null)
                            {
                                main.points += 75;
                                if (mainer.profile.localPlayer)
                                {
                                    Level.Add(new PointsGetManager(75, "Kill"));
                                }
                            }
                            Level.Remove(this);
                        }*/
                    }
                }
            }
            if (setted == true)
            {
                _sprite.frame = 1;
                center = new Vec2(16f, 12f);
                collisionSize = new Vec2(26f, 6f);
                collisionOffset = new Vec2(-13f, -3f);
                if (catched != null || deactivate)
                {
                    _sprite.frame = 0;
                }

                if(catched == null)
                {
                    foreach (Duck d in Level.CheckRectAll<Duck>(topLeft, bottomRight))
                    {
                        if (d.HasEquipment(typeof(Fuse_armor)))
                        {
                            Fuse_armor armor = d.GetEquipment(typeof(Fuse_armor)) as Fuse_armor;
                            if (armor.team != team && catched == null && !deactivate)
                            {
                                Catch(d);
                                foreach (Duck du in Level.current.things[typeof(Duck)])
                                {
                                    DinamicSFX.PlayDef(du.position, position, 1.5f, 48f, "SFX/KapkanActivate.wav");
                                }
                            }
                        }
                    }
                }
            }
            base.Update();
        }
    }
}
