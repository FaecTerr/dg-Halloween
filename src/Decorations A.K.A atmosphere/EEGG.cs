using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    [EditorGroup("Halloween|Decor|EG")]
    public class EEGG : Thing
    {
        public SpriteMap _sprite;

        public EditorProperty<int> style;
        public EEGG(float xpos, float ypos) : base(xpos, ypos)
        {
            _sprite = new SpriteMap(GetPath("Sprites/Decorations/EasterEggsSpriteSheet.png"), 64, 64);
            base.graphic = _sprite;
            center = new Vec2(32f, 32f);
            collisionSize = new Vec2(62f, 62f);
            collisionOffset = new Vec2(-31f, -31f);
            scale = new Vec2(0.5f, 0.5f);
            base.depth = -0.5f;
            style = new EditorProperty<int>(0, this, 0, 3, 1);
        }

        public override void Update()
        {
            _sprite.frame = style;
            base.Update();
        }
    }
}
