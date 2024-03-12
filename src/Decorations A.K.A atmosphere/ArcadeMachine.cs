using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    [EditorGroup("Halloween|Decor|Static")]
    public class ArcadeMachine : Decoration
    {
        public SpriteMap _sprite;
        public EditorProperty<int> style;

        public ArcadeMachine(float xpos, float ypos) : base(xpos, ypos)
        {
            _sprite = new SpriteMap(GetPath("Sprites/Decorations/Machines.png"), 29, 36);
            base.graphic = _sprite;
            center = new Vec2(14.5f, 18f);
            collisionSize = new Vec2(29f, 34f);
            collisionOffset = new Vec2(-14.5f, -17f);
            base.depth = -0.5f;
            hugWalls = WallHug.Floor;
            style = new EditorProperty<int>(0, this, 0, 14, 1);

            decType = "floor";
        }

        public override void Update()
        {
            _sprite.flipH = flipHorizontal;
            base.Update();
        }

        public override void Draw()
        {
            _sprite.frame = style;
            _sprite.flipH = flipHorizontal;
            base.Draw();
        }
    }
}
