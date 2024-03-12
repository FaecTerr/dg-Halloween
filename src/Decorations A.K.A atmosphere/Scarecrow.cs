using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    [EditorGroup("Halloween|Decor|Static")]
    public class Scarecrow : Decoration
    {
        public SpriteMap _sprite;

        public Scarecrow(float xpos, float ypos) : base(xpos, ypos)
        {
            _sprite = new SpriteMap(GetPath("Sprites/Decorations/Scarecrow.png"), 18, 40);
            base.graphic = _sprite;
            center = new Vec2(9f, 20f);
            collisionSize = new Vec2(16f, 38f);
            collisionOffset = new Vec2(-8f, -19f);
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
