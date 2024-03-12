using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    [EditorGroup("Halloween|Decor|Static")]
    public class MeatHanger : Thing 
    {
        public SinWave _float = 0.05f;
        public SpriteMap _chain;
        public SpriteMap _sprite;

        public EditorProperty<int> length;
        public EditorProperty<int> style;
        public EditorProperty<float> delta;
        public MeatHanger(float xpos, float ypos) : base(xpos, ypos)
        {
            _sprite = new SpriteMap(GetPath("Sprites/Decorations/MeatHanger.png"), 18, 40);
            _chain = new SpriteMap(GetPath("Sprites/Decorations/HangChain.png"), 10, 16);
            _chain.CenterOrigin();
            base.graphic = _sprite;
            center = new Vec2(9f, 0f);
            collisionSize = new Vec2(18f, 4f);
            collisionOffset = new Vec2(-9f, 0f);
            base.depth = -0.5f;
            length = new EditorProperty<int>(1, this, 0, 16, 1);
            style = new EditorProperty<int>(1, this, 1, 8, 1);
            delta = new EditorProperty<float>(0.5f, this, 0f, 1.2f, 0.05f);
        }

        public override void Update()
        {
            _sprite.frame = style-1;
            angle = _float*0.5f*delta;
            base.Update();
        }

        public override void Draw()
        {       

            _sprite.flipH = flipHorizontal;

            for (int i = 0; i < length; i++)
            {
                _chain.frame = 0;
                if (i == 0)
                {
                    _chain.frame = 2;
                }
                Graphics.Draw(_chain, position.x, position.y - 16 * i);
            }
            base.Draw();
        }
    }
}
