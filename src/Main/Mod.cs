using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;

// The title of your mod, as displayed in menus
[assembly: AssemblyTitle("Halloween event")]

// The author of the mod
[assembly: AssemblyCompany("FaecTerr")]

// The description of the mod
[assembly: AssemblyDescription("One gamemode event for one month")]

// The mod's version
[assembly: AssemblyVersion("1.0.0.0")]


namespace DuckGame.Halloween
{
    public class Halloween : Mod
    {
        public static ulong[] VIPid { get; private set; }
        //Skin changer config
        public static Thread tr;
        public static LanguageManager lan = new LanguageManager();
        public static Tex2D CorrectTexture(Tex2D tex, bool recolor = false, Vec3 color = default(Vec3))
        {
            if (recolor)
                return Graphics.Recolor(tex, color);
            RenderTarget2D t = new RenderTarget2D(tex.width, tex.height);
            Graphics.SetRenderTarget(t);
            Graphics.Clear(new Color(0, 0, 0, 0));
            Graphics.screen.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
            Graphics.Draw(tex, new Vec2(), new Rectangle?(), Color.White, 0.0f, new Vec2(), new Vec2(1f, 1f), SpriteEffects.None, (Depth)0.5f);
            Graphics.screen.End();
            Graphics.device.SetRenderTarget(null);
            return t;
        }
        
        public static updater upd;
        public static Dictionary<string, SkinChanger> customHats = new Dictionary<string, SkinChanger>();
        // The mod's priority; this property controls the load order of the mod.
        public override Priority priority
        {
            get { return base.priority; }
        }

        // This function is run before all mods are finished loading.
        protected override void OnPreInitialize()
        {
            VIPid = new ulong[]{
                  76561198352294502, //Faecterr - dev
                  76561198305366691, //Faktor77 - dev
                  76561198392352745, //Pipidaster - dev
                  76561198209585131, //Kolosandr - donator
                  76561198219898521, //Doomov - friend
                  76561198246538686, //GreenDash - friend
                  76561198434662428, //ALOXA - friend
                  76561198254456698, //Nek0b0y - top donator
                  76561198163128497  //Antikore - spanish translator
            };
            string path = DuckFile.optionsDirectory + "/HalloweenStats.data";
            //PlayerStats.Save();
           
            base.OnPreInitialize();
        }
        // This function is run after all mods are loaded.
        protected override void OnPostInitialize()
        {
            base.OnPostInitialize();
         //   string name = Halloween.tr.CurrentCulture.ThreeLetterWindowsLanguageName;
          //  DevConsole.Log("System language name is " + name, Color.Aqua);
            PlayerStats.Load();
            PlayerStats.Save();
            UpdateNMs();
            tr = new Thread(wait);
            tr.Start();
            UpdateCH();
            copyLevels();

            if (PlayerStats.firstLaunch)
            {
                string name = "";
                if (tr != null)
                {
                    name = tr.CurrentCulture.ThreeLetterWindowsLanguageName;
                }
                if (name == "RUS" || name == "РУС")
                {
                    PlayerStats.language = 1;
                }
                else if (name == "SPA" || name == "ESS" || name == "ESB" || name == "ESP" || name == "ESO" || name == "ESC" || name == "ESD" || name == "ESN" || name == "ESL"
                    || name == "ESF" || name == "ESG" || name == "ESH" || name == "ESM" || name == "ESI" || name == "ESA" || name == "ESR" || name == "ESU" || name == "ESZ"
                    || name == "ESE" || name == "EST" || name == "ESY" || name == "ESV")
                {
                    PlayerStats.language = 2;
                }
                else
                {
                    PlayerStats.language = 0;
                }
                PlayerStats.firstLaunch = false;
            }
        }

        public void UpdateCH()
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetCustomAttributes(typeof(CustomHat), true).Length > 0))
            {
                
                var attr = (type.GetCustomAttributes(typeof(CustomHat), true)[0] as CustomHat);
                Teams.core.teams.Add(new Team(attr.Name, attr.TexturePath));
                var skin = Activator.CreateInstance(type) as SkinChanger;
                if (!String.IsNullOrEmpty(skin.equipmentPrefix))
                {
                 
                    List<string> equipments;
                    if (skin.equipmentPrefix.Contains("/"))
                        equipments = new DirectoryInfo(Path.Combine(configuration.contentDirectory.Replace('/', '\\'), skin.equipmentPrefix.Replace('/', '\\'))).GetFiles("*.png").Select<FileInfo, string>(x => x.FullName.Replace('\\', '/').Replace(".png", "").Substring(configuration.contentDirectory.Length)).ToList();
                    else
                        equipments = new DirectoryInfo(configuration.contentDirectory.Replace('/', '\\')).GetFiles(skin.equipmentPrefix + "*.png", SearchOption.AllDirectories).Select<FileInfo, string>(x => x.FullName.Replace('\\', '/').Replace(".png", "").Substring(configuration.contentDirectory.Length)).ToList();
                    skin.equipment = equipments.ToDictionary(x => x.Substring(x.LastIndexOf(skin.equipmentPrefix) + skin.equipmentPrefix.Length));
                }
                else
                    skin.equipment = new Dictionary<string, string>();
                customHats.Add(attr.Name, skin);
            }
        }
        public static void UpdateNMs()
        {
            Network.typeToMessageID.Clear();
            ushort key = 1;
            foreach (Type type in Editor.GetSubclasses(typeof(NetMessage)))
            {
                if (type.GetCustomAttributes(typeof(FixedNetworkID), false).Length != 0)
                {
                    FixedNetworkID customAttribute = (FixedNetworkID)type.GetCustomAttributes(typeof(FixedNetworkID), false)[0];
                    if (customAttribute != null)
                    {
                        Network.typeToMessageID.Add(type, customAttribute.FixedID);
                    }
                }
            }
            foreach (Type type in Editor.GetSubclasses(typeof(NetMessage)))
            {
                if (!Network.typeToMessageID.ContainsValue(type))
                {
                    while (Network.typeToMessageID.ContainsKey(key))
                        ++key;
                    Network.typeToMessageID.Add(type, key);
                    ++key;
                }
            }
        }

        private byte[] GetMD5Hash(byte[] sourceBytes)
        {
            return new MD5CryptoServiceProvider().ComputeHash(sourceBytes);
        }

        void wait()
        {
            while (Level.current == null || !(Level.current.ToString() == "DuckGame.TitleScreen") && !(Level.current.ToString() == "DuckGame.TeamSelect2"))
                Thread.Sleep(200);
            upd = new updater();
            AutoUpdatables.Add(upd);
        }

        //Setting up maps
        private static bool FilesAreEqual(FileInfo first, FileInfo second)
        {
            if (first.Length != second.Length)
            {
                return false;
            }
            int iterations = (int)Math.Ceiling((double)first.Length / 8.0);
            using (FileStream fs = first.OpenRead())
            {
                using (FileStream fs2 = second.OpenRead())
                {
                    byte[] one = new byte[8];
                    byte[] two = new byte[8];
                    for (int i = 0; i < iterations; i++)
                    {
                        fs.Read(one, 0, 8);
                        fs2.Read(two, 0, 8);
                        if (BitConverter.ToInt64(one, 0) != BitConverter.ToInt64(two, 0))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private static void copyLevels()
        {
            string levelFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "DuckGame\\Levels\\HalloweenPack");
            if (!Directory.Exists(levelFolder))
            {
                Directory.CreateDirectory(levelFolder);
            }
            foreach (string sourcePath in Directory.GetFiles(Mod.GetPath<Halloween>("Levels\\Pack")))
            {
                string destPath = Path.Combine(levelFolder, Path.GetFileName(sourcePath));
                bool file_exists = File.Exists(destPath);
                if (!file_exists || !Halloween.FilesAreEqual(new FileInfo(sourcePath), new FileInfo(destPath)))
                {
                    if (file_exists)
                    {
                        File.Delete(destPath);
                    }
                    File.Copy(sourcePath, destPath);
                }
            }
        }
    }
}


