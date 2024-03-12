using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    public class Screenshake : Thing
    {
        public float length;
        public float power;

        public Vec2 Shake;
        public Vec2 defPos;

        public Screenshake(float l, float p) : base()
        {
            length = l;
            power = p;
        }
        public override void Update()
        {
            if (Level.current.camera != null)
            {
                if (!(Level.current is Editor))
                {
                    if (defPos == new Vec2(0, 0))
                    {
                        defPos = Level.current.camera.position;
                    }
                    else
                    {
                        if (length > 0)
                        {
                            Shake = new Vec2(Rando.Float(-power, power), Rando.Float(-power, power));
                            Level.current.camera.position = defPos - Shake;
                            length -= 0.01666666f;
                            if (length <= 0)
                            {
                                Level.current.camera.position = defPos;
                                Shake = Vec2.Zero;
                            }
                        }
                    }
                }
            }
            base.Update();
        }
    }
}
