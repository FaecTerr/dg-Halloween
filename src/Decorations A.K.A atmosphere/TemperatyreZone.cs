using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    [EditorGroup("Halloween|Decor")]
    public class TemperatureZone : Thing
    {
        public SpriteMap _sprite;
        public EditorProperty<float> wide;
        public EditorProperty<float> heig;
        public EditorProperty<int> Type;

        public TemperatureZone(float xpos, float ypos) : base(xpos, ypos)
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
            Type = new EditorProperty<int>(0, this, 0, 1, 1);
        }

        public override void Update()
        {
            _sprite.frame = Type;
            foreach(Duck d in Level.CheckRectAll<Duck>(position - new Vec2(wide / 2, heig / 2), position + new Vec2(wide / 2, heig / 2)))
            {
                if (d.profile.localPlayer)
                {
                    int count = 0;
                    foreach(Temperature t in Level.current.things[typeof(Temperature)])
                    {
                        count++;
                        t.Timer = 0.5f;
                        if(t._sprite.alpha < 0.5f)
                        {
                            t._sprite.alpha += 0.002f;
                        }
                    }
                    if(count == 0)
                    {
                        Level.Add(new Temperature(d.position.x, d.position.y) { fr = Type });
                    }
                }
            }
            base.Update();
        }


        public override void Draw()
        {
            _sprite.frame = Type;
            Graphics.DrawRect(position - new Vec2(wide/2, heig/2), position + new Vec2(wide / 2, heig / 2), Color.Blue, 0.5f, false, 2);
            base.Draw();
        }
    }
}
