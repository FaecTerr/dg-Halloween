using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    public class OverlayMarker : Thing
    {
        public SpriteMap _sprite;

        public float existTime = 1f;
        public float outTime = 0.2f;
        public float scaleIncrease = 0.1f;
        public float alphaIncrease = -0.1f;

        public OverlayMarker(float xpos, float ypos, int fram) : base(xpos, ypos)
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
            depth = 0.78f;
            if(existTime > 0)
            {
                existTime -= 0.01666666f;
            }
            else
            {
                if(outTime > 0)
                {
                    outTime -= 0.01666666f;
                    scale += new Vec2(scaleIncrease, scaleIncrease);
                    alpha += alphaIncrease;
                }
                else
                {
                    Level.Remove(this);
                }
            }
            base.Update();
        }
    }
}
