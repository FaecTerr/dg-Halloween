using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    //[EditorGroup("Halloween|Decor")]
    public class Vase : BreakableByMovementDecor
    {
        public Vase(float xval, float yval) : base(xval, yval)
        {
            sprite = new SpriteMap(GetPath("Sprites/Decorations/Vase.png"), 12, 16);
            graphic = sprite;
            center = new Vec2(6f, 8f);
            collisionOffset = new Vec2(-5f, -7f);
            collisionSize = new Vec2(10f, 14f);
        }
    }
}
