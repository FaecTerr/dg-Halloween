using System;

namespace DuckGame.Halloween
{
    public class DuckIsModableLogo : Level
    {
        public override void Initialize()
        {
            _font = new BitmapFont("biosFont", 8, -1);
            _logo = new Sprite(Mod.GetPath<Halloween>("Sprites/logomod.png"), 0f, 0f);
            Graphics.fade = 0f;
        }
        
        public override void Update()
        {
            if (!_fading)
            {
                if (Graphics.fade < 1f)
                {
                    Graphics.fade += 0.013f;
                }
                else
                {
                    Graphics.fade = 1f;
                }
            }
            else if (Graphics.fade > 0f)
            {
                Graphics.fade -= 0.013f;
            }
            else
            {
                Graphics.fade = 0f;
                if (MonoMain.startInEditor)
                {
                    Level.current = Main.editor;
                }
                else
                {
                    Level.current = new TitleScreen();
                }
            }
            _wait -= 0.006f;
            if (_wait < 0f || Input.Pressed("START", "Any") || Input.Pressed("SELECT", "Any"))
            {
                _fading = true;
            }
        }
        
        public override void PostDrawLayer(Layer layer)
        {
            if (layer == Layer.Game)
            {
                Graphics.Draw(_logo, 0, 0);
            }
        }
        
        private BitmapFont _font;
        private Sprite _logo;
        private float _wait = 1f;
        private bool _fading;
    }
}
