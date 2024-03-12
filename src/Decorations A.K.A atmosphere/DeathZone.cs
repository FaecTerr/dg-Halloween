using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    [EditorGroup("Halloween|Decor")]
    public class DeathZone : Thing
    {
        public SpriteMap _sprite;
        public EditorProperty<float> wide;
        public EditorProperty<float> heig;

        public DeathZone(float xpos, float ypos) : base(xpos, ypos)
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
            _sprite.frame = 2;
        }

        public override void Update()
        {
            foreach (Duck d in Level.CheckRectAll<Duck>(position - new Vec2(wide / 2, heig / 2), position + new Vec2(wide / 2, heig / 2)))
            {
                if (d.HasEquipment(typeof(Ghost)))
                {
                    Ghost g = d.GetEquipment(typeof(Ghost)) as Ghost;
                    g.health -= 1;
                }
            }
            base.Update();
        }


        public override void Draw()
        {
            Graphics.DrawRect(position - new Vec2(wide / 2, heig / 2), position + new Vec2(wide / 2, heig / 2), Color.IndianRed, 0.5f, false, 2);
            base.Draw();
        }
    }
}
