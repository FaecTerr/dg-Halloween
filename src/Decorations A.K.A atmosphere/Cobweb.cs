using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    [EditorGroup("Halloween|Decor|Static")]
    public class Cobweb : Decoration
    {
        public SpriteMap _sprite;

        public EditorProperty<int> style;
        public Cobweb(float xpos, float ypos) : base(xpos, ypos)
        {
            _sprite = new SpriteMap(GetPath("Sprites/Decorations/cobweb.png"), 12, 12);
            base.graphic = _sprite;
            center = new Vec2(6f, 6f);
            collisionSize = new Vec2(12f, 12f);
            collisionOffset = new Vec2(-6f, -6f);
            base.depth = -0.5f;
            hugWalls = WallHug.Ceiling;
            style = new EditorProperty<int>(0, this, 0, 2, 1);
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
            _sprite.frame = style;
            base.Draw();
        }
    }
}
