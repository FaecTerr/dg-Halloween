using System;
using System.Collections.Generic;

namespace DuckGame.Halloween
{
    public class SkinChanger
    {
        public SpriteMap ReplaceSprite;
        public SpriteMap ReplaceFeather;
        public SpriteMap ReplaceArm;
        public SpriteMap ReplaceQuack;
        public SpriteMap ReplaceControlled;
        public bool recolor = false;
        public string equipmentPrefix;
        public Dictionary<string, string> equipment;
        public SkinChanger()
        {
        }

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

        public virtual void Equip(Duck d)
        {
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
    }
}
