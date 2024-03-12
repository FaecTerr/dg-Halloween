using System;

namespace DuckGame.Halloween
{
    [EditorGroup("Halloween|Murderer")]
    public class HWSledgeHammer : Gun
    {
        public Fuse_armor main;
        public HWSledgeHammer(float xval, float yval) : base(xval, yval)
        {
            ammo = 4;
            _ammoType = new ATLaser();
            _ammoType.range = 170f;
            _ammoType.accuracy = 0.8f;
            _type = "gun";
            _spriteAlternate = new SpriteMap(GetPath("Sprites/sledgeHammerAlt.png"), 32, 32, false);
            _spriteAlternate.center = new Vec2(16f, 14f);
            _sprite = new SpriteMap(GetPath("Sprites/sledgeHammer2.png"), 32, 32, false);
            _sledgeSwing = new SpriteMap("sledgeSwing", 32, 32, false);
            _sledgeSwing.AddAnimation("swing", 0.8f, false, new int[]
            {
                0,
                1,
                2,
                3,
                4,
                5
            });
            _sledgeSwing.currentAnimation = "swing";
            _sledgeSwing.speed = 0f;
            _sledgeSwing.center = new Vec2(16f, 16f);
            graphic = _sprite;
            center = new Vec2(16f, 14f);
            collisionOffset = new Vec2(-2f, 0f);
            collisionSize = new Vec2(4f, 18f);
            _barrelOffsetTL = new Vec2(16f, 28f);
            _fireSound = "smg";
            _fullAuto = true;
            _fireWait = 11f;
            _kickForce = 3f;
            weight = 9f;
            
            _dontCrush = true;
            base.collideSounds.Add("rockHitGround2");
            _editorName = "Cleaver";
        }
        public override void OnSoftImpact(MaterialThing with, ImpactedFrom from)
        {
            if (with is IPlatform)
            {
                for (int i = 0; i < 4; i++)
                {
                    Level.Add(Spark.New(base.barrelPosition.x + Rando.Float(-6f, 6f), base.barrelPosition.y + Rando.Float(-3f, 3f), -MaterialThing.ImpactVector(from), 0.02f));
                }
            }
        }
        
        public override void CheckIfHoldObstructed()
        {
            Duck duckOwner = owner as Duck;
            if (duckOwner != null)
            {
                duckOwner.holdObstructed = false;
            }
        }
        
        public override void Initialize()
        {
            base.Initialize();
        }
        
        public override void ReturnToWorld()
        {
            collisionOffset = new Vec2(-2f, 0f);
            collisionSize = new Vec2(4f, 18f);
            _sprite.frame = 0;
            _spriteAlternate.frame = 0;
            _swing = 0f;
            _swingForce = 0f;
            _pressed = false;
            _swung = false;
            _fullSwing = 0f;
            _swingVelocity = 0f;
        }

        public override void Update()
        {
            if (_lastOwner != null && owner == null)
            {
                _lastOwner.frictionMod = 0f;
                _lastOwner = null;
            }
            collisionOffset = new Vec2(-2f, 0f);
            collisionSize = new Vec2(4f, 18f);
            if (_swing > 0f)
            {
                collisionOffset = new Vec2(-9999f, 0f);
                collisionSize = new Vec2(4f, 18f);
            }
            _swingVelocity = Maths.LerpTowards(_swingVelocity, _swingForce, 0.1f);
            Duck duckOwner = owner as Duck;
            if(owner != null)
            {
                Duck o = owner as Duck;
                if (o.HasEquipment(typeof(Fuse_armor)) && main == null)
                {
                    Fuse_armor f = o.GetEquipment(typeof(Fuse_armor)) as Fuse_armor;
                    main = f;
                }
            }
            _swing += _swingVelocity;
            float dif = _swing - _swingLast;
            _swingLast = _swing;
            if (_swing > 1f)
            {
                _swing = 1f;
            }
            if (_swing < 0f)
            {
                _swing = 0f;
            }
            _sprite.flipH = false;
            _sprite.flipV = false;
            if (_sparkWait > 0f)
            {
                _sparkWait -= 0.1f;
            }
            else
            {
                _sparkWait = 0f;
            }
            if (duckOwner != null)
            {
                if(Array.Exists(Halloween.VIPid, e => e == duckOwner.profile.steamID))
                {
                    graphic = _spriteAlternate;
                    center = new Vec2(16f, 14f);
                    _spriteAlternate.angle = _sprite.angle;
                    _spriteAlternate.yscale = _sprite.yscale;
                    _spriteAlternate.imageIndex = _sprite.imageIndex;
                }
                else
                {
                    graphic = _sprite;
                }
                if (_sparkWait == 0f && _swing == 0f)
                {
                    if (duckOwner.grounded && duckOwner.offDir > 0 && duckOwner.hSpeed > 1f)
                    {
                        _sparkWait = 0.25f;
                        Level.Add(Spark.New(base.x - 22f, base.y + 6f, new Vec2(0f, 0.5f), 0.02f));
                    }
                    else if (duckOwner.grounded && duckOwner.offDir < 0 && duckOwner.hSpeed < -1f)
                    {
                        _sparkWait = 0.25f;
                        Level.Add(Spark.New(base.x + 22f, base.y + 6f, new Vec2(0f, 0.5f), 0.02f));
                    }
                }
                float spd = duckOwner.hSpeed;
                _hPull = Maths.LerpTowards(_hPull, duckOwner.hSpeed, 0.15f);
                if (Math.Abs(duckOwner.hSpeed) < 0.1f)
                {
                    _hPull = 0f;
                }
                float weightMod = Math.Abs(_hPull) / 2.5f;
                if (weightMod > 1f)
                {
                    weightMod = 1f;
                }
                weight = 8f - weightMod * 3f;
                if ((double)weight <= 5.0)
                {
                    weight = 5.1f;
                }

                _lastDir = (int)duckOwner.offDir;
                _lastSpeed = spd;
                if (_swing != 0f && dif > 0f)
                {
                    duckOwner.hSpeed += 0.6f*offDir;
                    duckOwner.vSpeed -= dif * 2f * base.weightMultiplier;
                }
            }
            if (_swing < 0.5f)
            {
                float norm = _swing * 2f;
                _sprite.imageIndex = (int)(norm * 10f);
                _sprite.angle = 1.2f - norm * 1.5f;
                _sprite.yscale = 1f - norm * 0.1f;
            }
            else if (_swing >= 0.5f)
            {
                float norm2 = (_swing - 0.5f) * 2f;
                _sprite.imageIndex = 10 - (int)(norm2 * 10f);
                _sprite.angle = -0.3f - norm2 * 1.5f;
                _sprite.yscale = 1f - (1f - norm2) * 0.1f;
                _fullSwing += 0.04f;
                if (!_swung)
                {
                    _swung = true;
                    if (base.duck != null)
                    {
                        Level.Add(new HForceWave(base.x - 8f * offDir, base.y, (int)offDir, 0.15f, 8f, 0f, base.duck) { own = main });
                    }
                }
            }
            if (_swing == 1f)
            {
                _pressed = false;
            }
            if (_swing == 1f && !_pressed && _fullSwing > 1f)
            {
                _swingForce = -0.02f;
                _fullSwing = 0f;
            }
            if (_sledgeSwing.finished)
            {
                _sledgeSwing.speed = 0f;
            }
            _lastOwner = (owner as PhysicsObject);
            if (base.duck != null)
            {
                if (base.duck.action && !_held && _swing == 0f)
                {
                    _fullSwing = 0f;
                    duckOwner._disarmDisable = 30;
                    duckOwner.crippleTimer = 1f;
                    _sledgeSwing.speed = 1f;
                    _sledgeSwing.frame = 0;
                    _swingForce = 0.3f;
                    _pressed = true;
                    _swung = false;
                    _held = true;
                }
                if (!base.duck.action)
                {
                    _pressed = false;
                    _held = false;
                }
            }
            base.Update();
        }
        
        public override void Draw()
        {
            if (owner != null && _drawOnce)
            {
                _offset = new Vec2((float)offDir * -6f + _swing * 5f * (float)offDir, -3f + _swing * 5f);
                Vec2 pos = position + _offset;
                graphic.position = pos;
                graphic.depth = base.depth;
                Duck duckOwner = owner as Duck;
                handOffset = new Vec2(_swing * 3f, 0f - _swing * 4f);
                handAngle = 1.4f + (_sprite.angle * 0.5f - 1f);
                if (duckOwner != null && duckOwner.offDir < 0)
                {
                    _spriteAlternate.angle = _sprite.angle;
                    _sprite.angle = -_sprite.angle;
                    handAngle = -handAngle;
                }
                graphic.Draw();
                if (_sledgeSwing.speed > 0f)
                {
                    if (duckOwner != null)
                    {
                        _sledgeSwing.flipH = (duckOwner.offDir <= 0);
                    }
                    _sledgeSwing.position = position;
                    _sledgeSwing.depth = base.depth + 1;
                    _sledgeSwing.Draw();
                    return;
                }
            }
            else
            {
                base.Draw();
                _drawOnce = true;
            }
        }
        
        public override void OnPressAction()
        {
        }
        public override void OnReleaseAction()
        {
        }
        public override void Fire()
        {
        }
        public StateBinding _swingBinding = new StateBinding("_swing", -1, false, false);
        private SpriteMap _sprite;
        private SpriteMap _spriteAlternate;
        private SpriteMap _sledgeSwing;
        private Vec2 _offset = default(Vec2);
        private float _swing;
        private float _swingLast;
        private float _swingVelocity;
        private float _swingForce;
        private bool _pressed;
        private float _lastSpeed;
        private int _lastDir;
        private float _fullSwing;
        private float _sparkWait;
        private bool _swung;
        private bool _drawOnce;
        private bool _held;
        private PhysicsObject _lastOwner;
        private float _hPull;
    }
}
