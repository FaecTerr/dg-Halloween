using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    public class NMApplySave : NMEvent
    {
        public Fuse_armor f;
        public Kapkan k;
        public NMApplySave()
        {
        }

        public NMApplySave(Fuse_armor f, Kapkan k)
        {
            this.f = f;
            this.k = k;
        }

        public override void Activate()
        {
            if (f != null && k != null)
            {
                if (k.catched != null)
                {
                    k.catched = null;
                    k.Break();
                }
            }
        }
    }
}
