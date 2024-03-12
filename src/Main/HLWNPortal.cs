using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    public class HLWNPortal : Thing
    {
        public SpriteMap _sprite = new SpriteMap(Mod.GetPath<Halloween>("Sprites/Portal.png"), 50, 40);
        public HLWNPortal(float xpos, float ypos) : base(xpos, ypos)
        {
            graphic = _sprite;
            center = new Vec2(25, 20);
            collisionSize = new Vec2(50, 40);
            collisionOffset = new Vec2(-25, -20);
            _sprite.AddAnimation("idle", 0.1f, true, new int[] { 0, 1, 2, 3, 4});
            _sprite.SetAnimation("idle");
            depth = -0.7f;
            layer = Layer.Game;
        }
        public override void Update()
        {
            Duck d = Level.CheckRect<Duck>(topLeft, bottomRight) as Duck;
            if(d != null)
            {
                if (d.inputProfile.Pressed("SHOOT") || d.inputProfile.Pressed("GRAB") || d.inputProfile.Pressed("SELECT"))
                {
                    Level.current = new HalloweenLevel() { p = d.profile };
                }
            }
            base.Update();
        }
    }
}
