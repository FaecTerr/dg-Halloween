using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    [EditorGroup("Halloween|Decor|Animated")]
    public class Bat : Decoration
    {
        public SpriteMap _sprite;

        public int spriteCheck;

        public Bat(float xpos, float ypos) : base(xpos, ypos)
        {
            _sprite = new SpriteMap(GetPath("Sprites/Decorations/Bat.png"), 12, 16);
            base.graphic = _sprite;
            center = new Vec2(6f, 8f);
            collisionSize = new Vec2(12f, 16f);
            collisionOffset = new Vec2(-6f, -8f);
            depth = -0.5f;
            hugWalls = WallHug.Ceiling;
            decType = "ceiling";
        }

        public override void Update()
        {
            _sprite.flipH = flipHorizontal;

            if (spriteCheck <= 0)
            {
                float r = Rando.Float(1);

                int t = 6;
                if (_sprite.frame > 0)
                {
                    t = 24;
                    _sprite.frame = 0;
                }
                else
                {
                    if (r > 0.85f)
                    {
                        _sprite.frame = 1;
                    }
                    else if (r > 0.7f)
                    {
                        _sprite.frame = 2;
                    }
                    else
                    {
                        _sprite.frame = 0;
                    }
                }
                spriteCheck = t;
            }
            else
            {
                spriteCheck--;
            }

            base.Update();
        }
        public override void Draw()
        {
            _sprite.flipH = flipHorizontal;
            base.Draw();
        }
    }
}
