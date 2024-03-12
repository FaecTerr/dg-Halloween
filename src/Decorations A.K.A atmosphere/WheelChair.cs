using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    [EditorGroup("Halloween|Decor|Static")]
    public class WheelChair : Thing
    {
        public SpriteMap _sprite;

        public WheelChair(float xpos, float ypos) : base(xpos, ypos)
        {
            _sprite = new SpriteMap(GetPath("Sprites/Decorations/WheelChair.png"), 24, 24);
            base.graphic = _sprite;
            center = new Vec2(12f, 12f);
            collisionSize = new Vec2(24f, 24f);
            collisionOffset = new Vec2(-12f, -12f);
            depth = -0.5f;
            hugWalls = WallHug.Floor;
        }

        public override void Update()
        {
            _sprite.flipH = flipHorizontal;
            base.Update();
        }
        public override void Draw()
        {
            _sprite.flipH = flipHorizontal;
            base.Draw();
        }
    }
}
