using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    [EditorGroup("Halloween|Tutorial|Traps")]
    public class GasCanister : Device
    {
        public SpriteMap _sprite;
        public SpriteMap _countdown;
        public int setting = 32;

        public GasCanister(float xval, float yval) : base(xval, yval)
        {
            _sprite = new SpriteMap(GetPath("Sprites/GasCanister.png"), 16, 16, false);
            _countdown = new SpriteMap(GetPath("Sprites/Loading.png"), 14, 14, false);
            _countdown.center = new Vec2(7, 7);
            graphic = _sprite;
            center = new Vec2(8f, 8f);
            collisionOffset = new Vec2(-4f, -6f);
            collisionSize = new Vec2(8f, 12f);
            weight = 0.9f;
            thickness = 0.1f;
            placeable = false;
            zeroSpeed = false;
            frame = 1;

            damage = 9;
            lethal = true;
            name = LanguageManager.GetPhrase("GS");
            description = LanguageManager.GetPhrase("H GS");
        }
        public override void Update()
        {
            base.Update();
            text = LanguageManager.GetPhrase("Throw");
            if (owner == null)
            {
                setting--;
                if (setting <= 0)
                {
                    foreach (Duck d in Level.current.things[typeof(Duck)])
                    {
                        DinamicSFX.Play(d.position, position, 2f, 1f, "SFX/Smoke.wav", 96f);
                    }
                    if (main != null && mainer != null)
                    {
                        main.points += 30;
                        if (mainer.profile.localPlayer)
                        {
                            Level.Add(new PointsGetManager(30, "Trap"));
                            
                        }
                    }
                    _enablePhysics = false;
                    grounded = true;
                    _hSpeed = 0f;
                    _vSpeed = 0f;
                    setted = true;
                    canPickUp = false;
                    Level.Add(new GasSmoke(position.x, position.y) { main = main, mainer = mainer });
                    Level.Remove(this);
                }
            }
        }
        public override void Draw()
        {
            if (owner == null)
            {
                _countdown.alpha = 0.6f;
                _countdown.scale = new Vec2(2f,2f);
                _countdown.frame = setting / 4;
                Graphics.Draw(_countdown, position.x, position.y);
            }
            base.Draw();
        }
    }
}
