using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    public class Toxic : Thing
    {
        public StateBinding _positionStateBinding = new CompressedVec2Binding("position");
        protected SpriteMap _sprite;
        private float radius;
        private int outFrame = 0;
        private SinWave _pulse1 = 0.2f;
        private SinWave _pulse2 = 0.2f;
        public bool IsLocalDuckAffected
        {
            get;
            set;
        }
        public float Timer
        {
            get;
            set;
        }

        public Toxic(float xval, float yval, float stayTime = 0.2f, float rad = 160f, float alp = 0.7f) : base(xval, yval)
        {
            Timer = stayTime;
            depth = 0.95f;
            layer = Layer.Foreground;
            depth = 0.95f;
            radius = rad;
            SetIsLocalDuckAffected();
            _sprite = new SpriteMap(GetPath("Sprites/Toxic.png"), 32, 32);
            _sprite.CenterOrigin();
            _sprite.alpha = alp;
            _sprite.AddAnimation("idle", 0.03f, true, new int[] { 0, 1, 2});
            _sprite.SetAnimation("idle");
        }

        public virtual void SetIsLocalDuckAffected()
        {
            List<Duck> ducks = new List<Duck>();
            foreach (Duck duck in Level.CheckCircleAll<Duck>(position, radius))
            {
                if (!ducks.Contains(duck))
                {
                    ducks.Add(duck);
                }
            }
            foreach (Ragdoll ragdoll in Level.CheckCircleAll<Ragdoll>(position, radius))
            {
                if (!ducks.Contains(ragdoll._duck))
                {
                    ducks.Add(ragdoll._duck);
                }
            }
            foreach (Duck duck in ducks)
            {
                if (duck.profile.localPlayer)
                {
                    if (Level.CheckLine<Block>(position, duck.position, duck) == null)
                    {
                        IsLocalDuckAffected = true;
                        return;
                    }
                }
            }
            IsLocalDuckAffected = false;
        }

        public override void Update()
        {
            base.Update();
            _sprite.xscale = Level.current.camera.width / (31 - _pulse1 * 0.2f);
            _sprite.yscale = Level.current.camera.height / (31 - _pulse1 * 0.2f);
            if (Timer > 0f)
                _sprite.alpha = 0.9f + _pulse2 * 0.2f;
            if (Timer > 0)
            {
                Timer -= 0.01666666f;
            }
            else
            {
                _sprite.alpha -= 0.035f;
                outFrame++;
            }
            if (outFrame > 20)
            {
                Level.Remove(this);
            }
        }

        public override void Draw()
        {
            if (IsLocalDuckAffected)
            {
                //updater.ShaderController();
                Graphics.Draw(_sprite, Level.current.camera.center.x, Level.current.camera.center.y, 0.95f);
            }
            base.Draw();
        }
    }
}
