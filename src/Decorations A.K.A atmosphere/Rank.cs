using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    public class Rank : Thing
    {
        public SpriteMap _sprite = new SpriteMap(Mod.GetPath<Halloween>("Sprites/RankIcons.png"), 16, 16);

        public Rank(float xpos, float ypos) : base(xpos, ypos)
        {
            base.graphic = _sprite;
            center = new Vec2(8f, 8f);
            collisionSize = new Vec2(16f, 16f);
            collisionOffset = new Vec2(-8f, -8f);
            base.depth = -0.5f;

            _sprite.frame = (int)(Halloween.upd.ELO/100);
        }
    }
}
