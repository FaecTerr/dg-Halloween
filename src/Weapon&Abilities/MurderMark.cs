using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    public class MurderMark : Thing
    {
        public float radius1;
        public float Timer = 0.14f;
        public MurderMark(float xval, float yval) : base(xval, yval)
        {
            layer = Layer.Foreground;
        }
        public override void Update()
        {
            base.Update();
            if (Timer > 0)
            {
                Timer -= 0.01f;
            }
            else
            {
                Level.Remove(this);
            }
            radius1 += 1f;
        }
        public override void Draw()
        {
            base.Draw();
            Graphics.DrawCircle(position, radius1, Color.Purple, 2f, 1f, 32);
        }
    }
}
