using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    [EditorGroup("Halloween|Decor")]
    public class SwampZone : Thing
    {
        public SpriteMap _sprite;
        public EditorProperty<float> wide;
        public EditorProperty<float> heig;

        public SwampZone(float xpos, float ypos) : base(xpos, ypos)
        {
            _sprite = new SpriteMap(GetPath("Sprites/Decorations/Temperature.png"), 16, 16);
            base.graphic = _sprite;
            center = new Vec2(8f, 8f);
            collisionSize = new Vec2(16f, 16f);
            collisionOffset = new Vec2(-8f, -8f);
            base.depth = -0.5f;
            _visibleInGame = false;
            wide = new EditorProperty<float>(8, this, 8, 480, 32);
            heig = new EditorProperty<float>(8, this, 8, 480, 32);
            _sprite.frame = 3;
        }

        public override void Update()
        {
            foreach (Duck d in Level.CheckRectAll<Duck>(position - new Vec2(wide / 2, heig / 2), position + new Vec2(wide / 2, heig / 2)))
            {
                d.vSpeed *= (d.vSpeed > 0 ? 1f : 0.97f);
            }
            base.Update();
        }


        public override void Draw()
        {
            Graphics.DrawRect(position - new Vec2(wide / 2, heig / 2), position + new Vec2(wide / 2, heig / 2), Color.Green, 0.5f, false, 2);
            base.Draw();
        }
    }
}
