using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    public class NMApplyBleed : NMEvent
    {
        public Fuse_armor f;
        public NMApplyBleed()
        {
        }

        public NMApplyBleed(Fuse_armor f)
        {
            this.f = f;
        }

        public override void Activate()
        {
            if (f != null)
            {
                f.bleeding = true;
            }
        }
    }
}
