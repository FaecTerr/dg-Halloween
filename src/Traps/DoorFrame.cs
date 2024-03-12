using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    [EditorGroup("Halloween")]
    public class DoorFrame : Thing
    {
        public SpriteMap _sprite;
        public DoorFrame(float xval, float yval) : base(xval, yval)
        {
            _sprite = new SpriteMap(Mod.GetPath<Halloween>("Sprites/DoorFrame.png"), 12, 36, false);
            base.graphic = _sprite;
            center = new Vec2(6f, 18f);
            _sprite.frame = 2;
            collisionOffset = new Vec2(-6f, -18f);
            collisionSize = new Vec2(12f, 36f);
            graphic = _sprite;
            hugWalls = WallHug.Floor;
            
        }
        public override void Update()
        {
            base.Update();
        }
    }
}
