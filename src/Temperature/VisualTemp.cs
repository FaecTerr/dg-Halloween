using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    public class Temperature : Thing
    {
        public StateBinding _positionStateBinding = new CompressedVec2Binding("position");
        public SpriteMap _sprite;
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

        public int fr;

        public Temperature(float xval, float yval, float stayTime = 0.5f, float rad = 160f, float alp = 0f) : base(xval, yval)
        {
            Timer = stayTime;
            depth = 0.95f;
            layer = Layer.Foreground;
            depth = 0.95f;
            radius = rad;
            SetIsLocalDuckAffected();
            _sprite = new SpriteMap(GetPath("Sprites/Light.png"), 32, 32);
            _sprite.center = new Vec2(16, 16);
            _sprite.alpha = alp;
            _sprite.frame = fr;
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
            
            _sprite.xscale = Level.current.camera.width / (31 - _pulse1 * 0.2f);
            _sprite.yscale = Level.current.camera.height / (31 - _pulse1 * 0.2f);
            if (Timer > 0)
            {
                Timer -= 0.01666666f;
            }
            else
            {
                _sprite.alpha -= 0.005f;
                outFrame++;
            }
            if (outFrame > 100)
            {
                Level.Remove(this);
            }
            base.Update();
        }

        public override void Draw()
        {
            if (IsLocalDuckAffected)
            {
                _sprite.frame = fr;
                //updater.ShaderController();
                Graphics.Draw(_sprite, Level.current.camera.center.x, Level.current.camera.center.y, 0.95f);
            }
            base.Draw();
        }
    }
}
