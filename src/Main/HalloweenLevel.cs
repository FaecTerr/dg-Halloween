using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    public class HalloweenLevel : Level
    {
        public int xpos;
        public int ypos;

        public Color c = Color.White;

        public SinWave _pulse = 0.01f;

        public int lang;
        public Profile p;

        public float moving;
        public SpriteMap _VHS;
        public SpriteMap _background;
        public SpriteMap _ghostTut;
        public SpriteMap _murdererTut;
        public SpriteMap _settings;
        public SpriteMap _achievements;
        public SpriteMap _exit;

        public int screenshake = 50;
        public int DictorVolume = 100;
        public bool ShowButtons = true;
        public bool ShowTimer = true;
        public bool ShowHints = true;
        public bool subtitles = true;

        public float timeFromStart;
        public bool completed;

        public int strin = 0;

        // 320 x 180

        public override void Initialize()
        {
            Music.Load(Mod.GetPath<Halloween>("Music/OptionsMusic.ogg"), true);
            Music.PlayLoaded();
            Music.volumeMult = 6;

            DictorVolume = PlayerStats.DictorVolume;
            screenshake = PlayerStats.screenshake;
            ShowButtons = PlayerStats.ShowButtons;
            ShowHints = PlayerStats.ShowHints;
            ShowTimer = PlayerStats.ShowTimer;
            subtitles = PlayerStats.subtitles;
            completed = Halloween.upd.challengeCompleted;
            lang = PlayerStats.language;
            base.Initialize();       
        }

        public HalloweenLevel() : base()
        {
            Layer.Game.fade = 0f;
            Layer.Foreground.fade = 0f;
            _background = new SpriteMap(Mod.GetPath<Halloween>("Sprites/Menu/BackStage.png"), 320, 240);
            _background.depth = 0.5f;
            _VHS = new SpriteMap(Mod.GetPath<Halloween>("Sprites/VHS.png"), 320, 180);
            _VHS.depth = 0.5f;
            _ghostTut = new SpriteMap(Mod.GetPath<Halloween>("Sprites/Menu/GhostTutorial.png"), 110, 30);
            _murdererTut = new SpriteMap(Mod.GetPath<Halloween>("Sprites/Menu/HunterTutorial.png"), 110, 30);
            _settings = new SpriteMap(Mod.GetPath<Halloween>("Sprites/Menu/SettingsButton.png"), 20, 20);
            _achievements = new SpriteMap(Mod.GetPath<Halloween>("Sprites/Menu/DealboardButton.png"), 20, 20);
            _exit = new SpriteMap(Mod.GetPath<Halloween>("Sprites/Menu/ExitButton.png"), 20, 20);
        }
        public override void Update()
        {
            base.Update();
            timeFromStart += 0.01666666f;
            if (Halloween.upd.challengeProgression >= Halloween.upd.challengeTarget)
            {
                completed = true;
            }
            else
            {
                completed = false;
            }
            if (Halloween.upd.challengeProgression >= Halloween.upd.challengeTarget && timeFromStart > 5f)
            {
                Halloween.upd.challengeCompleted = true;
            }

            float AntiWave = _pulse - 1f; 
            if(AntiWave > 1)
            {
                AntiWave = 1f - (AntiWave - 1f);
            }
            if (AntiWave < -1)
            {
                AntiWave = -1f - (AntiWave + 1f);
            }
            moving += 0.6f - AntiWave * 0.3f;

            //_VHS.alpha -= 0.01f;

            if (moving > 180)
            {
                moving = 0;
            }
            PlayerStats.Data.DictorVolume = DictorVolume;
            PlayerStats.Data.screenshake = screenshake;
            PlayerStats.Data.ShowButtons = ShowButtons;
            PlayerStats.Data.ShowHints = ShowHints;
            PlayerStats.Data.ShowTimer = ShowTimer;
            PlayerStats.Data.subtitles = subtitles;
            PlayerStats.Data.Language = lang;
            Halloween.upd.Init();
            /*if (Keyboard.Pressed(Keys.F1))
            {
                
                Level.current = new ChallengeLevel(Mod.GetPath<Halloween>("Levels/Tutorial/[Halloween] [Tutorial Ghost] Southwest mansion.lev"));
                
            }
            if (Keyboard.Pressed(Keys.F3))
            {

                Level.current = new ChallengeLevel(Mod.GetPath<Halloween>("Levels/Tutorial/[Halloween] [Tutorial Murderer] Southwest mansion.lev"));

            }*/

            if ((strin == 7 && (p.inputProfile.Pressed("SELECT"))) || p.inputProfile.Pressed("QUACK"))
            {
                Halloween.upd.Init();
                PlayerStats.Save();
                current = new TitleScreen();
            }
            if (p.inputProfile.Pressed("UP") || p.inputProfile.Pressed("JUMP"))
            {
                if(strin > 0)
                {
                    strin--;
                }
            }
            if (p.inputProfile.Pressed("DOWN"))
            {
                if (strin < 7)
                {
                    strin++;
                }
            }
            if (p.inputProfile.Pressed("LEFT"))
            {
                if(strin == 0)
                {
                    if(screenshake > 0)
                    {
                        screenshake -= 5;
                    }
                }
                if (strin == 1)
                {
                    if (DictorVolume > 0)
                    {                   
                        DictorVolume -= 10;
                        SFX.Play(Mod.GetPath<Halloween>("SFX/30Sec.wav"), (float)DictorVolume / 100, 0.0f, 0.0f, false);
                    }
                }
                if(strin == 2)
                {
                    ShowHints = false;
                }
                if (strin == 3)
                {
                    ShowButtons = false;
                }
                if (strin == 4)
                {
                    ShowTimer = false;
                }
                if (strin == 5)
                {
                    subtitles = false;
                }
                if(strin == 6)
                {
                    lang--;
                    if(lang < 0)
                    {
                        lang = 2;
                    }
                }
            }
            if (p.inputProfile.Pressed("RIGHT"))
            {
                if (strin == 0)
                {
                    if (screenshake < 100)
                    {
                        screenshake += 5;
                    }
                }
                if (strin == 1)
                {
                    if (DictorVolume < 100)
                    {                    
                        DictorVolume += 10;
                        SFX.Play(Mod.GetPath<Halloween>("SFX/30Sec.wav"), (float)DictorVolume / 100, 0.0f, 0.0f, false);
                    }
                }
                if (strin == 2)
                {
                    ShowHints = true;
                }
                if (strin == 3)
                {
                    ShowButtons = true;
                }
                if (strin == 4)
                {
                    ShowTimer = true;
                }
                if (strin == 5)
                {
                    subtitles = true;
                }
                if (strin == 6)
                {
                    lang++;
                    if (lang > 2)
                    {
                        lang = 0;
                    }
                }
            }
        }

        public override void PostDrawLayer(Layer layer)
        {
            if (layer == Layer.Foreground)
            {
                int camSmollness = 200;
                Vec2 camSize = new Vec2(Level.current.camera.width, Level.current.camera.width);
                Vec2 realCamSize = new Vec2(Level.current.camera.width, Level.current.camera.height);
                Vec2 camPos = new Vec2(Level.current.camera.position.x, Level.current.camera.position.y);

                SpriteMap _rank = new SpriteMap(Mod.GetPath<Halloween>("Sprites/RankIcons.png"), 16, 16);
                _rank.frame = (int)Halloween.upd.ELO / 100;
                _rank.scale = new Vec2(camSize / (camSmollness));
                string txt = "ELO: " + Convert.ToString(Halloween.upd.ELO);

                BitmapFont f = new BitmapFont("biosFont", 8, -1);
                f.scale = camSize / (camSmollness * 2.5f);

                //f.DrawOutline(txt, new Vec2(camPos.x + camSize.x - _rank.scale.x * 32f, camPos.y + camSize.y *(3/4) + (camSize.y / camSmollness) + _rank.scale.y * 24f), Color.White, Color.Black, 0.8f);
                //Graphics.Draw(_rank, camPos.x - _rank.scale.x * 26 + camSize.x, camPos.y + camSize.y * (3 / 4) + _rank.scale.y * 4 + (camSize.y / camSmollness), 0.8f);
                /*Graphics.Draw(_ghostTut, 45, 40);
                Graphics.Draw(_murdererTut, 165, 40);
                Graphics.Draw(_settings, 110, 90);
                Graphics.Draw(_achievements, 150, 90);
                Graphics.Draw(_exit, 190, 90);*/

                for (int i = 0; i < 3; i++)
                {
                    
                    Graphics.Draw(_VHS, 0f, -180+moving + 180*i);
                }

            }
            if(layer == Layer.Game)
            {
                if(completed)
                {
                    c = Color.Green;
                }
                else
                {
                    c = Color.White;
                }
                float time1 = (timeFromStart - 2f) * 80;
                float time2 = (timeFromStart - 2.2f) * 80;
                float time3 = (timeFromStart - 2.4f) * 80;

                string name = Halloween.upd.challengeName;
                string description = Halloween.upd.challengeDescription;

                if (completed == false)
                {
                    time1 = 0;
                    time2 = 0;
                    time3 = 0;
                    name = Halloween.upd.challengeName;
                    description = Halloween.upd.challengeDescription;
                }
                else
                {
                    name = "CHALLENGE COMPLETED";
                    description = "You earned 75$";
                }

                if (time1 < 0)
                {
                    time1 = 0;
                }
                if (time2 < 0)
                {
                    time2 = 0;
                }
                if (time3 < 0)
                {
                    time3 = 0;
                }

                if(Halloween.upd.challengeProgression > Halloween.upd.challengeTarget)
                {
                    Halloween.upd.challengeProgression = Halloween.upd.challengeTarget;
                }
                //DevConsole.Log(Convert.ToString((float)Halloween.upd.challengeProgression) + " / " + Convert.ToString((float)Halloween.upd.challengeTarget), Color.White);
                float wide = ((float)Halloween.upd.challengeProgression / (float)Halloween.upd.challengeTarget) * 180f;
                Graphics.DrawString(name, new Vec2(70 - time3, 25), c, 0.92f, null, 1.2f);
                Graphics.DrawString(description, new Vec2(70 - time2, 35), c, 0.92f, null, 0.8f);
                Graphics.DrawLine(new Vec2(70 - time1, 55), new Vec2(70 + 180 - time1, 55), Color.LightGray, 3f, 0.97f);
                Graphics.DrawLine(new Vec2(70 - time1, 55), new Vec2(70 + wide - time1, 55), Color.Green, 3f, 0.98f);

                int lenMax = 18;

                Graphics.DrawString(LanguageManager.GetPhrase("SSS"), new Vec2(90, 70), Color.White, 0.92f, null, Math.Min(10f / LanguageManager.GetPhrase("SSS").Length, 1));
                Graphics.DrawString(LanguageManager.GetPhrase("SDV"), new Vec2(90, 84), Color.White, 0.92f, null, Math.Min(10f / LanguageManager.GetPhrase("SDV").Length, 1));
                Graphics.DrawString(LanguageManager.GetPhrase("SSH"), new Vec2(90, 98), Color.White, 0.92f, null, Math.Min(10f / LanguageManager.GetPhrase("SSH").Length, 1));
                Graphics.DrawString(LanguageManager.GetPhrase("SKB"), new Vec2(90, 112), Color.White, 0.92f, null, Math.Min(10f / LanguageManager.GetPhrase("SKB").Length, 1));
                Graphics.DrawString(LanguageManager.GetPhrase("SST"), new Vec2(90, 126), Color.White, 0.92f, null, Math.Min(10f / LanguageManager.GetPhrase("SST").Length,1));
                Graphics.DrawString(LanguageManager.GetPhrase("SSUB"), new Vec2(90, 140), Color.White, 0.92f, null, Math.Min(10f / LanguageManager.GetPhrase("SSUB").Length,1));
                Graphics.DrawString(LanguageManager.GetPhrase("STL"), new Vec2(90, 154), Color.White, 0.92f, null, Math.Min(10f / LanguageManager.GetPhrase("STL").Length, 1));
                Graphics.DrawString(LanguageManager.GetPhrase("EXIT"), new Vec2(90, 168), Color.White, 0.92f, null, Math.Min(10f / LanguageManager.GetPhrase("EXIT").Length, 1));

                Graphics.DrawString(Convert.ToString(screenshake), new Vec2(200, 70), Color.White, 0.92f, null, 1f);
                Graphics.DrawString(Convert.ToString(DictorVolume), new Vec2(200, 84), Color.White, 0.92f, null, 1f);
                Graphics.DrawString(Convert.ToString(ShowHints), new Vec2(200, 98), Color.White, 0.92f, null, 1f);
                Graphics.DrawString(Convert.ToString(ShowButtons), new Vec2(200, 112), Color.White, 0.92f, null, 1f);
                Graphics.DrawString(Convert.ToString(ShowTimer), new Vec2(200, 126), Color.White, 0.92f, null, 1f);
                Graphics.DrawString(Convert.ToString(subtitles), new Vec2(200, 140), Color.White, 0.92f, null, 1f);


                string space = " ";
                for (int i = 0; i < lenMax; i++)
                {
                    space += " ";
                }
                Graphics.DrawString(">" + space + "<", new Vec2(80, 70 + 14 * strin), Color.White, 0.9f, null, 1f);

                string text = "ENG";
                if (lang == 1)
                {
                    text = "RUS";
                }
                if (lang == 2)
                {
                    text = "ESP";
                }
                Graphics.DrawString(text, new Vec2(200, 154), Color.White, 0.92f, null, 1f);
            }
            if(layer == Layer.Background)
            {
                Graphics.Draw(_background, 0, -30+_pulse*30);
            }
            base.PostDrawLayer(layer);
            
        }
    }
}
