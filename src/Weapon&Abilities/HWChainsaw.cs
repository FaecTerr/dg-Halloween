using System;
using System.Collections.Generic;

namespace DuckGame.Halloween
{ 
    [EditorGroup("Halloween|Murderer")]
    public class HWChainsaw : Gun
    {
        public Fuse_armor main;
        public List<Fuse_armor> hitted = new List<Fuse_armor>();
        public StateBinding _angleOffsetBinding = new StateBinding("_hold", -1, false, false);
        public StateBinding _throwSpinBinding = new StateBinding("_throwSpin", -1, false, false);
        public StateBinding _gasBinding = new StateBinding("_gas", -1, false, false);
        public StateBinding _floodBinding = new StateBinding("_flood", -1, false, false);
        public StateBinding _chainsawStateBinding = new StateFlagBinding(new string[]
        {
            "_flooded",
            "_started",
            "_throttle"
        });
        private float _hold;
        private bool _shing;
        private static bool _playedShing;
        public float _throwSpin;
        private int _framesExisting;
        private int _hitWait;
        private SpriteMap _swordSwing;
        private SpriteMap _sprite;
        private float _rotSway;
        public bool _started;
        private int _pullState = -1;
        private float _animRot;
        private float _upWait;
        private float _engineSpin;
        private float _bladeSpin;
        private float _engineResistance = 1f;
        private SinWave _idleWave = 0.6f;
        private SinWave _spinWave = 1f;
        private bool _puffClick;
        public bool _flooded;
        public float _flood;
        private bool _releasePull;
        public float _gas = 1f;
        private bool _struggling;
        private bool _throttle;
        private float _throttleWait;
        private bool _releasedSincePull;
        private int _skipDebris;
        private bool _resetDuck;
        public int refuelframes;
        public bool overheat;
        public int canStack = 3;
        public float fuel = 1f;
        private ConstantSound _sound = new ConstantSound("chainsawIdle", 0f, 0f, "chainsawIdleMulti");
        private ConstantSound _bladeSound = new ConstantSound("chainsawBladeLoop", 0f, 0f, "chainsawBladeLoopMulti");
        private ConstantSound _bladeSoundLow = new ConstantSound("chainsawBladeLoopLow", 0f, 0f, "chainsawBladeLoopLowMulti");
        private bool _smokeFlipper;
        private bool _skipSmoke;
        public Color color;
        public override float angle
        {
            get
            {
                return base.angle + _hold * (float)offDir + _animRot * (float)offDir + _rotSway * (float)offDir;
            }
            set
            {
                _angle = value;
            }
        }
        public Vec2 barrelStartPos
        {
            get
            {
                return position + (Offset(base.barrelOffset) - position).normalized * 2f;
            }
        }
        
        public HWChainsaw(float xval, float yval) : base(xval, yval)
        {
            ammo = 4;
            _ammoType = new ATLaser();
            _ammoType.range = 170f;
            _ammoType.accuracy = 0.8f;
            _type = "gun";
            _sprite = new SpriteMap("chainsaw", 29, 13, false);
            graphic = _sprite;
            center = new Vec2(8f, 7f);
            collisionOffset = new Vec2(-8f, -6f);
            collisionSize = new Vec2(20f, 11f);
            _barrelOffsetTL = new Vec2(27f, 8f);
            _fireSound = "smg";
            _fullAuto = true;
            _fireWait = 1f;
            _kickForce = 3f;
            _holdOffset = new Vec2(-4f, 4f);
            weight = 8f;
            physicsMaterial = PhysicsMaterial.Metal;
            _swordSwing = new SpriteMap("swordSwipe", 32, 32, false);
            _swordSwing.AddAnimation("swing", 0.6f, false, new int[]
            {
                0,
                1,
                1,
                2
            });
            _swordSwing.currentAnimation = "swing";
            _swordSwing.speed = 0f;
            _swordSwing.center = new Vec2(9f, 25f);
            throwSpeedMultiplier = 0.5f;
            _bouncy = 0.5f;
            _impactThreshold = 0.3f;
            collideSounds.Add("landTV");

            _editorName = "ChainSaw";
        }
        
        public override void Initialize()
        {
            _sprite = new SpriteMap("chainsaw", 29, 13, false);
            graphic = _sprite;
            base.Initialize();
        }

        public void PullEngine()
        {
            float pitch = 0f;
            if (!_flooded && _gas > 0f && (_engineResistance < 1f))
            {
                SFX.Play("chainsawFire", 1f, 0f, 0f, false);
                _started = true;
                _engineSpin = 1.5f;
                for (int i = 0; i < 2; i++)
                {
                    Level.Add(SmallSmoke.New(base.x + (float)(offDir * 4), base.y + 5f));
                }
                _flooded = false;
                _flood = 0f;
            }
            else
            {
                if (_flooded && _gas > 0f)
                {
                    SFX.Play("chainsawFlooded", 0.9f, Rando.Float(-0.2f, 0.2f), 0f, false);
                    _engineSpin = 1.6f;
                }
                else
                {
                    if (_gas == 0f || Rando.Float(1f) > 0.3f)
                    {
                        SFX.Play("chainsawPull", 1f, pitch, 0f, false);
                    }
                    else if (fuel > 0f)
                    {
                        SFX.Play("chainsawFire", 1f, pitch, 0f, false);
                    }
                    _engineSpin = 0.8f;
                }
                if (Rando.Float(1f) > 0.8f)
                {
                    _flooded = false;
                    _flood = 0f;
                }
            }
            _engineResistance -= 0.5f;
            if (_gas > 0f)
            {
                int num = _flooded ? 4 : 2;
                for (int j = 0; j < num; j++)
                {
                    Level.Add(SmallSmoke.New(base.x + (float)(offDir * 4), base.y + 5f));
                }
            }
        }

        public override void ReturnToWorld()
        {
            _throwSpin = 90f;
        }
        
        public override void Update()
        {
            base.Update();
            if(refuelframes > 0)
            {
                refuelframes--;
                color = Color.LightGray;
            }
            if (fuel <= 0f)
            {
                overheat = true;
            }
            if (fuel >= 1f)
            {
                overheat = false;
                color = Color.White;
            }
            if (fuel <= 1f && !_throttle && refuelframes <= 0 && overheat == false)
            {
                fuel += 0.005f;
                color = Color.White;
            }
            if (overheat == true)
            {
                color = Color.OrangeRed;
                fuel += 0.01f;
            }
            if (_swordSwing.finished)
            {
                _swordSwing.speed = 0f;
            }
            if (_hitWait > 0)
            {
                _hitWait--;
            }
            if (_gas < 0.01f)
            {
                ammo = 0;
            }
            _framesExisting++;
            if (_framesExisting > 100)
            {
                _framesExisting = 100;
            }
            float pitch = 0f;
            _sound.lerpVolume = ((_started && !_throttle) ? 0.6f : 0f);
            _sound.pitch = pitch;
            if (_started)
            {
                if (!_puffClick && _idleWave > 0.9f)
                {
                    _skipSmoke = !_skipSmoke;
                    if (_throttle || !_skipSmoke)
                    {
                        Level.Add(SmallSmoke.New(base.x + (float)(offDir * 4), base.y + 5f, _smokeFlipper ? -0.1f : 0.8f, 0.7f));
                        _smokeFlipper = !_smokeFlipper;
                        _puffClick = true;
                    }
                }
                else if (_puffClick && _idleWave < 0f)
                {
                    _puffClick = false;
                }
                if (_pullState < 0)
                {
                    float extraShake = 1f + Maths.NormalizeSection(_engineSpin, 1f, 2f) * 2f;
                    float wave = _idleWave;
                    if (extraShake > 1f)
                    {
                        wave = _spinWave;
                    }
                    handOffset = Lerp.Vec2Smooth(handOffset, new Vec2(0f, 2f + wave * extraShake), 0.23f);
                    _holdOffset = Lerp.Vec2Smooth(_holdOffset, new Vec2(1f, 2f + wave * extraShake), 0.23f);
                    float extraShake2 = Maths.NormalizeSection(_engineSpin, 1f, 2f) * 3f;
                    _rotSway = _idleWave.normalized * extraShake2 * 0.03f;
                }
                else
                {
                    _rotSway = 0f;
                }
                if (_triggerHeld)
                {
                    if (_releasedSincePull)
                    {
                        if (!_throttle)
                        {
                            _throttle = true;
                            if(fuel > 0f && overheat == false)
                                SFX.Play("chainsawBladeRevUp", 0.5f, pitch, 0f, false);
                        }
                        _engineSpin = Lerp.FloatSmooth(_engineSpin, 4f, 0.1f, 1f);
                    }
                }
                else
                {
                    if (_throttle)
                    {
                        _throttle = false;
                        if (_engineSpin > 1.7f && fuel > 0f && overheat == false)
                        {
                            SFX.Play("chainsawBladeRevDown", 0.5f, pitch, 0f, false);
                        }
                    }
                    _engineSpin = Lerp.FloatSmooth(_engineSpin, 0f, 0.1f, 1f);
                    _releasedSincePull = true;
                }
            }
            else
            {
                _releasedSincePull = false;
                _throttle = false;
            }
            _bladeSound.lerpSpeed = 0.1f;
            if(fuel > 0f && overheat == false)
                _throttleWait = Lerp.Float(_throttleWait, _throttle ? 1f : 0f, 0.07f);
            else
            {
                _throttleWait = Lerp.Float(_throttleWait, 0f, 0.07f);
            }
            _bladeSound.lerpVolume = ((_throttleWait > 0.96f) ? 0.6f : 0f);
            if (_struggling)
            {
                _bladeSound.lerpVolume = 0f;
            }
            _bladeSoundLow.lerpVolume = ((_throttleWait > 0.96f && _struggling) ? 0.6f : 0f);
            _bladeSound.pitch = pitch;
            _bladeSoundLow.pitch = pitch;
            if (owner == null)
            {
                collisionOffset = new Vec2(-8f, -6f);
                collisionSize = new Vec2(13f, 11f);
            }
            else if (base.duck != null && (base.duck.sliding || base.duck.crouch))
            {
                collisionOffset = new Vec2(-8f, -6f);
                collisionSize = new Vec2(6f, 11f);
            }
            else
            {
                collisionOffset = new Vec2(-8f, -6f);
                collisionSize = new Vec2(10f, 11f);
            }
            if (owner != null)
            {
                
                _resetDuck = false;
                if (_pullState == -1)
                {
                    if (!_started)
                    {
                        handOffset = Lerp.Vec2Smooth(handOffset, new Vec2(0f, 2f), 0.25f);
                        _holdOffset = Lerp.Vec2Smooth(_holdOffset, new Vec2(1f, 2f), 0.23f);
                    }
                    _upWait = 0f;
                }
                else if (_pullState == 0)
                {
                    _animRot = Lerp.FloatSmooth(_animRot, -0.4f, 0.15f, 1f);
                    handOffset = Lerp.Vec2Smooth(handOffset, new Vec2(-2f, -2f), 0.25f);
                    _holdOffset = Lerp.Vec2Smooth(_holdOffset, new Vec2(-4f, 4f), 0.23f);
                    if (_animRot <= -0.35f)
                    {
                        _animRot = -0.4f;
                        _pullState = 1;
                        PullEngine();
                    }
                    _upWait = 0f;
                }
                else if (_pullState == 1)
                {
                    _releasePull = false;
                    _holdOffset = Lerp.Vec2Smooth(_holdOffset, new Vec2(2f, 3f), 0.23f);
                    handOffset = Lerp.Vec2Smooth(handOffset, new Vec2(-4f, -2f), 0.23f);
                    _animRot = Lerp.FloatSmooth(_animRot, -0.5f, 0.07f, 1f);
                    if (_animRot < -0.45f)
                    {
                        _animRot = -0.5f;
                        _pullState = 2;
                    }
                    _upWait = 0f;
                }
                else if (_pullState == 2)
                {
                    if (_releasePull || !_triggerHeld)
                    {
                        _releasePull = true;
                        if (_started)
                        {
                            handOffset = Lerp.Vec2Smooth(handOffset, new Vec2(0f, 2f + _idleWave.normalized), 0.23f);
                            _holdOffset = Lerp.Vec2Smooth(_holdOffset, new Vec2(1f, 2f + _idleWave.normalized), 0.23f);
                            _animRot = Lerp.FloatSmooth(_animRot, 0f, 0.1f, 1f);
                            if (_animRot > -0.07f)
                            {
                                _animRot = 0f;
                                _pullState = -1;
                            }
                        }
                        else
                        {
                            _holdOffset = Lerp.Vec2Smooth(_holdOffset, new Vec2(-4f, 4f), 0.24f);
                            handOffset = Lerp.Vec2Smooth(handOffset, new Vec2(-2f, -2f), 0.24f);
                            _animRot = Lerp.FloatSmooth(_animRot, -0.4f, 0.12f, 1f);
                            if (_animRot > -0.44f)
                            {
                                _releasePull = false;
                                _animRot = -0.4f;
                                _pullState = 3;
                                _holdOffset = new Vec2(-4f, 4f);
                                handOffset = new Vec2(-2f, -2f);
                            }
                        }
                    }
                    _upWait = 0f;
                }
                else if (_pullState == 3)
                {
                    _releasePull = false;
                    _upWait += 0.1f;
                    if (_upWait > 6f)
                    {
                        _pullState = -1;
                    }
                }
                _bladeSpin += _engineSpin;
                while (_bladeSpin >= 1f && overheat==false && fuel > 0f)
                {
                    _bladeSpin -= 1f;
                    int f = _sprite.frame + 1;
                    if (f > 15)
                    {
                        f = 0;
                    }
                    _sprite.frame = f;
                }
                _engineSpin = Lerp.FloatSmooth(_engineSpin, 0f, 0.1f, 1f);
                _engineResistance = Lerp.FloatSmooth(_engineResistance, 1f, 0.01f, 1f);
                _hold = -0.4f;
                center = new Vec2(8f, 7f);
                _framesSinceThrown = 0;
            }
            else
            {
                _rotSway = 0f;
                _shing = false;
                _animRot = Lerp.FloatSmooth(_animRot, 0f, 0.18f, 1f);
                if (_framesSinceThrown == 1)
                {
                    _throwSpin = base.angleDegrees;
                }
                _hold = 0f;
                base.angleDegrees = _throwSpin;
                center = new Vec2(8f, 7f);
                bool spinning = false;
                bool againstWall = false;
                if ((Math.Abs(hSpeed) + Math.Abs(vSpeed) > 2f || !base.grounded) && gravMultiplier > 0f)
                {
                    if (!base.grounded)
                    {
                        Block b = Level.CheckRect<Block>(position + new Vec2(-8f, -6f), position + new Vec2(8f, -2f), null);
                        if (b != null)
                        {
                            againstWall = true;
                        }
                    }
                    if (!againstWall && !_grounded && Level.CheckPoint<IPlatform>(position + new Vec2(0f, 8f), null, null) == null)
                    {
                        if (offDir > 0)
                        {
                            _throwSpin += (Math.Abs(hSpeed) + Math.Abs(vSpeed)) * 1f + 5f;
                        }
                        else
                        {
                            _throwSpin -= (Math.Abs(hSpeed) + Math.Abs(vSpeed)) * 1f + 5f;
                        }
                        spinning = true;
                    }
                }
                if (!spinning || againstWall)
                {
                    _throwSpin %= 360f;
                    if (_throwSpin < 0f)
                    {
                        _throwSpin += 360f;
                    }
                    if (againstWall)
                    {
                        if (Math.Abs(_throwSpin - 90f) < Math.Abs(_throwSpin + 90f))
                        {
                            _throwSpin = Lerp.Float(_throwSpin, 90f, 16f);
                        }
                        else
                        {
                            _throwSpin = Lerp.Float(-90f, 0f, 16f);
                        }
                    }
                    else if (_throwSpin > 90f && _throwSpin < 270f)
                    {
                        _throwSpin = Lerp.Float(_throwSpin, 180f, 14f);
                    }
                    else
                    {
                        if (_throwSpin > 180f)
                        {
                            _throwSpin -= 360f;
                        }
                        else if (_throwSpin < -180f)
                        {
                            _throwSpin += 360f;
                        }
                        _throwSpin = Lerp.Float(_throwSpin, 0f, 14f);
                    }
                }
            }
            if(owner != null)
            {
                Duck d = owner as Duck;
                d.hMax = 12;
                d._disarmDisable = 30;
                if (d.grounded)
                    canStack = 5;
                //d.gravMultiplier = 0.80f;
            }
            if (base.duck != null)
            {
                base.duck.frictionMult = 1f;
                if (_pullState == -1)
                {
                    if (!_throttle)
                    {
                        _animRot = MathHelper.Lerp(_animRot, 0.3f, 0.2f);
                        handOffset = Lerp.Vec2Smooth(handOffset, new Vec2(-2f, 2f), 0.25f);
                        _holdOffset = Lerp.Vec2Smooth(_holdOffset, new Vec2(-3f, 4f), 0.23f);
                    }
                    else if (_shing)
                    {
                        _animRot = MathHelper.Lerp(_animRot, -1.8f, 0.4f);
                        handOffset = Lerp.Vec2Smooth(handOffset, new Vec2(1f, 0f), 0.25f);
                        _holdOffset = Lerp.Vec2Smooth(_holdOffset, new Vec2(1f, 2f), 0.23f);
                        if (_animRot < -1.5f)
                        {
                            _shing = false;
                        }
                    }
                    else if (base.duck.crouch)
                    {
                        _animRot = MathHelper.Lerp(_animRot, 0.6f, 0.2f);
                        handOffset = Lerp.Vec2Smooth(handOffset, new Vec2(1f, 0f), 0.25f);
                        _holdOffset = Lerp.Vec2Smooth(_holdOffset, new Vec2(1f, 2f), 0.23f);
                    }
                    else if (base.duck.inputProfile.Down("UP"))
                    {
                        _animRot = MathHelper.Lerp(_animRot, -0.9f, 0.2f);
                        handOffset = Lerp.Vec2Smooth(handOffset, new Vec2(1f, 0f), 0.25f);
                        _holdOffset = Lerp.Vec2Smooth(_holdOffset, new Vec2(1f, 2f), 0.23f);
                    }
                    else
                    {
                        _animRot = MathHelper.Lerp(_animRot, 0f, 0.2f);
                        handOffset = Lerp.Vec2Smooth(handOffset, new Vec2(1f, 0f), 0.25f);
                        _holdOffset = Lerp.Vec2Smooth(_holdOffset, new Vec2(1f, 2f), 0.23f);
                    }
                }
            }
            else if (!_resetDuck && base.prevOwner != null)
            {
                PhysicsObject t = base.prevOwner as PhysicsObject;
                if (t != null)
                {
                    t.frictionMult = 1f;
                }
                _resetDuck = true;
            }
            if (_skipDebris > 0)
            {
                _skipDebris++;
            }
            if (_skipDebris > 3)
            {
                _skipDebris = 0;
            }
            _struggling = false;
            if (owner != null && _started && _throttle && !_shing && fuel > 0f && overheat == false)
            {
                refuelframes = 40;
                fuel -= 0.005f;
                color = Color.LightSlateGray;
                Duck d = owner as Duck;
                d.hMax = 24;
                (Offset(base.barrelOffset) - position).Normalize();
                Offset(base.barrelOffset);
                owner.hSpeed += 0.17f * offDir;
                Duck hit = Level.CheckLine<Duck>(barrelStartPos, base.barrelPosition) as Duck;
                Block wallHit = Level.CheckLine<Block>(barrelStartPos, base.barrelPosition, null);
                if (owner != null)
                {
                    Duck o = owner as Duck;
                    if (o.HasEquipment(typeof(Fuse_armor)) && main == null)
                    {
                        Fuse_armor f = o.GetEquipment(typeof(Fuse_armor)) as Fuse_armor;
                        main = f;
                    }
                    IEnumerable<MaterialThing> j = Level.CheckLineAll<MaterialThing>(barrelStartPos, base.barrelPosition);
                    foreach (MaterialThing t2 in j)
                    {
                        if (t2 is RainforcableFloor)
                        {
                            RainforcableFloor r = t2 as RainforcableFloor;
                            if (r.Unbreakable == false && r.rainforced == false)
                            {
                                r.health = 0;
                                fuel = 0;
                                overheat = true;
                            }
                        }
                        if (t2 is RainforcableWall)
                        {
                            RainforcableWall r = t2 as RainforcableWall;
                            if (r.Unbreakable == false && r.rainforced == false)
                            {
                                r.health = 0;
                                fuel = 0;
                                overheat = true;
                            }
                        }
                        if (t2 is Device)
                        {
                            Device dev = t2 as Device;
                            dev.Break();
                            if (d.profile.localPlayer)
                            {
                                /*if (Halloween.upd.challengeID == 3)
                                {
                                    if (Halloween.upd.challengeProgression < Halloween.upd.challengeTarget)
                                    {
                                        Halloween.upd.challengeProgression += 1;
                                    }
                                }*/
                            }
                            if (dev != null)
                            {
                                if (dev is Claymore)
                                {
                                    if (d.profile.localPlayer)
                                    {
                                        Level.Add(new PointsGetManager(20, "Destroy"));
                                    }
                                    main.points += 25;
                                }
                                if (dev is Grzmot)
                                {
                                    if (d.profile.localPlayer)
                                    {
                                        Level.Add(new PointsGetManager(10, "Destroy"));
                                    }
                                    main.points += 20;
                                }
                                if (dev is EDD)
                                {
                                    if (d.profile.localPlayer)
                                    {
                                        Level.Add(new PointsGetManager(35, "Destroy"));
                                    }
                                    main.points += 35;
                                }
                                if (dev is Kapkan)
                                {
                                    if (d.profile.localPlayer)
                                    {
                                        Level.Add(new PointsGetManager(25, "Destroy"));
                                    }
                                    main.points += 30;
                                }
                                if (dev is GasCanister)
                                {
                                    if (d.profile.localPlayer)
                                    {
                                        Level.Add(new PointsGetManager(15, "Destroy"));
                                    }
                                    main.points += 40;
                                }
                            }
                        }
                        if (t2 is Block && !(t2 is Door) && canStack > 0 && !d.crouch)
                        {
                            if (owner != null)
                            {
                                canStack--;
                                d.vSpeed -= 1;
                                d.hSpeed -= 1.5f * offDir;
                            }
                        }
                        if (t2.Hurt((t2 is Door) ? 1.8f : 0.5f))
                        {
                            if (duck != null && duck.sliding && t2 is Door && (t2 as Door)._jammed)
                            {
                                t2.Destroy(new DTImpale(this));
                                fuel = 0;
                                overheat = true;
                            }
                            else
                            {
                                fuel -= 0.015f;
                                _struggling = true;
                                if (duck != null)
                                {
                                    duck.frictionMult = 4f;
                                }
                                if (_skipDebris == 0)
                                {
                                    _skipDebris = 1;
                                    Vec2 point = Collision.LinePoint(barrelStartPos, base.barrelPosition, t2.rectangle);
                                    if (point != Vec2.Zero)
                                    {
                                        point += base.barrelVector * Rando.Float(0f, 3f);
                                        Vec2 dir = -base.barrelVector.Rotate(Rando.Float(-0.2f, 0.2f), Vec2.Zero);
                                        if (t2.physicsMaterial == PhysicsMaterial.Wood)
                                        {
                                            Thing ins = WoodDebris.New(point.x, point.y);
                                            ins.hSpeed = dir.x * 3f;
                                            ins.vSpeed = dir.y * 3f;
                                            Level.Add(ins);
                                        }
                                        else if (t2.physicsMaterial == PhysicsMaterial.Metal)
                                        {
                                            Thing ins2 = Spark.New(point.x, point.y, Vec2.Zero, 0.02f);
                                            ins2.hSpeed = dir.x * 3f;
                                            ins2.vSpeed = dir.y * 3f;
                                            Level.Add(ins2);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (hit != null)
                {
                    if (hit.HasEquipment(typeof(Fuse_armor)))
                    {
                        Fuse_armor f = hit.GetEquipment(typeof(Fuse_armor)) as Fuse_armor;
                        if (f.team == 1 && !f.dead)
                        {
                            fuel = 0;
                            overheat = true;
                            f.bleeding = true;
                            DuckNetwork.SendToEveryone(new NMApplyBleed(f));
                            f.health -= 5;
                            if (main != null)
                            {
                                if (!hitted.Contains(f))
                                {
                                    f.health -= 30;
                                    hitted.Add(f);
                                    main.points += 100;
                                    if (d.profile.localPlayer)
                                    {
                                        if (Halloween.upd.challengeID == 1)
                                        {
                                            if (Halloween.upd.challengeProgression < Halloween.upd.challengeTarget)
                                            {
                                                Halloween.upd.challengeProgression += 1;
                                            }
                                        }
                                        Level.Add(new PointsGetManager(100, "Kill"));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        
        public override void Draw()
        {
            _playedShing = false;
            if (_swordSwing.speed > 0f)
            {
                if (base.duck != null)
                {
                    _swordSwing.flipH = (base.duck.offDir <= 0);
                }
                _swordSwing.alpha = 0.4f;
                _swordSwing.position = position;
                _swordSwing.depth = base.depth + 1;
                _swordSwing.Draw();
            }
            Graphics.DrawRect(position + new Vec2(-16f, 6f), position + new Vec2(16f, 12f), Color.White, 1f, false, 1f);
            Graphics.DrawRect(position + new Vec2(-14f, 8f), position + new Vec2(-14f + fuel*28f, 10f), color, 1f, true, 1f);
            if (base.duck != null && (_pullState == 1 || _pullState == 2))
            {
                Graphics.DrawLine(Offset(new Vec2(-2f, -2f)), base.duck.armPosition + new Vec2(handOffset.x * (float)offDir, handOffset.y), Color.White, 1f, base.duck.depth + 10 - 1);
            }
            base.Draw();
        }
        
        public override void OnPressAction()
        {
            if (!_started)
            {
                if (_pullState == -1)
                {
                    _pullState = 0;
                    return;
                }
                if (_pullState == 3)
                {
                    _pullState = 1;
                    PullEngine();
                }
            }
        }
        
        public override void Fire()
        {
        }
    }
}