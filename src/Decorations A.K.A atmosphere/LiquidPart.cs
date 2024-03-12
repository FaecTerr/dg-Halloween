using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    public class LiquidPart : PhysicsObject
    {
        protected SpriteMap sprite;
        public float life = 1f;
        public LiquidPart(float xval, float yval) : base(xval, yval)
        {
            sprite = new SpriteMap(GetPath("Sprites/Decorations/LiquidParts.png"), 8, 6);
            graphic = sprite;
            center = new Vec2(3f, 3f);
            collisionOffset = new Vec2(-3f, -3f);
            collisionSize = new Vec2(6f, 6f);
            sprite.frame = Rando.Int(0, 2);
            thickness = 0f;
            weight = 0f;
            bouncy = 0.3f;
            friction = 0.02f;
            sprite.AddAnimation("idle", 0.04f, true, new int[] { 0, 1});
            sprite.SetAnimation("idle");
        }
        public override void Update()
        {
            angleDegrees = Maths.PointDirection(position, position+new Vec2(hSpeed, vSpeed)) + 3.14159f/2;
            if (grounded)
            {
                angle = 0;
                sprite.frame = 3;
                life -= 0.001f;
            }
            alpha = life;
            if(life <= 0)
            {
                Level.Remove(this);
            }
            base.Update();
        }
    }
}