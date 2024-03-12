using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Runtime.Serialization.Formatters.Binary;

namespace DuckGame.Halloween
{
    public class updater : IAutoUpdate
    {
        private int pos;
        public static string SubLang;
        public SinWave _wave = 0.3f;
        public static RenderTarget2D _renderTarget;
        public bool arrived;
        public int counter;
        public bool openTitle;
        public bool init;

        public Profile p;

        public bool firstLaunch = true;

        public int challengeID = -1;
        public int challengeProgression = 0;
        public int challengeTarget;
        public int challengeReward = 75;
        public bool challengeCompleted;

        public string challengeName;
        public string challengeDescription;

        public int EnemyElo = 500;

        public int ELO = PlayerStats.Data.ELO;
        public float Accuracy = 40;

        private BitmapFont _font;
        private static UIMenuAction openMenu;

        public Level prevLevel;

        public SpriteMap _spectatorBox;
        public SpriteMap _hpBar;
        public SpriteMap _item;

        public int prevProg;

        public updater() : base()
        {
            challengeID = PlayerStats.Data.ChallengeID;
            challengeProgression = challengeID = PlayerStats.Data.ChallengeProgress;
            if (firstLaunch && !MonoMain.noIntro)
            {
                firstLaunch = false;
                Level.current = new DuckIsModableLogo();
            }
            _font = new BitmapFont("biosFont", 8, -1);
            AutoUpdatables.Add(this);
            _spectatorBox = new SpriteMap(Mod.GetPath<Halloween>("SpectatorBox.png"), 107, 40);
            _hpBar = new SpriteMap(Mod.GetPath<Halloween>("HealthBar.png"), 64, 20);
            _item = new SpriteMap(Mod.GetPath<Halloween>("Sprites/Item.png"), 32, 32);
        }

        public void Init()
        {
            int len = 36;
            if(challengeID == 0)
            {
                challengeName = LanguageManager.GetPhrase("CHAL0N");
                challengeDescription = LanguageManager.GetPhrase("CHAL0D");
                challengeDescription = LanguageManager.SplitInLines(challengeDescription, len);
                challengeTarget = 10;
            }
            if (challengeID == 1)
            {
                challengeName = LanguageManager.GetPhrase("CHAL1N");
                challengeDescription = LanguageManager.GetPhrase("CHAL1D");
                challengeDescription = LanguageManager.SplitInLines(challengeDescription, len);
                challengeTarget = 10;
            }
            if (challengeID == 2)
            {
                challengeName = LanguageManager.GetPhrase("CHAL2N");
                challengeDescription = LanguageManager.GetPhrase("CHAL2D");
                challengeDescription = LanguageManager.SplitInLines(challengeDescription, len);
                challengeTarget = 10;
            }
            if (challengeID == 3)
            {
                challengeName = LanguageManager.GetPhrase("CHAL3N");
                challengeDescription = LanguageManager.GetPhrase("CHAL3D");
                challengeDescription = LanguageManager.SplitInLines(challengeDescription, len);
                challengeTarget = 50;
            }
            if (challengeID == 4)
            {
                challengeName = LanguageManager.GetPhrase("CHAL4N");
                challengeDescription = LanguageManager.GetPhrase("CHAL4D");
                challengeDescription = LanguageManager.SplitInLines(challengeDescription, len);
                challengeTarget = 25;
            }
            if (challengeID == 5)
            {
                challengeName = LanguageManager.GetPhrase("CHAL5N");
                challengeDescription = LanguageManager.GetPhrase("CHAL5D");
                challengeDescription = LanguageManager.SplitInLines(challengeDescription, len);
                challengeTarget = 5;
            }
            if (challengeID == 6)
            {
                challengeName = LanguageManager.GetPhrase("CHAL6N");
                challengeDescription = LanguageManager.GetPhrase("CHAL6D");
                challengeDescription = LanguageManager.SplitInLines(challengeDescription, len);
                challengeTarget = 5;
            }
            if (challengeID == 7)
            {
                challengeName = LanguageManager.GetPhrase("CHAL7N");
                challengeDescription = LanguageManager.GetPhrase("CHAL7D");
                challengeDescription = LanguageManager.SplitInLines(challengeDescription, len);
                challengeTarget = 10;
            }
            if (challengeID == 8)
            {
                challengeName = LanguageManager.GetPhrase("CHAL8N");
                challengeDescription = LanguageManager.GetPhrase("CHAL8D");
                challengeDescription = LanguageManager.SplitInLines(challengeDescription, len);
                challengeTarget = 50;
            }
            if (challengeID == 9)
            {
                challengeName = LanguageManager.GetPhrase("CHAL9N");
                challengeDescription = LanguageManager.GetPhrase("CHAL9D");
                challengeDescription = LanguageManager.SplitInLines(challengeDescription, len);
                challengeTarget = 5;
            }
            if (challengeID == 10)
            {
                challengeName = LanguageManager.GetPhrase("CHAL10N");
                challengeDescription = LanguageManager.GetPhrase("CHAL10D");
                challengeDescription = LanguageManager.SplitInLines(challengeDescription, len);
                challengeTarget = 20;
            }
            if (challengeID == 11)
            {
                challengeName = LanguageManager.GetPhrase("CHAL11N");
                challengeDescription = LanguageManager.GetPhrase("CHAL11D");
                challengeDescription = LanguageManager.SplitInLines(challengeDescription, len);
                challengeTarget = 3;
            }
            if (challengeID == 12)
            {
                challengeName = LanguageManager.GetPhrase("CHAL12N");
                challengeDescription = LanguageManager.GetPhrase("CHAL12D");
                challengeDescription = LanguageManager.SplitInLines(challengeDescription, len);
                challengeTarget = 5;
            }
            if (challengeID == 13)
            {
                challengeName = LanguageManager.GetPhrase("CHAL13N");
                challengeDescription = LanguageManager.GetPhrase("CHAL13D");
                challengeDescription = LanguageManager.SplitInLines(challengeDescription, len);
                challengeTarget = 1;
            }
            if(challengeID == 14)
            {
                challengeName = LanguageManager.GetPhrase("CHAL14N"); 
                challengeDescription = LanguageManager.GetPhrase("CHAL14D");
                challengeDescription = LanguageManager.SplitInLines(challengeDescription, len);
                challengeTarget = 1;
            }
        }

        public void Update()
        {
            int id = challengeID;

            if (p == null)
            {
                p = Profiles.experienceProfile;
                DevConsole.Log(Convert.ToString(p.steamID), Color.White);
                /*foreach (Profile d in Profiles.all)
                {
                    if (d.linkedProfile != null)
                    {
                        Profiles.
                        if (d.linkedProfile != null)
                        {
                            p = d;
                        }
                    }
                }*/
            }

            if (id == -1)
            {
                Init();
            }

            if (challengeCompleted)
            {
                challengeCompleted = false;
                challengeID = Rando.Int(9);
                PlayerStats.ChallengeID = challengeID;
                challengeProgression = 0;
                PlayerStats.ChallengeProgress = challengeProgression;
                PlayerStats.Save();
                if (p != null)
                {
                    p.littleManBucks += challengeReward;
                }
            }

            if(prevProg != challengeProgression)
            {
                prevProg = challengeProgression;
                PlayerStats.ChallengeProgress = challengeProgression;
                PlayerStats.Save();
            }

            if (init == false)
            {
                Init();
                init = true;
            }
            if (ELO > 2000)
            {
                ELO = 2000;
            }
            if (ELO < 0)
            {
                ELO = 0;
            }
            Level level = Level.current;
            if (level != null && level.initialized)
            {
                if (!(level is TitleScreen))
                {
                    openTitle = false;
                }
                if (level is GameLevel)
                {
                    if (level != prevLevel)
                    {
                        prevLevel = level;
                        if (prevLevel is GameLevel)
                        {
                            /*if (Network.isActive)
                            {
                                Send.Message(new NMSendElo((byte)ELO));
                            }*/
                            foreach (Profile d in GameMode.lastWinners)
                            {
                                DevConsole.Log("Previous round winner: " + Convert.ToString(d.name), Color.White);
                            }
                            
                            /*if (GameMode.lastWinners.Contains(p))
                            {
                                ELO += 100 - ((ELO - EnemyElo) / (int)Accuracy);
                                Accuracy += 2f;
                            }
                            else
                            {
                                ELO += -100 - ((ELO - EnemyElo) / (int)Accuracy);
                                Accuracy += 2f;
                            }*/
                            PlayerStats.ELO = ELO;
                            PlayerStats.Save();
                        }
                    }
                }
                    /*if (level is RockScoreboard)
                    {
                        foreach (Duck d in Level.current.things[typeof(Duck)])
                        {
                            if (d.profile.localPlayer)
                            {
                                Level.Add(new Rank(p.duck.position.x, p.duck.position.y));
                            }
                        }
                    }*/
                
                if (level is TitleScreen)
                {
                    if (Keyboard.Pressed(Keys.F2) && p != null)
                    {
                        //PlayerStats.OpenOptionsMenu();
                        //ShowControls();
                        //Level.current = new HalloweenLevel() { p = p };
                    }
                    if (Music.currentSong == "Title")
                    {
                        Music.Load(Mod.GetPath<Halloween>("Music/HalloweenTitle.ogg"), true);
                        Music.PlayLoaded();
                        Music.volumeMult = 12;
                        //Music.PlayLoaded();
                    }
                    counter++;
                    if (counter % 15 == 0)
                    {
                        Level.Add(new Blood(Rando.Float(Level.current.topLeft.x, Level.current.bottomRight.x), Level.current.topLeft.y - 100f));
                    }
                    if (openTitle == false)
                    {
                        Level.Add(new HLWNPortal(72, 150));
                        openTitle = true;
                        DetectorNode d = new DetectorNode(112, 144);
                        Level.Add(d);
                        DetectorNode b = new DetectorNode(208, 144);
                        Level.Add(b);
                        d.position.x = 112;
                        d.position.y = 154;
                        b.position.x = 208;
                        b.position.y = 154;
                    }
                    if (!arrived && PlayerStats.firstLaunch && level.initialized)
                    {
                        //ShowControls();
                        arrived = true;
                    }
                }
            }
        }

        public void ShowControls()
        {
            string title = "|ORANGE|Halloween Event";

            UIComponent menu = new UIComponent(160f, 90f, 140f, 90f);
            UIMenu uiMenu = new UIMenu(title, Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, -1, -1f, "", (InputProfile)null, false);

            BitmapFont f = new BitmapFont("smallBiosFontUI", 7, 5);
            /*string ability = "@QUACK@";
            string swapAbility = "@STRAFE@";
            string setDevice = "@SHOOT@";*/

            UIComponent[] uiTextArray = new UIComponent[]
            {
                new UIText("|DGYELLOW|Welcome to halloween event mod!", Color.White, UIAlign.Center, 0.0f, (InputProfile) null),
                new UIText("", Color.White, UIAlign.Center, 0.0f, (InputProfile) null),
                new UIText("Do you want to open tutorial?", Color.White, UIAlign.Center, 0.0f, (InputProfile) null),
                new UIText("", Color.White, UIAlign.Center, 0.0f, (InputProfile) null),
            };

            foreach (UIComponent uiText in uiTextArray)
            {
                uiMenu.Add(uiText);
            }
            uiMenu.Add(new UIMenuItem("Launch murderer tutorial", new UIMenuActionChangeLevel(menu, new ChallengeLevel(Mod.GetPath<Halloween>("Levels/Tutorial/[Halloween] [Tutorial Ghost] Southwest mansion.lev"))), UIAlign.Center, new Color(), true), true);
            uiMenu.Add(new UIMenuItem("Launch ghost tutorial", new UIMenuActionChangeLevel(menu, new ChallengeLevel(Mod.GetPath<Halloween>("Levels/Tutorial/[Halloween] [Tutorial Ghost] Southwest mansion.lev"))), UIAlign.Center, new Color(), true), true);
            uiMenu.Add(new UIMenuItem("Let me go", new UIMenuActionCloseMenu(menu), UIAlign.Center, new Color(), true), true);
            menu.Add(uiMenu, false);
            Level.Add(menu);
            menu.Open();
            uiMenu.Open();


        }
    }
}
