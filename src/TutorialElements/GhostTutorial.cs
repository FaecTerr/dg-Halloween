using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    [EditorGroup("Halloween|Tutorial")]
    public class GhostTutorial : Ghost
    {
        public GhostTutorial(float xpos, float ypos) : base(xpos, ypos)
        {
            maxStorage = 999;
            delay = 7;
            charges = 3;
        }

    }
}
