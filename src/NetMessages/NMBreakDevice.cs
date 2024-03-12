using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    public class NMBreakDevice : NMEvent
    {
        public Device d;
        public NMBreakDevice()
        {
        }

        public NMBreakDevice(Device d)
        {
            this.d = d;
        }

        public override void Activate()
        {
            if (d != null)
            {
                float dist = 0f;
                float dir = 60f + Rando.Float(-10f, 10f);
                ExplosionPart ins = new ExplosionPart(d.position.x + (float)(Math.Cos((double)Maths.DegToRad(dir)) * (double)dist), d.position.y - (float)(Math.Sin((double)Maths.DegToRad(dir)) * (double)dist), true);
                Level.Add(ins);
                SFX.Play("explode", 1f, 0f, 0f, false);
                Level.Remove(d);
            }
        }
    }
}
