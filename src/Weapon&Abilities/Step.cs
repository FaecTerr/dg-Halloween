using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    public class Step : PhysicsObject
    {
        public float time = 20f;
        public SpriteMap _sprite;
        public SpriteMap _arrow;
        public Step prevStep;
        public Step(float xpos, float ypos) : base(xpos, ypos)
        {
            _sprite = new SpriteMap(GetPath("Sprites/Step.png"), 8, 2);
            _arrow = new SpriteMap(GetPath("Sprites/arrow.png"), 5, 5);
            _arrow.CenterOrigin();
            _sprite.CenterOrigin();
            graphic = _sprite;
            center = new Vec2(4f, 1f);
            collisionOffset = new Vec2(-4, 0.5f);
            collisionSize = new Vec2(8f, 1f);
            alpha = 0f;
            thickness = 0f;
            weight = 0f;
        }
        public override void Update()
        {
            base.Update();         
            time -= 0.01666666f;
            if(time < 15)
            {
                _sprite.frame = 1;
            }
            if (time < 10)
            {
                _sprite.frame = 2;
            }
            if (time < 5)
            {
                _sprite.frame = 3;
            }
            if (time < 0)
            {
                Level.Remove(this);
            }
        }

        public override void Draw()
        {
            if (alpha > 0.5f && prevStep != null)
            {
                Graphics.DrawLine(position, prevStep.position, Color.White, 0.5f);
                _arrow.angle = Maths.PointDirection(position, prevStep.position);
                Graphics.Draw(_arrow, 0, (position.x + prevStep.position.x) / 2, (position.y + prevStep.position.y) / 2);
            }
            base.Draw();
        }
    }
}
