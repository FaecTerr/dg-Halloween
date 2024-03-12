using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckGame.Halloween
{
    public class LocalOutsideBlock : Block
    {
        public LocalOutsideBlock(float x, float y) : base(x, y)
        {
            collisionSize = new Vec2(16f, 16f);
            collisionOffset = new Vec2(0f, 0f);
            thickness = 0;
        }

        public override void Update()
        {
            base.Update();

            foreach (Murder m in Level.current.things[typeof(Murder)])
            {
                if(m.duck != null)
                {
                    if (m.duck.localDuck)
                    {
                        Level.Remove(this);
                    }
                }
            }
        }
    }
}
