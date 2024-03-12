using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Globalization;

namespace DuckGame.Halloween
{
    public class LanguageManager
    {
        public static string SplitInLines(string text, int lineSize)
        {
            string updatedText = "";
            string[] words = text.Split(' ');
            int charCount = 0;

            for (int i = 0; i < words.Length; i++)
            {
                charCount += words[i].Length;
                if (charCount > lineSize)
                {
                    updatedText += '\n';
                    charCount = 0;
                }
                updatedText += words[i];
                updatedText += ' ';
            }


            return updatedText;
        }
        public static string GetPhrase(string phrase)
        {            
            string name = "";
            if (Halloween.tr != null)
            {
                name = Halloween.tr.CurrentCulture.ThreeLetterWindowsLanguageName;
            }

            //DevConsole.Log(name, Color.White);
            Subtitles lang = new Subtitles();

            /*if (name == "RUS" || name == "РУС")
            {
                lang = new RussianSubtitles();
            }
            else if (name == "SPA" || name == "ESS" || name == "ESB" || name == "ESP" || name == "ESO" || name == "ESC" || name == "ESD" || name == "ESN" || name == "ESL"
                || name == "ESF" || name == "ESG" || name == "ESH" || name == "ESM" || name == "ESI" || name == "ESA" || name == "ESR" || name == "ESU" || name == "ESZ" 
                || name == "ESE" || name == "EST" || name == "ESY" || name == "ESV")
            {
                lang = new SpanishSubtitles();
            }
            else
            {
                lang = new EnglishSubtitles();
            }*/

            if(PlayerStats.language == 0)
            {
                lang = new EnglishSubtitles();
            }
            if (PlayerStats.language == 1)
            {
                lang = new RussianSubtitles();
            }
            if (PlayerStats.language == 2)
            {
                lang = new SpanishSubtitles();
            }

            if (lang != null)
            {
                string text = "";
                if (phrase == "TO")
                {
                    text = lang.TimeOut;
                }
                if (phrase == "HW")
                {
                    text = lang.hunterWin;
                }
                if (phrase == "FM")
                {
                    text = lang.firstMonologue;
                }
                if (phrase == "LG")
                {
                    text = lang.LastGhost;
                }
                if (phrase == "PPE")
                {
                    text = lang.PrePhaseEnded;
                }
                if (phrase == "R15")
                {
                    text = lang.Remaining15;
                }
                if (phrase == "R30")
                {
                    text = lang.Remaining30;
                }
                if (phrase == "R45")
                {
                    text = lang.Remaining45;
                }

                if (phrase == "GN")
                {
                    text = lang.GhostNear;
                }
                if (phrase == "HN")
                {
                    text = lang.HunterNear;
                }

                if (phrase == "SA")
                {
                    text = lang.ScannerActivate;
                }
                if (phrase == "TA")
                {
                    text = lang.TrackerActivate;
                }
                if (phrase == "S")
                {
                    text = lang.Scanned;
                }
                if (phrase == "T")
                {
                    text = lang.Tracked;
                }

                if (phrase == "PM1")
                {
                    text = lang.PMTrapDeployed;
                }
                if (phrase == "PM2")
                {
                    text = lang.PMEnemyDied;
                }
                if (phrase == "PM3")
                {
                    text = lang.PMTrapDestroyed;
                }
                if (phrase == "PM4")
                {
                    text = lang.PMTimeIsOut;
                }
                if (phrase == "PM5")
                {
                    text = lang.PMInvis;
                }
                if (phrase == "PM6")
                {
                    text = lang.PMDetect;
                }
                if (phrase == "PM7")
                {
                    text = lang.PMScan;
                }
                if (phrase == "PM8")
                {
                    text = lang.PMTrack;
                }
                if (phrase == "PM9")
                {
                    text = lang.PMLantern;
                }
                if (phrase == "PM10")
                {
                    text = lang.PMTrapActivated;
                }
                if (phrase == "PMSK")
                {
                    text = lang.PMBSilentKiller;
                }
                if (phrase == "PMAH")
                {
                    text = lang.PMBAvoidHunter;
                }

                if (phrase == "H CH")
                {
                    text = lang.HINTChainsaw;
                }
                if (phrase == "CH")
                {
                    text = lang.Chainsaw;
                }
                if (phrase == "H HAM")
                {
                    text = lang.HINTHammer;
                }
                if (phrase == "HAM")
                {
                    text = lang.Hammer;
                }
                if (phrase == "H SP")
                {
                    text = lang.HINTSpear;
                }
                if (phrase == "SP")
                {
                    text = lang.Spear;
                }

                if (phrase == "H KAP")
                {
                    text = lang.HINTKapkan;
                }
                if (phrase == "KAP")
                {
                    text = lang.Kapkan;
                }

                if (phrase == "H GR")
                {
                    text = lang.HINTGrzmot;
                }
                if (phrase == "GR")
                {
                    text = lang.Grzmot;
                }

                if (phrase == "H EDD")
                {
                    text = lang.HINTEDD;
                }
                if (phrase == "EDD")
                {
                    text = lang.EDD;
                }

                if (phrase == "H CL")
                {
                    text = lang.HINTClaymore;
                }
                if (phrase == "CL")
                {
                    text = lang.Claymore;
                }

                if (phrase == "H GS")
                {
                    text = lang.HINTGasSmoke;
                }
                if (phrase == "GS")
                {
                    text = lang.GasSmoke;
                }

                if (phrase == "SCAN")
                {
                    text = lang.Scan;
                }
                if (phrase == "HSCAN")
                {
                    text = lang.HScan;
                }

                if (phrase == "TREK")
                {
                    text = lang.Tracker;
                }
                if (phrase == "HTREK")
                {
                    text = lang.HTracker;
                }

                if (phrase == "LAMP")
                {
                    text = lang.Lamp;
                }
                if (phrase == "HLAMP")
                {
                    text = lang.HLamp;
                }


                if (phrase == "CHAL0N")
                {
                    text = lang.Challenge1Name;
                }
                if (phrase == "CHAL0D")
                {
                    text = lang.Challenge1Desc;
                }
                if (phrase == "CHAL1N")
                {
                    text = lang.Challenge2Name;
                }
                if (phrase == "CHAL1D")
                {
                    text = lang.Challenge2Desc;
                }
                if (phrase == "CHAL2N")
                {
                    text = lang.Challenge3Name;
                }
                if (phrase == "CHAL2D")
                {
                    text = lang.Challenge3Desc;
                }
                if (phrase == "CHAL3N")
                {
                    text = lang.Challenge4Name;
                }
                if (phrase == "CHAL3D")
                {
                    text = lang.Challenge4Desc;
                }
                if (phrase == "CHAL4N")
                {
                    text = lang.Challenge5Name;
                }
                if (phrase == "CHAL4D")
                {
                    text = lang.Challenge5Desc;
                }
                if (phrase == "CHAL5N")
                {
                    text = lang.Challenge6Name;
                }
                if (phrase == "CHAL5D")
                {
                    text = lang.Challenge6Desc;
                }
                if (phrase == "CHAL6N")
                {
                    text = lang.Challenge7Name;
                }
                if (phrase == "CHAL6D")
                {
                    text = lang.Challenge7Desc;
                }
                if (phrase == "CHAL7N")
                {
                    text = lang.Challenge8Name;
                }
                if (phrase == "CHAL7D")
                {
                    text = lang.Challenge8Desc;
                }
                if (phrase == "CHAL8N")
                {
                    text = lang.Challenge9Name;
                }
                if (phrase == "CHAL8D")
                {
                    text = lang.Challenge9Desc;
                }
                if (phrase == "CHAL9N")
                {
                    text = lang.Challenge10Name;
                }
                if (phrase == "CHAL9D")
                {
                    text = lang.Challenge10Desc;
                }
                if (phrase == "CHAL10N")
                {
                    text = lang.Challenge11Name;
                }
                if (phrase == "CHAL10D")
                {
                    text = lang.Challenge11Desc;
                }
                if (phrase == "CHAL11N")
                {
                    text = lang.Challenge12Name;
                }
                if (phrase == "CHAL11D")
                {
                    text = lang.Challenge12Desc;
                }
                if (phrase == "CHAL12N")
                {
                    text = lang.Challenge13Name;
                }
                if (phrase == "CHAL12D")
                {
                    text = lang.Challenge13Desc;
                }
                if (phrase == "CHAL13N")
                {
                    text = lang.Challenge14Name;
                }
                if (phrase == "CHAL13D")
                {
                    text = lang.Challenge14Desc;
                }
                if (phrase == "CHAL14N")
                {
                    text = lang.Challenge15Name;
                }
                if (phrase == "CHAL14D")
                {
                    text = lang.Challenge15Desc;
                }

                if (phrase == "SSS")
                {
                    text = lang.Screenshake;
                }
                if (phrase == "SDV")
                {
                    text = lang.DictorVolume;
                }
                if (phrase == "SSH")
                {
                    text = lang.ShowHints;
                }
                if (phrase == "SKB")
                {
                    text = lang.ShowKeys;
                }
                if (phrase == "SST")
                {
                    text = lang.ShowTimer;
                }
                if (phrase == "SSUB")
                {
                    text = lang.subt;
                }
                if (phrase == "STL")
                {
                    text = lang.textLang;
                }
                if(phrase == "EXIT")
                {
                    text = lang.exit;
                }

                if(phrase == "Coastline")
                {
                    text = lang.Coastline;
                }
                if (phrase == "Hospital")
                {
                    text = lang.Hospital;
                }
                if (phrase == "Prison")
                {
                    text = lang.Prison;
                }
                if (phrase == "Vault tec.")
                {
                    text = lang.Vault;
                }
                if (phrase == "Westwood")
                {
                    text = lang.Westwood;
                }
                if (phrase == "Mansion")
                {
                    text = lang.Mansion;
                }
                if (phrase == "Favela")
                {
                    text = lang.Favela;
                }
                if (phrase == "Night city")
                {
                    text = lang.Nightcity;
                }
                if (phrase == "Consulate")
                {
                    text = lang.Consulate;
                }
                if (phrase == "Factory")
                {
                    text = lang.Factory;
                }
                if (phrase == "Forest")
                {
                    text = lang.Forest;
                }
                if (phrase == "Arcade")
                {
                    text = lang.Arcade;
                }
                if (phrase == "Arctic")
                {
                    text = lang.Arctic;
                }

                if(phrase == "Damage")
                {
                    text = lang.damage;
                }
                if (phrase == "Lethal")
                {
                    text = lang.lethal;
                }

                if (phrase == "INV")
                {
                    text = lang.Invis;
                }
                if (phrase == "HINV")
                {
                    text = lang.HInvis;
                }

                if (phrase == "Prep")
                {
                    text = lang.PreparationPhase;
                }
                if (phrase == "Act")
                {
                    text = lang.ActionPhase;
                }

                return text;
            }
            return "";
        }
    }
}
