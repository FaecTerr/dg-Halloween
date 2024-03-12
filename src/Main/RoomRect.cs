using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckGame.Halloween
{
    [EditorGroup("Halloween|Scripting")]
    public class RoomRect : Thing
    {
        public EditorProperty<int> sizex;
        public EditorProperty<int> sizey;
        public EditorProperty<string> name;
        public EditorProperty<bool> outside;

        bool init;

        public RoomRect(float xpos, float ypos) : base(xpos, ypos)
        {
            sizex = new EditorProperty<int>(1, this, 1, 40, 1);
            sizey = new EditorProperty<int>(1, this, 1, 40, 1);
            name = new EditorProperty<string>("1F - Main hall");
            layer = Layer.Foreground;
            outside = new EditorProperty<bool>(false);
        }

        public override void Update()
        {
            base.Update();
            if (!(Level.current is Editor))
            {
                if (outside && !init)
                {
                    init = true;
                    Level.Add(new LocalOutsideBlock(position.x + 8, position.y + 8) { collisionSize = new Vec2(16 * sizex, 16 * sizey) });
                }
            }
        }

        public override void Draw()
        {
            base.Draw();
            if (Level.current is Editor)
            {
                Graphics.DrawRect(position + new Vec2(8, 8), position + new Vec2(16f * sizex + 8, 16f * sizey + 8), Color.White * 0.5f, 1f, false, 2f);
                Graphics.DrawDottedRect(position - new Vec2(8f, 8f), position + new Vec2(8f, 8f), Color.White * 0.5f, 1f, 0.5f, 4f);
                if (!outside)
                {
                    Graphics.DrawStringOutline(name, position + new Vec2(name.value.Length * (-4) + sizex * 8, -6f + sizey * 8), Color.White * 0.5f, Color.Black * 0.5f, 1f, null, 1f);
                }
            }
        }
    }
}
