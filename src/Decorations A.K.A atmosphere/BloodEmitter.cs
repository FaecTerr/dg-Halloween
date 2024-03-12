using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    [EditorGroup("Halloween|Decor")]
    public class BloodEmitter : Thing
    {
        public SpriteMap _sprite;
        public EditorProperty<int> intensivity;
        public int Emitting = Rando.Int(180);
        public BloodEmitter(float xpos, float ypos) : base(xpos, ypos)
        {
            _sprite = new SpriteMap(GetPath("Sprites/Decorations/BloodEmitter.png"), 16, 16);
            base.graphic = _sprite;
            center = new Vec2(8f, 8f);
            collisionSize = new Vec2(16f, 16f);
            collisionOffset = new Vec2(-8f, -8f);
            base.depth = -0.9f;
            _visibleInGame = false;
            intensivity = new EditorProperty<int>(180, this, 30, 300, 30);
        }

        public override void Update()
        {
            Duck d = Level.CheckCircle<Duck>(position, 16) as Duck;
            _sprite.flipH = flipHorizontal;
            if (Emitting % intensivity == 0)
            {
                Emitting = 0 + Rando.Int(intensivity/4);
                Level.Add(new Blood(Rando.Float(topLeft.x, bottomRight.x), position.y));
            }
            else
            {
                Emitting++;
            }
            base.Update();
        }
    }
}
