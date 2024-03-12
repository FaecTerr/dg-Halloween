using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    [EditorGroup("Halloween|Decor|Static")]
    public class Graves : Decoration
    {
        public SpriteMap _sprite;

        public EditorProperty<int> style;
        public Graves(float xpos, float ypos) : base(xpos, ypos)
        {
            _sprite = new SpriteMap(GetPath("Sprites/Decorations/graves.png"), 15, 15);
            base.graphic = _sprite;
            center = new Vec2(7.5f, 7.5f);
            collisionSize = new Vec2(15f, 15f);
            collisionOffset = new Vec2(-7.5f, -7.5f);
            base.depth = -0.5f;
            hugWalls = WallHug.Floor;
            style = new EditorProperty<int>(0, this, 0, 5, 1);
        }

        public override void Update()
        {
            _sprite.flipH = flipHorizontal;
            _sprite.frame = style;
            base.Update();
        }
        public override void Draw()
        {
            _sprite.flipH = flipHorizontal;
            base.Draw();
        }
    }
}
