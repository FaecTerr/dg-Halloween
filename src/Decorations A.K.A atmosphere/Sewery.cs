using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    [EditorGroup("Halloween|Decor|Animated")]
    public class Sewery : Decoration
    {
        public SpriteMap _sprite;
        public int lifeTime;
        public Sewery(float xpos, float ypos) : base(xpos, ypos)
        {
            _sprite = new SpriteMap(GetPath("Sprites/Decorations/Sewerage.png"), 36, 24);
            base.graphic = _sprite;
            hugWalls = WallHug.Floor;
            center = new Vec2(18f, 9f);
            collisionSize = new Vec2(22f, 18f);
            collisionOffset = new Vec2(-11f, -9f);
            base.depth = -0.9f;
            _sprite.AddAnimation("idle",1f,false, new int[] { 0 });
            _sprite.AddAnimation("appearence", 0.1f, false, new int[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 });
            _sprite.AddAnimation("wat", 0.1f, false, new int[] { 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 });
            _sprite.SetAnimation("idle");
        }

        public override void Update()
        {
            if(lifeTime > 0)
            {
                lifeTime--;
            }
            Duck d = Level.CheckCircle<Duck>(position, 16) as Duck;
            if(d != null && _sprite.currentAnimation == "idle")
            {
                _sprite.SetAnimation("appearence");
                lifeTime = 600;
            }
            if(_sprite.currentAnimation == "appearence" && _sprite.finished && lifeTime <= 0)
            {
                _sprite.SetAnimation("wat");
            }
            if (_sprite.currentAnimation == "wat" && _sprite.finished)
            {
                _sprite.SetAnimation("idle");
            }
            base.Update();
        }

        public override void Draw()
        {

            _sprite.flipH = flipHorizontal;

            base.Draw();
        }
    }
}
