using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    [EditorGroup("Halloween|Decor|Static")]
    public class Desk : Thing
    {
        public SpriteMap _sprite;

        public Desk(float xpos, float ypos) : base(xpos, ypos)
        {
            _sprite = new SpriteMap(GetPath("Sprites/Decorations/Desk.png"), 50, 34);
            base.graphic = _sprite;
            center = new Vec2(15f, 17f);
            collisionSize = new Vec2(48f, 32f);
            collisionOffset = new Vec2(-24f, -16f);
            base.depth = -0.5f;
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
