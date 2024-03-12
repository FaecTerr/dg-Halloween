using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    [EditorGroup("Halloween|Decor|Static")]
    public class ThingTop : Thing
    {
        public int delay;
        public SpriteMap _sprite;
        public EditorProperty<int> style;
        public SinWave _pulse = 0.5f;

        public ThingTop(float xpos, float ypos) : base(xpos, ypos, null)
        {
            _sprite = new SpriteMap(GetPath("Sprites/ThingTop.png"), 48, 48);
            graphic = _sprite;
            center = new Vec2(24f, 24f);
            _collisionSize = new Vec2(16f, 16f);
            _collisionOffset = new Vec2(-8f, -8f);
            base.depth = 0.9f;
            style = new EditorProperty<int>(0, this, 0, 5, 1);
        }

        public override void Update()
        {
            base.Update();
            _sprite.angle = 0;
            angle = 0;
            if (delay > 0)
            {
                delay--;
                _sprite.angle = _pulse*0.06f;
                angle = _pulse*0.06f;
            }
            if(style == 3)
            {
                Duck d = Level.CheckRect<Duck>(topLeft - new Vec2(16f, 16f), bottomRight + new Vec2(16f, 16f));
                if (d != null)
                {
                    if (Math.Abs(d.hSpeed) >= 0.5f && delay <= 0)
                    {
                        delay = 40;
                        foreach (Duck du in Level.current.things[typeof(Duck)])
                        {
                            DinamicSFX.Play(du.position, position, 2f, 1f, "SFX/grassmoving.wav", 64f);
                        }
                    }
                }
            }
            
        }

        public override void Draw()
        {
            _sprite.frame = style;

            _sprite.flipH = flipHorizontal;

            base.Draw();
        }
        
    }
}
