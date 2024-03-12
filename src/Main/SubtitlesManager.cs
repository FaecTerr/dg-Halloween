using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    public class SubtitlesManager : Thing
    {
        public string text;
        public int time;
        private BitmapFont _font;
        public float move;
        public int pos;

        public SubtitlesManager(string str = "", int Time = 90) : base()
        {
            _font = new BitmapFont("biosFont", 8, -1);
            text = str;
            PlayerStats.Load();
            layer = Layer.Foreground;
            depth = 0.8f;
            time = (int)(Time*1.5f);
            float count = 0;
            foreach (SubtitlesManager s in Level.current.things[typeof(SubtitlesManager)])
            {
                if (s != this)
                {
                    count++;
                }
            }
            move = count;
            pos = (int)count;
        }
        public override void Update()
        {
            bool RightPosition = false;
            foreach (SubtitlesManager s in Level.current.things[typeof(SubtitlesManager)])
            {
                if (s.pos == pos - 1 && pos != 0)
                {
                    RightPosition = true;
                }
                if(s.pos == pos)
                {
                    if(pos.GetHashCode() > s.GetHashCode())
                    {
                        pos++;
                    }
                }
            }
            if (!RightPosition)
            {
                pos--;
            }
            Maths.LerpTowards(move, pos, 0.1f);
            time--;
            if (time <= 0)
            {
                Level.Remove(this);
            }
            base.Update();
        }
        public override void Draw()
        {
            if (PlayerStats.subtitles)
            {
                Vec2 pos = new Vec2(Level.current.camera.position.x + Level.current.camera.width / 2, Level.current.camera.position.y + Level.current.camera.height * (49 - move*2) / 64);
                _font.scale = new Vec2(Level.current.camera.size.x / 400 * 0.5625f, Level.current.camera.size.y / 400);
                Color col = Color.LightBlue;
                float xpos = pos.x - _font.GetWidth(text, false, null) / 2f;
                _font.Draw(text, xpos - 1f, pos.y - 1f, Color.Black, 0.8f, null, false);
                _font.Draw(text, xpos + 1f, pos.y - 1f, Color.Black, 0.8f, null, false);
                _font.Draw(text, xpos - 1f, pos.y + 1f, Color.Black, 0.8f, null, false);
                _font.Draw(text, xpos + 1f, pos.y + 1f, Color.Black, 0.8f, null, false);
                _font.Draw(text, xpos, pos.y, col, 0.9f, null, false);
            }
            base.Draw();
        }
    }
}
