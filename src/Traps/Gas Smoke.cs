using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    public class GasSmoke : PhysicsObject
    {
        public float Timer;

        public Vec2 move;
        float angleIncrement;
        float scaleDecrement;
        float fastGrowTimer;
        int damageFrame;

        public Duck mainer;
        public Fuse_armor main;

        public GasSmoke(float xpos, float ypos, float stayTime = 6f) : base(xpos, ypos)
        {
            xscale = 1;
            yscale = xscale;
            angle = Maths.DegToRad(Rando.Float(360f));
            fastGrowTimer = 0.6f;
            Timer = stayTime;
            angleIncrement = 0.1f;
            scaleDecrement = 0.02f;
            move = new Vec2();
            center = new Vec2(0f, 0f);
            collisionOffset = new Vec2(-1f, -1f);
            collisionSize = new Vec2(2f, 2f);
            gravMultiplier = 0.1f;

            GraphicList graphicList = new GraphicList();
            Sprite graphic1 = new Sprite("smoke", 0.0f, 0.0f);
            graphic1.alpha = 0.3f;
            graphic1.depth = 1f;
            graphic1.CenterOrigin();
            graphic1.color = Color.Yellow;
            graphicList.Add(graphic1);

            Sprite graphic2 = new Sprite("smokeBack", 0.0f, 0.0f);
            graphic2.depth = 0.1f;
            graphic2.alpha = 0.3f;
            graphic2.color = Color.Green;
            graphic2.CenterOrigin();
            graphicList.Add(graphic2);

            graphic = graphicList;
            center = new Vec2(0.0f, 0.0f);
            depth = 1f;

            thickness = 0;
            weight = 0;
        }

        public override void Update()
        {
            foreach (Murder f in Level.CheckCircleAll<Murder>(position, 48))
            {
                if (f.poisonFrames <= 0 /*&& Level.CheckLine<Block>(position, f.position) == null*/)
                {
                    f.poisonFrames = 20;
                    if(f.health <= 3)
                    {
                        if (main != null && mainer != null)
                        {
                            main.points += 75;
                            if (mainer.profile.localPlayer)
                            {
                                Level.Add(new PointsGetManager(75, "Kill"));
                                if (Halloween.upd.challengeID == 10)
                                {
                                    if (Halloween.upd.challengeProgression < Halloween.upd.challengeTarget)
                                    {
                                        Halloween.upd.challengeProgression += 1;
                                    }
                                }
                                if (Halloween.upd.challengeID == 11)
                                {
                                    if (Halloween.upd.challengeProgression < Halloween.upd.challengeTarget)
                                    {
                                        Halloween.upd.challengeProgression += 1;
                                    }
                                }
                            }
                        }
                    }
                    if(f._equippedDuck != null && f.team == 2)
                    {
                        if (f._equippedDuck.profile.localPlayer)
                        {
                            Level.Add(new Toxic(position.x, position.y));
                        }
                        f._equippedDuck.hSpeed *= 0f;
                    }
                }
            }
            angle += angleIncrement;

            if (Timer > 0)
            {
                Timer -= 0.01666666f;
            }
            else
            {
                xscale -= scaleDecrement;
                scaleDecrement += 0.001f;
            }
            if ((double)fastGrowTimer > 0)
            {
                fastGrowTimer -= 0.01f;
                xscale += 0.06f;
            }
            yscale = xscale;

            if (xscale < 0.1)
            {
                Level.Remove(this);
            }
        }
    }
}
