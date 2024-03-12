using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    public class BreakableByMovementDecor : PhysicsObject
    {
        protected SpriteMap sprite;
        public PhysicsObject particle;

        public BreakableByMovementDecor(float xval, float yval) : base(xval, yval)
        {
            thickness = 0f;
            weight = 0f;
        }

        public virtual void Break()
        {

        }

        public override void Update()
        {
            Duck d = Level.CheckRect<Duck>(topLeft, bottomRight);
            if(d != null)
            {
                if (Math.Abs(d.hSpeed) + Math.Abs(d.vSpeed) >= 2f)
                {
                    Break();
                    Level.Remove(this);
                }
            }
            base.Update();
        }
    }
}
