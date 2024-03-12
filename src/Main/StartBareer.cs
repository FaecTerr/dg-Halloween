using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    [EditorGroup("Halloween")]
    public class StartBareer : Block
    {
        private SpriteMap _sprite;
        public EditorProperty<float> time;
        public EditorProperty<bool> vertical;
        private float Time = -1f;
        public StartBareer(float xpos, float ypos) : base(xpos, ypos)
        {
            time = new EditorProperty<float>(5f, this, 1f, 300f, 0.1f, null, false, false);
            _sprite = new SpriteMap(GetPath("Sprites/ColoredSquare.png"), 4, 16);
            _sprite.frame = 10;
            _visibleInGame = false;
            base.graphic = _sprite;
            center = new Vec2(2f, 8f);
            collisionSize = new Vec2(4f, 16f);
            collisionOffset = new Vec2(-2f, -8f);
            thickness = 0f;
            base.depth = -0.5f;
            vertical = new EditorProperty<bool>(false);
        }
        public override void EditorPropertyChanged(object property)
        {
            if (vertical == true)
            {
                collisionSize = new Vec2(16f, 4f);
                collisionOffset = new Vec2(-8f, -2f);
            }
            base.EditorPropertyChanged(property);
        }
        public override void Update()
        {
            base.Update();
            collisionSize = new Vec2(4f, 16f);
            collisionOffset = new Vec2(-2f, -8f);
            if (vertical == true)
            {
                collisionSize = new Vec2(16f, 4f);
                collisionOffset = new Vec2(-8f, -2f);
            }
            if(Time == -1f)
            {
                Time = time;
            }
            if(Time>0f)
            {
                Time -= 0.01666666f;
            }
            if(Time <= 0f)
            {
                Level.Remove(this);
            }
        }
        public override void Draw()
        {
            base.Draw();
            _sprite.angleDegrees = 0;
            if (vertical == true)
            {
                _sprite.angleDegrees = 90;
            }
        }
    }
}
