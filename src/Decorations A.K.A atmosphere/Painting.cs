using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    [EditorGroup("Halloween|Decor|Animated")]
    public class Painting : Decoration
    {
        public SpriteMap _sprite;
        public Painting(float xpos, float ypos) : base(xpos, ypos)
        {
            _sprite = new SpriteMap(GetPath("Sprites/Decorations/Painting.png"), 29, 28);
            base.graphic = _sprite;
            hugWalls = WallHug.Floor;
            center = new Vec2(14.5f, 14f);
            collisionSize = new Vec2(27f, 26f);
            collisionOffset = new Vec2(-13.5f, -13f);
            base.depth = -0.9f;
            _sprite.AddAnimation("appearence", 0.05f, false, new int[] { 0, 1, 2, 3 });
            _sprite.AddAnimation("default", 0.05f, false, new int[] { 0 });
            _sprite.AddAnimation("idle", 0.05f, true, new int[] { 0, 1});
            _sprite.AddAnimation("1", 0.1f, false, new int[] { 4, 5, 6, 7, 8 });
            _sprite.AddAnimation("2", 0.1f, false, new int[] { 9, 10, 11, 12, 13 });
            _sprite.AddAnimation("3", 0.1f, false, new int[] { 14, 15, 16, 17, 18, 19, 20, 21 });
            _sprite.AddAnimation("4", 0.1f, false, new int[] { 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38 });
            _sprite.SetAnimation("idle");
        }

        public override void Update()
        {
            Duck d = Level.CheckCircle<Duck>(position, 16) as Duck;
            _sprite.flipH = flipHorizontal;
            if (d != null && _sprite.currentAnimation == "default")
            {
                _sprite.SetAnimation("appearence");
            }
            if (_sprite.currentAnimation == "appearence" && _sprite.finished)
            {
                _sprite.SetAnimation("idle");
            }
            if (_sprite.currentAnimation == "idle")
            {
                float f = Rando.Float(1f);
                if (f > 0.997)
                {
                    int n = Rando.Int(1, 4);
                    if (n == 1)
                    {
                        _sprite.SetAnimation("1");
                    }
                    if (n == 2)
                    {
                        _sprite.SetAnimation("2");
                    }
                    if (n == 3)
                    {
                        _sprite.SetAnimation("3");
                    }
                    if(n == 4)
                    {
                        _sprite.SetAnimation("4");
                    }
                }
            }
            if ((_sprite.currentAnimation == "1" || _sprite.currentAnimation == "2" || _sprite.currentAnimation == "3" || _sprite.currentAnimation == "4") && _sprite.finished)
            {
                _sprite.SetAnimation("idle");
            }
            base.Update();
        }

        public override void Draw()
        {

            base.Draw();
        }
    }
}
