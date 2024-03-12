using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    [EditorGroup("Halloween|Decor|Static")]
    public class Well : Thing
    {
        public SpriteMap _sprite;

        public Well(float xpos, float ypos) : base(xpos, ypos)
        {
            _sprite = new SpriteMap(GetPath("Sprites/Decorations/Well.png"), 22, 38);
            base.graphic = _sprite;
            center = new Vec2(11f, 19f);
            collisionSize = new Vec2(20f, 36f);
            collisionOffset = new Vec2(-10f, -18f);
            base.depth = -0.5f;
            hugWalls = WallHug.Floor;
        }

        public override void Update()
        {
            _sprite.flipH = flipHorizontal;
            base.Update();
        }
    }
}
