using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    /*[EditorGroup("Halloween|Decor|Animated")]
    public class Rats : Thing
    {
        public SpriteMap _sprite;

        public Rats(float xpos, float ypos) : base(xpos, ypos)
        {
            _sprite = new SpriteMap(GetPath("Sprites/Decorations/Pumpkin.png"), 19, 19);
            base.graphic = _sprite;
            hugWalls = WallHug.Floor;
            center = new Vec2(9.5f, 8.5f);
            collisionSize = new Vec2(17f, 17f);
            collisionOffset = new Vec2(-8.5f, -8.5f);
            base.depth = -0.9f;
            _sprite.AddAnimation("default", 1f, true, new int[] { 0 });
            _sprite.AddAnimation("appearence", 0.1f, false, new int[] { 0, 1, 2, 3 });
            _sprite.AddAnimation("idle", 0.05f, true, new int[] { 7, 8 });
            _sprite.AddAnimation("overwatch", 0.05f, false, new int[] { 14, 15, 16, 17, 18, 19, 20 });
            _sprite.AddAnimation("laugh", 0.05f, false, new int[] { 21, 22, 23, 24, 25, 26, 27 });
        }

        public override void Update()
        {
            Duck d = Level.CheckCircle<Duck>(position, 16) as Duck;
            _sprite.flipH = flipHorizontal;
            if (d != null && _sprite.currentAnimation == "default")
            {
                _sprite.SetAnimation("appearence");
            }
            if (_sprite.currentAnimation == "appearence" && _sprite.finished)
            {
                _sprite.SetAnimation("idle");
            }
            if (_sprite.currentAnimation == "idle")
            {
                float f = Rando.Float(1f);
                if (f > 0.997)
                {
                    int n = Rando.Int(1, 4);
                    if (n == 1)
                    {
                        _sprite.SetAnimation("overwatch");
                    }
                    if (n == 2)
                    {
                        _sprite.SetAnimation("laugh");
                    }
                    if (n == 3)
                    {
                        _sprite.SetAnimation("rageIN");
                    }
                }
            }
            if ((_sprite.currentAnimation == "overwatch" || _sprite.currentAnimation == "laugh" || _sprite.currentAnimation == "rageOut") && _sprite.finished)
            {
                _sprite.SetAnimation("idle");
            }
            if (_sprite.currentAnimation == "rageIN" && _sprite.finished)
            {
                _sprite.SetAnimation("rageIdle");
            }
            if (_sprite.currentAnimation == "rageIdle" && _sprite.finished)
            {
                _sprite.SetAnimation("rageOut");
            }
            base.Update();
        }
    }*/
}
