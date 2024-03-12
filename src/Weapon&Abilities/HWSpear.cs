using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    [EditorGroup("Halloween|Murderer")]
    public class HWSpear : Gun
    {
        public Fuse_armor main;
        private SpriteMap _sprite;
        private int jumpFrames;
        public HWSpear(float xval, float yval) : base(xval, yval)
        {
            ammo = 4;
            _ammoType = new ATLaser();
            _ammoType.range = 170f;
            _ammoType.accuracy = 0.8f;
            _type = "gun";
            _sprite = new SpriteMap(GetPath("Sprites/Spear.png"), 32, 32, false);
            graphic = _sprite;
            center = new Vec2(15f, 20f);
            collisionOffset = new Vec2(-14f, -4f);
            collisionSize = new Vec2(28f, 8f);
            _kickForce = 3f;
            //_holdOffset = new Vec2(-4f, 4f);
            weight = 8f;
            physicsMaterial = PhysicsMaterial.Metal;
            throwSpeedMultiplier = 0.5f;
            _impactThreshold = 0.3f;
            _editorName = "Claws";
        }

        public override void Update()
        {
            if(jumpFrames > 0)
            {
                jumpFrames--;
            }
            base.Update();
            if (owner == null)
            {
                alpha = 1f;
            }
            if (owner == null && prevOwner != null)
            {
                Duck d = prevOwner as Duck;
                d.alpha = 1f;
            }
            if (owner != null)
            {  
                Duck d = owner as Duck;
                if (d.HasEquipment(typeof(Fuse_armor)) && main == null)
                {
                    Fuse_armor f = d.GetEquipment(typeof(Fuse_armor)) as Fuse_armor;
                    main = f;
                }
                d._spriteArms.alpha = 0f;
                d.hSpeed *= 0.9875f + d.alpha * 0.045f;
                if(d.alpha > 0.1f)
                {
                    d.alpha -= 0.01f;
                }
                else
                {
                    d.alpha = 0.05f;
                }
                if (alpha > 0.1f)
                {
                    alpha -= 0.01f;
                }
                else
                {
                    alpha = 0.05f;
                }
            }
        }
        public virtual void Jump()
        {
            if (owner != null && jumpFrames <= 0)
            {
                Duck d = owner as Duck;
                d.hSpeed = 3f * offDir;
                d.vSpeed += -3f;
                jumpFrames = 120;

                bool invis = d.alpha < 0.1f ? true : false;

                Level.Add(new HForceWave(base.x + 8f * offDir, base.y, (int)offDir, 0.2f, 15f, 0f, d) { own = main, isInvis = invis, isClaws = true });
                d.alpha = 1f;
                alpha = 1f;
            }
        }
        public override void OnPressAction()
        {
            Jump();
        }
        public override void Fire()
        {
        }
    }
}
