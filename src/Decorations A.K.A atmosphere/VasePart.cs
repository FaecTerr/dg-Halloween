using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    public class VasePart : PhysicsObject
    {
        protected SpriteMap sprite;
        public float life = 1f;
        public float randoSpin = Rando.Float(-1f, 1f);
        public VasePart(float xval, float yval) : base(xval, yval)
        {
            sprite = new SpriteMap(GetPath("Sprites/Decorations/VasePart.png"), 6, 6);
            graphic = sprite;
            center = new Vec2(3f, 3f);
            collisionOffset = new Vec2(-3f, -3f);
            collisionSize = new Vec2(6f, 6f);
            sprite.frame = Rando.Int(0, 3);
            thickness = 0f;
            weight = 0f;
        }
        public override void Update()
        {
            _angle += randoSpin;
            randoSpin *= 0.99f;
            if (grounded)
            {
                randoSpin *= 0.99f;
                life -= 0.01f;
            }
            alpha = life;
            if (life <= 0)
            {
                Level.Remove(this);
            }
            base.Update();
        }
    }
}