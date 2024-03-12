using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    public class DecorationsPlacer : Thing
    {
        public SpriteMap _sprite;
        private bool init;

        public DecorationsPlacer(float xpos, float ypos, int fram) : base(xpos, ypos)
        {
            _sprite = new SpriteMap(GetPath("Sprites/PlayerIcons.png"), 18, 18);
            base.graphic = _sprite;
            _sprite.frame = fram;
            center = new Vec2(9f, 9f);
            collisionSize = new Vec2(18f, 18f);
            collisionOffset = new Vec2(-9f, -9f);
            layer = Layer.Foreground;
            depth = 0.78f;
        }

        public override void Update()
        {
            if(!(Level.current is Editor))
            {
                if (Level.current.initialized && !init)
                {
                    init = true;
                    foreach (Block b in Level.current.things[typeof(Block)])
                    {
                        //if(Level.CheckRect<Block>())
                    }
                }
            }
            base.Update();
        }
    }
}
