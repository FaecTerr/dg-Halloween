using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    public class PointsGetManager : Thing
    {
        public SpriteMap _icon = new SpriteMap(Mod.GetPath<Halloween>("Sprites/GetPoints"), 16, 16);
        public string text = "";
        public string secondText = "";
        private BitmapFont _fon = new BitmapFont("biosFont", 8, -1);
        public int p;
        public int time = 240;

        public PointsGetManager(int points = 0, string category = "Deploy", string altText = " ")
        {
            p = points;
            layer = Layer.Foreground;
            if (category == "Deploy")
            {
                _icon.frame = 0;
                text = LanguageManager.GetPhrase("PM1");
            }
            if (category == "Kill")
            {
                _icon.frame = 1;
                text = LanguageManager.GetPhrase("PM2");
            }
            if (category == "Destroy")
            {
                _icon.frame = 2;
                text = LanguageManager.GetPhrase("PM3");
            }
            if (category == "Time")
            {
                _icon.frame = 3;
                text = LanguageManager.GetPhrase("PM4");
            }
            if (category == "Invis")
            {
                _icon.frame = 4;
                text = LanguageManager.GetPhrase("PM5");
            }
            if (category == "Detect")
            {
                _icon.frame = 5;
                text = LanguageManager.GetPhrase("PM6");
            }
            if (category == "Scan")
            {
                _icon.frame = 6;
                text = LanguageManager.GetPhrase("PM7");
            }
            if (category == "Track")
            {
                _icon.frame = 7;
                text = LanguageManager.GetPhrase("PM8");
            }
            if (category == "Lantern")
            {
                _icon.frame = 8;
                text = LanguageManager.GetPhrase("PM9");
            }
            if (category == "Trap")
            {
                _icon.frame = 9;
                text = LanguageManager.GetPhrase("PM10");
            }
            secondText = altText;
        }

        public override void Draw()
        {
            time--;
            if(time <= 0)
            {
                Level.Remove(this);
            }
            int camSmollness = 480;
            Vec2 camSize = new Vec2(Level.current.camera.width, Level.current.camera.width);
            Vec2 realCamSize = new Vec2(Level.current.camera.width, Level.current.camera.height);
            Vec2 camPos = new Vec2(Level.current.camera.position.x, Level.current.camera.position.y);

            _icon.scale = camSize / (camSmollness);
            _fon.scale = camSize / (camSmollness*2.5f);

            _icon.alpha = time / 60;
            _fon.alpha = time / 60;

            Graphics.Draw(_icon, camPos.x + camSize.x * 3 / camSmollness, camPos.y + (camSize.y / camSmollness) * (_icon.frame*20 + 32), 0.8f);
            //_fon.Draw(text, camPos.x + _icon.scale.x * 20 + camSize.x * 3 / camSmollness, camPos.y + (camSize.y / camSmollness) * (_icon.frame * 20 + 6), Color.White);
            _fon.DrawOutline(Convert.ToString(p), new Vec2(camPos.x + _icon.scale.x * 20 + camSize.x * 3 / camSmollness, camPos.y + _fon.scale.y * 26 + (camSize.y / camSmollness) * (_icon.frame * 20 + 32)), Color.White, Color.Black, 0.8f);
            _fon.DrawOutline(text, new Vec2(camPos.x + _icon.scale.x * 20 + camSize.x * 3 / camSmollness, camPos.y + (camSize.y / camSmollness) * (_icon.frame * 20 + 32)), Color.White, Color.Black, 0.8f);
            _fon.scale = camSize / (camSmollness * 3f);
            _fon.DrawOutline(secondText, new Vec2(camPos.x + _icon.scale.x * 20 + camSize.x * 3 / camSmollness, camPos.y + (camSize.y / camSmollness) * (_icon.frame * 20 + 38)), Color.White, Color.Black, 0.8f);
            //_fon.Draw(Convert.ToString(p), camPos.x + _icon.scale.x * 20 + camSize.x * 3 / camSmollness, camPos.y + _fon.scale.y * 10 + (camSize.y / camSmollness) * (_icon.frame * 20 + 6), Color.White);
           

            base.Draw();
        }
    }
}
