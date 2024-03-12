using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    public class DetectorNode : PhysicsObject
    {
        public float recoil = 0f;
        public float radius = 6f;
        public int power;
        public SpriteMap _sprite;
        public SpriteMap _light;
        public SinWave _pulse = 0.05f;
        public Fuse_armor main;
        public Duck mainer;

        public DetectorNode(float xpos, float ypos) : base(xpos, ypos)
        {
            _sprite = new SpriteMap(GetPath("Sprites/Lantern.png"), 12, 18);
            _sprite.CenterOrigin();
            _light = new SpriteMap(GetPath("Sprites/LanternLight.png"), 18, 18);
            _light.CenterOrigin();
            graphic = _sprite;
            center = new Vec2(6f, 9f);
            collisionOffset = new Vec2(-5f, -8f);
            collisionSize = new Vec2(10f, 16f);
            thickness = 0f;
            weight = 0f;
            depth = -0.6f;
        }
        public override void Update()
        {
            if(recoil > 0f)
            {
                recoil -= 0.01666666f;
                _sprite.frame = 2;
            }
            if (recoil <= 0f)
            {
                _sprite.frame = 0;
                foreach (Ghost g in Level.CheckCircleAll<Ghost>(position, radius))
                {
                    foreach (Duck d in Level.current.things[typeof(Duck)])
                    {
                        DinamicSFX.PlayDef(d.position, position, 1.5f, 32f, "SFX/GhostLantern.wav");
                    }
                    if (mainer != null && main != null)
                    {
                        main.points += 25;
                        if (mainer.profile.localPlayer)
                        {
                            
                            Level.Add(new PointsGetManager(25, "Detect"));
                        }
                    }
                    Level.Add(new OverlayMarker(g.position.x, g.position.y, 2));
                    recoil = 10f;
                    g.stunFrames = 30;
                    if (power == 1)
                    {
                        if (g._equippedDuck != null)
                        {
                            g._equippedDuck.hSpeed *= 0f;
                            g.stunFrames = 55;
                        }
                        recoil = 5f;
                    }
                }
            }
            base.Update();

        }

        public override void Draw()
        {
            if(recoil <= 0f)
            {
                for (int i = 0; i < 2; i++)
                {
                    _light.alpha = Math.Abs(i - (0.5f + _pulse*0.1f))+0.1f;
                    _light.frame = i;
                    _light.scale = new Vec2(Math.Abs(i - (0.5f + _pulse*0.1f)) + 1f, Math.Abs(i - (0.5f + _pulse*0.1f)) + 1f);
                    Graphics.Draw(_light, position.x, position.y);
                }
            }
            foreach (DetectorNode d in Level.CheckCircleAll<DetectorNode>(position, 160f))
            {
                if (d != this && Level.CheckLine<Block>(position, d.position) == null)
                {
                    if(recoil <= 0f)
                        _sprite.frame = 1;
                    Graphics.DrawLine(position, d.position, Color.Aqua, 1f);
                    d.power = 1;
                }
            }
            base.Draw();
        }
    }
}
