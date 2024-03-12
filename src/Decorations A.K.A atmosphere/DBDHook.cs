using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    [EditorGroup("Halloween|Decor|EG")]
    public class DBDHook : Thing
    {
        public SpriteMap _sprite;

        public DBDHook(float xpos, float ypos) : base(xpos, ypos)
        {
            _sprite = new SpriteMap(GetPath("Sprites/Decorations/DBDHook.png"), 32, 32);
            base.graphic = _sprite;
            center = new Vec2(16f, 16f);
            collisionSize = new Vec2(30f, 30f);
            collisionOffset = new Vec2(-15f, -15f);
            base.depth = -0.5f;
        }

        public override void Update()
        {
            base.Update();
        }
        public override void Draw()
        {
            _sprite.flipH = flipHorizontal;
            base.Draw();
        }
    }
}
