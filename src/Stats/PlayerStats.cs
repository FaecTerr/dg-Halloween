using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DuckGame.Halloween
{
    public class PlayerStats
    {
        //Yo Obama what the fuck is going on I don't know what am i doing what is that
        //Man i was creating that fckin thing like 6 months and found a normal way to do this only now
        //God please help me why tf i still doing that
        public static HLWNData _data;

        private static UIMenu _optionsMenu;

        private static bool _removedOptionsMenu = false;
        private static bool _openedOptionsMenu = false;
        public static UIMenu openOnClose = null;

        public static bool _SetLan;
        public static int ELO /*= 500*/;
        public static string SubLanguage /*= "English"*/;
        //Options
        public static bool firstLaunch;
        public static int screenshake /*= 50*/;
        public static int DictorVolume /*= 100*/;
        public static bool ShowButtons /*= true*/;
        public static bool ShowTimer /*= true*/;
        public static bool ShowHints /*= true*/;
        public static bool subtitles /*= true*/;
        public static int ChallengeID;
        public static int ChallengeProgress;

        public static int language;

        static PlayerStats()
        {
            _data = new HLWNData();
            //Load();
            firstLaunch = true;
            screenshake = 50;
            DictorVolume = 100;
            ShowButtons = true;
            ShowTimer = true;
            ShowHints = true;
            subtitles = true;
            ELO = 500;
            SubLanguage = "English";
        }

        public PlayerStats()
        {

        }

        public static HLWNData Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }

        public static void Save()
        {
            DuckXML duckXML = new DuckXML();
            DXMLNode data = new DXMLNode("Data");
            data.Add(_data.Serialize());
            duckXML.Add(data);
            string path = DuckFile.optionsDirectory + "/HalloweenStats.data";
            SaveDuckXML(duckXML, path);
        }

        public static void Initialize()
        {
            _optionsMenu = CreateOptionsMenu();
        }

        public static UIMenu CreateOptionsMenu()
        {
            UIMenu men = new UIMenu("HALLOWEEN OPTIONS", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 190f, -1f, "@CANCEL@BACK @SELECT@SELECT", null, false);
            men.Add(new UIMenuItemSlider("Screenshake", null, new FieldBinding(Options.Data, "screenshake", 0f, 100f, 1f), 1f, default(Color)), true);
            men.Add(new UIText(" ", Color.White, UIAlign.Center, 0f, null), true);
            men.Add(new UIMenuItemSlider("Dictor Volume", null, new FieldBinding(Options.Data, "DictorVolume", 0f, 100f, 1f), 1f, default(Color)), true);
            men.Add(new UIText(" ", Color.White, UIAlign.Center, 0f, null), true);
            men.Add(new UIMenuItemToggle("Show buttons", null, new FieldBinding(Options.Data, "ShowButtons", 0f, 1f, 0.1f), default(Color), null, null, false, false), true);
            men.Add(new UIText(" ", Color.White, UIAlign.Center, 0f, null), true);
            men.Add(new UIMenuItemToggle("Show timer", null, new FieldBinding(Options.Data, "ShowTimer", 0f, 1f, 0.1f), default(Color), null, null, false, false), true);
            men.Add(new UIText(" ", Color.White, UIAlign.Center, 0f, null), true);
            men.Add(new UIMenuItemToggle("Show hints", null, new FieldBinding(Options.Data, "ShowHints", 0f, 1f, 0.1f), default(Color), null, null, false, false), true);
            men.Add(new UIText(" ", Color.White, UIAlign.Center, 0f, null), true);
            men.SetBackFunction(new UIMenuActionCloseMenuCallFunction(men, new UIMenuActionCloseMenuCallFunction.Function(Options.OptionsMenuClosed)));
            men.Close();
            return men;
        }

        public static void SaveDuckXML(DuckXML doc, string path)
        {
            DevConsole.Log("Stats saved to " + path, Color.White);
            DuckFile.CreatePath(Path.GetDirectoryName(path));
            try
            {
                if (File.Exists(path))
                {
                    File.SetAttributes(path, FileAttributes.Normal);
                }
            }
            catch (Exception)
            {

            }
            try
            {
                if (File.Exists(path))
                {
                    File.SetAttributes(path, FileAttributes.Normal);
                }
            }
            catch (Exception)
            {
            }
            string docString = doc.ToString();
            if (string.IsNullOrWhiteSpace(docString))
            {
                throw new Exception("Blank XML (" + path + ")");
            }
            File.WriteAllText(path, docString);
        }

        public static DuckXML LoadDuckXML(string path)
        {
            DevConsole.Log("Stats loaded from " + path, Color.White);
            DuckFile.CreatePath(Path.GetDirectoryName(path));
            //DuckFile.PrepareToLoadCloudFile(path);
            if (!File.Exists(path))
            {
                return null;
            }
            DuckXML result;
            try
            {
                result = DuckXML.Load(path);
            }
            catch
            {
                result = null;
            }
            return result;
        }

        public static void Load()
        {
            //Halloween.upd.ELO = Data.ELO;
            if (File.Exists(DuckFile.optionsDirectory + "/HalloweenStats.data"))
            {
                DuckXML xdocument = LoadDuckXML(DuckFile.optionsDirectory + "/HalloweenStats.data");
                if (xdocument != null)
                {
                    new Profile("", null, null, null, false, null);

                    IEnumerable<DXMLNode> enumerable = xdocument.Elements("Data");
                    if (enumerable != null)
                    {
                        foreach (DXMLNode xelement in enumerable.Elements<DXMLNode>())
                        {
                            if (xelement.Name == "HalloweenStats")
                            {
                                _data.Deserialize(xelement);
                                break;
                            }
                        }
                    }
                }
            }

        }

        public static void OpenOptionsMenu()
        {
            _removedOptionsMenu = false;
            _openedOptionsMenu = true;
            if (_optionsMenu != null)
            {
                Level.Add(_optionsMenu);
                _optionsMenu.Open();
                MonoMain.pauseMenu = _optionsMenu;
            }
        }
        public static void OptionsMenuClosed()
        {
            Save();
            if (openOnClose != null)
            {
                openOnClose.Open();
            }
        }

        public static void Update()
        {
            firstLaunch = Data.firstLaunch;
            Halloween.upd.ELO = Data.ELO;
            screenshake = Data.screenshake;
            DictorVolume = Data.DictorVolume;
            ShowButtons = Data.ShowButtons;
            ShowHints = Data.ShowHints;
            ShowTimer = Data.ShowTimer;
            language = Data.Language;
            //Halloween.upd.challengeID = Data.ChallengeID;
            //Halloween.upd.challengeProgression = Data.ChallengeProgress;

            if (_openedOptionsMenu && !_removedOptionsMenu && !_optionsMenu.open && !_optionsMenu.animating)
            {
                _openedOptionsMenu = false;
                _removedOptionsMenu = true;
                Level.Remove(_optionsMenu);
            }
        }
    }
}
