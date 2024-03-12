using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    [EditorGroup("Halloween|Decor|Animated")]
    public class Drive : Thing
    {
        public SpriteMap _sprite;
        public int replaysound = 3;

        public Drive(float xpos, float ypos) : base(xpos, ypos)
        {
            _sprite = new SpriteMap(GetPath("Sprites/Decorations/Drive.png"), 55, 48);
            base.graphic = _sprite;
            center = new Vec2(27.5f, 24f);
            collisionSize = new Vec2(55f, 48f);
            collisionOffset = new Vec2(-27.5f, -24f);
            base.depth = -0.5f;
        }

        public override void Update()
        {
            base.Update();
            if (replaysound > 0)
            {
                replaysound--;
            }
            else
            {
                replaysound = 15;
                foreach (Duck d in Level.current.things[typeof(Duck)])
                {
                    if (d.profile.localPlayer)
                    {
                        DinamicSFX.PlayDef(d.position, position, 1.5f, 32f, "SFX/Drive.wav");
                    }
                }
            }
        }
        public override void Draw()
        {     
            _sprite.flipH = flipHorizontal;

        Graphics.Draw(_sprite, position.x + Rando.Float(-1, 1), position.y + Rando.Float(-1, 1));
            base.Draw();
        }
    }
}
