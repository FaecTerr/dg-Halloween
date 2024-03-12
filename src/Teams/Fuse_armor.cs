using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuckGame;

namespace DuckGame.Halloween
{
    public class Fuse_armor : Equipment, IDrawToDifferentLayers
    {
        public Duck theduck;
        public SinWave _pulse = 0.2f;
        public float intensivity;
        public float mod = 1f;
        public float num = 0.08f;
        public Vec2 pos;
        public float numb;
        public float thick;
        protected SpriteMap _sprite;
        protected SpriteMap _hp;
        protected Sprite _pickupSprite;
        protected Vec2 _barrelOffsetTL = default(Vec2);
        public int team;
        public int stepFrames = 20;
        public int health = 50;
        public int prevHealth = 50;
        public int showFrames;
        public int damageFromKapkan;
        public int points;
        public int stunFrames;
        public float targetMode = 1;

        public int removeFrames = 20;

        public int poisonFrames;
        public int tokens;

        public bool bleeding;
        public int bloodloss;

        public bool dead;
        public bool init;

        public bool coinGet;

        public SpriteMap ReplaceSprite;
        public SpriteMap ReplaceFeather;
        public SpriteMap ReplaceArm;
        public SpriteMap ReplaceQuack;
        public SpriteMap ReplaceControlled;
        public bool recolor = false;

        public virtual void loadTextures()
        {

        }

        public SpriteMap sprite(string path)
        {
            var sm = new SpriteMap(Mod.GetPath<Halloween>(path), 32, 32);
            return sm;
        }

        public SpriteMap feathersprite(string path)
        {
            var sm = new SpriteMap(Mod.GetPath<Halloween>(path), 12, 4);
            return sm;
        }
        public SpriteMap armsprite(string path)
        {
            var sm = new SpriteMap(Mod.GetPath<Halloween>(path), 16, 16);
            return sm;
        }



        public virtual void ApplyReplaceAnimation(DuckPersona persona)
        {
            ReplaceSprite.ClearAnimations();
            ReplaceSprite.AddAnimation("idle", 1f, true, new int[1]);
            ReplaceSprite.AddAnimation("run", 1f, true, 1, 2, 3, 4, 5, 6);
            ReplaceSprite.AddAnimation("jump", 1f, true, 7, 8, 9);
            ReplaceSprite.AddAnimation("slide", 1f, true, 10);
            ReplaceSprite.AddAnimation("crouch", 1f, true, 11);
            ReplaceSprite.AddAnimation("groundSlide", 1f, true, 12);
            ReplaceSprite.AddAnimation("dead", 1f, true, 13);
            ReplaceSprite.AddAnimation("netted", 1f, true, 14);
            ReplaceSprite.AddAnimation("listening", 1f, true, 16);
            ReplaceSprite.SetAnimation("idle");
        }

        public virtual void ApplyFeatherAnimation(DuckPersona persona)
        {
            ReplaceFeather.ClearAnimations();
            ReplaceFeather.CloneAnimations(persona.featherSprite);
        }

        public void ApplyTextureStuff(DuckPersona d)
        {
            if (ReplaceArm != null)
            {
                ReplaceArm.texture = Halloween.CorrectTexture(ReplaceArm.texture, recolor, d.color);
                ReplaceArm.CenterOrigin();
            }
            if (ReplaceSprite != null)
            {
                ReplaceSprite.texture = Halloween.CorrectTexture(ReplaceSprite.texture, recolor, d.color);
                ReplaceSprite.CenterOrigin();
                ApplyReplaceAnimation(d);
            }
            if (ReplaceFeather != null)
            {
                ReplaceFeather.texture = Halloween.CorrectTexture(ReplaceFeather.texture, recolor, d.color);
                ReplaceFeather.CenterOrigin();
                ApplyFeatherAnimation(d);
            }
            if (ReplaceQuack != null)
            {
                ReplaceQuack.texture = Halloween.CorrectTexture(ReplaceQuack.texture, recolor, d.color);
                ReplaceQuack.CenterOrigin();
            }
            if (ReplaceControlled != null)
            {
                ReplaceControlled.CenterOrigin();
                ReplaceControlled.texture = Halloween.CorrectTexture(ReplaceControlled.texture, recolor, d.color);
            }
        }

        public override void Equip(Duck d)
        {
            base.Equip(d);

            loadTextures();
            ApplyTextureStuff(d.persona);

            if (ReplaceArm != null)
                d.persona.armSprite = ReplaceArm.CloneMap();
            if (ReplaceSprite != null)
                d.persona.sprite = ReplaceSprite.CloneMap();
            if (ReplaceFeather != null)
                d.persona.featherSprite = ReplaceFeather.CloneMap();
            if (ReplaceQuack != null)
                d.persona.quackSprite = ReplaceQuack.CloneMap();
            if (ReplaceControlled != null)
                d.persona.controlledSprite = ReplaceControlled.CloneMap();
            d.InitProfile();

        }

        public Fuse_armor(float xval, float yval) : base(xval, yval)
        {
            _hp = new SpriteMap(GetPath("Sprites/HealthBar.png"), 64, 20);
            _hp.CenterOrigin();
            thickness = 10f;
            _equippedDepth = 4;
            center = new Vec2(16, 16);
            collisionOffset = new Vec2(-12f, -12f);
            collisionSize = new Vec2(24f, 24f);
            _equippedCollisionOffset = new Vec2(-12f, -12f);
            _equippedCollisionSize = new Vec2(24f, 24f);
            _equippedDepth = 3;
            team = 0;
        }

        protected override bool OnDestroy(DestroyType type = null)
        {
            return false;
        }


        public Sprite pickupSprite
        {
            get
            {
                return _pickupSprite;
            }
            set
            {
                _pickupSprite = value;
            }
        }

        public virtual void Bloodlost()
        {
            health -= 2;
            Level.Add(new Blood(position.x, position.y));
        }

        public override void Update()
        {
            if(mod < targetMode - 0.01666666f)
            {
                mod += 0.01666666f;
            }
            else if (mod > targetMode + 0.01666666f)
            {
                mod -= 0.01666666f;
            }
            else
            {
                mod = targetMode;
            }
            if (owner != null)
            {
                Duck d = owner as Duck;
                
                theduck = _equippedDuck;
                d._disarmDisable = 5;
                if (bleeding)
                {
                    if (bloodloss <= 0)
                    {
                        bloodloss = 30;
                        if (d.profile.localPlayer)
                        {
                            Level.Add(new Bloodlust(position.x, position.y));
                        }
                        Bloodlost();
                    }
                    else
                    {
                        bloodloss--;
                    }
                }
                if (stunFrames > 0)
                {
                    d.hSpeed *= 0f;
                    stunFrames--;
                }
                if (poisonFrames > 0)
                {
                    if(theduck != null)
                    {
                        theduck.hSpeed *= 0.87f;
                    }
                    poisonFrames--;
                    if (poisonFrames == 1)
                    {
                        health -= 3;
                    }
                }
                if (points >= 150 && !coinGet)
                {
                    coinGet = true;
                    if (d.profile.localPlayer)
                    {
                        if (PlayerStats.Data.ELO > 900)
                        {
                            d.profile.littleManBucks += 10;
                        }
                        if (PlayerStats.Data.ELO > 1300)
                        {
                            MonoMain.core.gachas += 1;
                        }
                        if (PlayerStats.Data.ELO > 1700)
                        {
                            MonoMain.core.rareGachas += 1;
                        }

                        d.profile.littleManBucks += 50;
                        MonoMain.core.gachas += 1;
                        //MonoMain.core.rareGachas += 1;
                    }
                    points -= 150;
                    tokens += 1;

                }
                if (health < 0)
                {
                    health = 0;
                }
                if (damageFromKapkan > 0)
                {
                    damageFromKapkan--;
                }
                if (stepFrames > 0)
                {
                    stepFrames--;
                }
                if (health <= 0 && _equippedDuck.dead == false)
                {
                    _equippedDuck.Kill(new DTFade());
                }
                if (theduck != null)
                {
                    init = true;
                }
                if (!dead)
                {
                    if (theduck.dead == true || (init && theduck == null) || health <= 0)
                    {
                        dead = true;
                    }
                }
                if (dead)
                {
                    if (health != 0)
                    {
                        health = 0;
                    }
                }
                if (showFrames > 0)
                {
                    showFrames--;
                }
                base.Update();
                if (!dead)
                {
                    foreach (Duck duck in Level.current.things[typeof(Duck)])
                    {
                        if (duck != null)
                        {
                            if (!duck.dead && duck.HasEquipment(typeof(Murder)) && !duck.profile.localPlayer)
                            {
                                Murder f = duck.GetEquipment(typeof(Murder)) as Murder;
                                if (f != null)
                                {
                                    if (duck.hSpeed != 0f && (duck.position - d.position).length < ((320) + 80) && f.stepFrames <= 0 && duck.grounded && duck != d)
                                    {
                                        int step = Rando.Int(1, 6);
                                        float silence = 0.75f;
                                        float pitch = 0.5f;
                                        f.stepFrames = 35;
                                        float panning = d.position.x - duck.position.x;
                                        if (Math.Abs(panning) <= 16f)
                                        {
                                            panning = 0;
                                        }
                                        if (Math.Abs(panning) <= 32f)
                                        {
                                            if (panning < 0)
                                            {
                                                panning = 0.35f;
                                            }
                                            else
                                            {
                                                panning = -0.35f;
                                            }
                                        }
                                        if (Math.Abs(panning) <= 48f)
                                        {
                                            if (panning < 0)
                                            {
                                                panning = 0.5f;
                                            }
                                            else
                                            {
                                                panning = -0.5f;
                                            }
                                        }
                                        else
                                        {
                                            if (panning < 0)
                                            {
                                                panning = 0.75f;
                                            }
                                            else
                                            {
                                                panning = -0.75f;
                                            }
                                        }
                                        if (Math.Abs(duck.hSpeed) > 2f)
                                        {
                                            silence = 1f;
                                            pitch = 1f;
                                            stepFrames = 20;
                                        }
                                        if (Math.Abs(duck.hSpeed) < 1f)
                                        {
                                            silence = 0.25f;
                                            pitch = 0f;
                                            stepFrames = 70;
                                        }
                                        float volume = (1f - ((duck.position - position).length / (((320) + 80)))) * silence;
                                        foreach (Block b in Level.CheckLineAll<Block>(duck.position, d.position))
                                        {
                                            volume *= 0.8f;
                                            pitch -= 0.5f;
                                            if (d.position.x > duck.position.x)
                                            {
                                                panning -= 0.1f;
                                            }
                                            else
                                            {
                                                panning += 0.1f;
                                            }
                                        }
                                        if (volume > 1)
                                        {
                                            volume = 1;
                                        }
                                        if (pitch < -1)
                                        {
                                            pitch = -1;
                                        }
                                        if (pitch > 1)
                                        {
                                            pitch = 1;
                                        }
                                        if (panning < -1)
                                        {
                                            panning = -1;
                                        }
                                        if (panning > 1)
                                        {
                                            panning = 1;
                                        }
                                        if (volume < 0)
                                        {
                                            volume = 0;
                                        }
                                        if (step == 1)
                                        {
                                            SFX.Play(GetPath("SFX/Step1.wav"), volume, pitch, panning);
                                        }
                                        if (step == 2)
                                        {
                                            SFX.Play(GetPath("SFX/Step2.wav"), volume, pitch, panning);
                                        }
                                        if (step == 3)
                                        {
                                            SFX.Play(GetPath("SFX/Step3.wav"), volume, pitch, panning);
                                        }
                                        if (step == 4)
                                        {
                                            SFX.Play(GetPath("SFX/Step4.wav"), volume, pitch, panning);
                                        }
                                        if (step == 5)
                                        {
                                            SFX.Play(GetPath("SFX/Step5.wav"), volume, pitch, panning);
                                        }
                                        if (step == 6)
                                        {
                                            SFX.Play(GetPath("SFX/Step6.wav"), volume, pitch, panning);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (removeFrames > 0)
                {
                    removeFrames--;
                }
                else
                {
                    Level.Remove(this);
                }
            }
        }

        public virtual void OnDrawLayer(Layer pLayer)
        {
            
        }

        public Vec2 barrelOffset
        {
            get
            {
                return _barrelOffsetTL - center + _extraOffset;
            }
        }
        public Vec2 barrelVector
        {
            get
            {
                return Offset(barrelOffset) - Offset(barrelOffset + new Vec2(-1f, 0f));
            }
        }
    }
}
