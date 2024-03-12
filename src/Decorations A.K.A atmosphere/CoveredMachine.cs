using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    [EditorGroup("Halloween|Decor|Static")]
    public class CoveredMachine : Decoration
    {
        public SpriteMap _sprite;

        public CoveredMachine(float xpos, float ypos) : base(xpos, ypos)
        {
            _sprite = new SpriteMap(GetPath("Sprites/Decorations/CoveredMachine.png"), 37, 37);
            base.graphic = _sprite;
            center = new Vec2(18f, 18f);
            collisionSize = new Vec2(36f, 36f);
            collisionOffset = new Vec2(-18f, -18f);
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
