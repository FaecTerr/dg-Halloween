using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    public class NMActivateDevice : NMEvent
    {
        public Fuse_armor f;
        public int damage;
        public int type;
        public Device dev;
        public NMActivateDevice()
        {
        }

        public NMActivateDevice(Fuse_armor f, int i, int type)
        {
            this.f = f;
            damage = i;
            this.type = type;
        }
        public NMActivateDevice(Device d)
        {
            this.dev = d;
        }


        public override void Activate()
        {
            if (f != null)
            {
                if(type == 0)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        Level.Add(new Stunlight(f.position.x, f.position.y, 2f, 96f, 0.8f));
                    }
                    Level.Add(new OverlayMarker(f.position.x, f.position.y, 6) { existTime = 1f, outTime = 0.5f, scaleIncrease = 0.1f, alphaIncrease = -0.04f });
                    foreach (Duck d in Level.current.things[typeof(Duck)])
                    {
                        DinamicSFX.Play(d.position, f.position, 2f, 1f, "SFX/flashGrenadeExplode.wav", 108f);
                    }
                }
                if(type == 1)
                {
                    f.health -= damage;
                    f.bleeding = true;
                }
                if(type == 2)
                {
                    f.health -= damage;
                }
            }
            if(dev != null)
            {
                dev.Explode();
            }
        }
    }
}