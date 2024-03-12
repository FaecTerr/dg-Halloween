using System;
using System.Collections.Generic;

namespace DuckGame.Halloween
{
    public class HForceWave : Thing
    {
        public Fuse_armor own;
        public bool isInvis;
        public bool isClaws;

        public HForceWave(float xpos, float ypos, int dir, float alphaSub, float speed, float speedv, Duck own) : base(xpos, ypos, null)
        {
            offDir = (sbyte)dir;
            graphic = new Sprite("sledgeForce", 0f, 0f);
            center = new Vec2((float)graphic.w, (float)graphic.h);
            _alphaSub = alphaSub;
            _speed = speed;
            _speedv = speedv;
            _collisionSize = new Vec2(6f, 26f);
            _collisionOffset = new Vec2(-3f, -13f);
            graphic.flipH = (offDir <= 0);
            owner = own;
            base.depth = -0.7f;
        }
        
        public override void Update()
        {
            if (base.alpha > 0.1f)
            {
                foreach(Fuse_armor f in Level.CheckRectAll<Fuse_armor>(topLeft, bottomRight))
                {
                    if (f.team == 1 && !f.dead)
                    {                 
                        if (owner != null)
                        {
                            Duck du = owner as Duck;
                            SFX.Play(GetPath("SFX/HammerSmash.wav"));
                        }
                        f.health -= 50;
                        if (isInvis)
                        {
                            if (own._equippedDuck != null)
                            {
                                if (isClaws && Halloween.upd.challengeID == 2)
                                {
                                    if (Halloween.upd.challengeProgression < Halloween.upd.challengeTarget)
                                    {
                                        Halloween.upd.challengeProgression += 1;
                                    }
                                }
                                if (!isClaws && Halloween.upd.challengeID == 0)
                                {
                                    if (Halloween.upd.challengeProgression < Halloween.upd.challengeTarget)
                                    {
                                        Halloween.upd.challengeProgression += 1;
                                    }
                                }
                                if (own._equippedDuck.profile.localPlayer)
                                {
                                    Level.Add(new PointsGetManager(120, "Kill", "+20 : " + LanguageManager.GetPhrase("PMSK")));
                                }
                            }
                            own.points += 120;
                        }
                        else
                        {
                            if (own._equippedDuck != null)
                            {
                                if (own._equippedDuck.profile.localPlayer)
                                {
                                    if (isClaws && Halloween.upd.challengeID == 2)
                                    {
                                        if (Halloween.upd.challengeProgression < Halloween.upd.challengeTarget)
                                        {
                                            Halloween.upd.challengeProgression += 1;
                                        }
                                    }
                                    if (!isClaws && Halloween.upd.challengeID == 0)
                                    {
                                        if (Halloween.upd.challengeProgression < Halloween.upd.challengeTarget)
                                        {
                                            Halloween.upd.challengeProgression += 1;
                                        }
                                    }
                                    Level.Add(new PointsGetManager(100, "Kill"));
                                }
                            }
                            own.points += 100;
                        }
                        Level.Remove(this);
                        return;
                    }
                }
                foreach (Device d in Level.CheckRectAll<Device>(base.topLeft, base.bottomRight))
                {
                    if (d != null)
                    {
                        SFX.Play(GetPath("SFX/HammerSmash.wav"));
                        if (owner != null)
                        {
                            Duck du = owner as Duck;
                            if (du.profile.localPlayer)
                            {
                                if (Halloween.upd.challengeID == 3)
                                {
                                    if (Halloween.upd.challengeProgression < Halloween.upd.challengeTarget)
                                    {
                                        Halloween.upd.challengeProgression += 1;
                                    }
                                }
                            }
                            if (d is Claymore)
                            {
                                if (du.profile.localPlayer)
                                {
                                    Level.Add(new PointsGetManager(25, "Destroy"));
                                }
                                own.points += 25;
                            }
                            if (d is Grzmot)
                            {
                                if (du.profile.localPlayer)
                                {
                                    Level.Add(new PointsGetManager(20, "Destroy"));
                                }
                                own.points += 20;
                            }
                            if (d is EDD)
                            {
                                if (du.profile.localPlayer)
                                {
                                    Level.Add(new PointsGetManager(35, "Destroy"));
                                }
                                own.points += 35;
                            }
                            if (d is Kapkan)
                            {
                                if (du.profile.localPlayer)
                                {
                                    Level.Add(new PointsGetManager(30, "Destroy"));
                                }
                                own.points += 30;
                            }
                            if (d is GasCanister)
                            {
                                if (du.profile.localPlayer)
                                {
                                    Level.Add(new PointsGetManager(40, "Destroy"));
                                }
                                own.points += 40;
                            }
                        }
                        d.Hurt(1f);
                    }
                }
                foreach (Crate d in Level.CheckRectAll<Crate>(base.topLeft, base.bottomRight))
                {
                    if (d != null)
                    {
                        d.Hurt(30f);
                    }
                    Level.Remove(this);
                    return;
                }
                IEnumerable<Door> doors = Level.CheckRectAll<Door>(base.topLeft, base.bottomRight);
                foreach (Door hit2 in doors)
                {
                    if (owner != null)
                    {
                        Thing.Fondle(hit2, owner.connection);
                    }
                    if (!hit2.destroyed)
                    {
                        Level.Remove(this);
                        
                        hit2.Destroy(new DTImpact(this));
                        return;
                    }
                }               
            }
            base.x += (float)offDir * _speed;
            base.y += _speedv;
            base.alpha -= _alphaSub;
            if (base.alpha <= 0f)
            {
                Level.Remove(this);
            }
        }
        
        private float _alphaSub;
        private float _speed;
        private float _speedv;
        private List<Thing> _hits = new List<Thing>();
    }
}
