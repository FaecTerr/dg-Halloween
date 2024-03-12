using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    [EditorGroup("Halloween|Decor|Animated")]
    public class LiquidMass : Decoration
    {
        public SpriteMap sprite;
        public bool small = Rando.Int(1) == 1 ? true : false;

        public int animation;

        public LiquidMass(float xval, float yval) : base(xval, yval)
        {
            sprite = new SpriteMap(GetPath("Sprites/Decorations/LiquidMass.png"), 16, 12);
            graphic = sprite;
            center = new Vec2(8f, 6f);
            collisionOffset = new Vec2(-7f, -5f);
            collisionSize = new Vec2(14f, 10f);

            if (small)
            {
                sprite.frame = Rando.Int(4, 7);
            }
            else
            {
                sprite.frame = Rando.Int(0, 3);
            }

            hugWalls = WallHug.Floor;
        }

        public override void Update()
        {
            if (small)
            {
                if(animation <= 0)
                {
                    int fr = sprite.frame + 1;

                    if(fr > 7)
                    {
                        fr = 4;
                    }

                    sprite.frame = fr;
                    animation = 13;
                }
                else
                {
                    animation--;
                }
            }
            else
            {
                if (animation <= 0)
                {
                    int fr = sprite.frame + 1;

                    if (fr > 3)
                    {
                        fr = 0;
                    }

                    sprite.frame = fr;
                    animation = 13;
                }
                else
                {
                    animation--;
                }
            }
            base.Update();
        }
    }
}
