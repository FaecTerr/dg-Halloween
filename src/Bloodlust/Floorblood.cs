using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    public class Blood : PhysicsObject
    {
        public float time = 10f;
        public SpriteMap _sprite;
        public SpriteMap _arrow;
        public Step prevStep;
        public bool gr;
        public Blood(float xpos, float ypos) : base(xpos, ypos)
        {
            _sprite = new SpriteMap(GetPath("Sprites/Blood.png"), 8, 8);            
            graphic = _sprite;
            
            center = new Vec2(4f, 4f);
            collisionOffset = new Vec2(-4, 0f);
            collisionSize = new Vec2(8f, 0f);
            thickness = 0f;
            weight = 0f;
            layer = Layer.Foreground;
            gravMultiplier = 0.6f;
            depth = 0.1f;
            _sprite.frame = 3;
            grounded = false;
            gr = false;
        }
        public override void Update()
        {
            base.Update();
            if (!grounded)
            {
                gr = false;
                _sprite.frame = 3;
            }
            if(grounded && !gr)
            {
                gr = true;
                _sprite.frame = Rando.Int(2);
            }
            if(Level.CheckRect<Spring>(topLeft, bottomRight + new Vec2(0f, 16f)) != null)
            {
                Level.Remove(this);
            }
            if(time < 3)
            {
                alpha -= 0.01666666f / 3;
            }
            time -= 0.01666666f;
            if (time < 0)
            {
                Level.Remove(this);
            }
        }
    }
}
