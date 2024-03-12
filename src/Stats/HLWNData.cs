using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    public class HLWNData : DataClass
    {
        public HLWNData()
        {
            _nodeName = "HalloweenStats";
            screenshake = 50;
            DictorVolume = 100;
            ShowButtons = true;
            ShowTimer = true;
            ShowHints = true;
            subtitles = true;
            ELO = 500;
            SubLanguage = "English";
            ChallengeID = -1;
            ChallengeProgress = 0;

        }
        public bool firstLaunch
        {
            get
            {
                return PlayerStats.firstLaunch;
            }
            set
            {
                PlayerStats.firstLaunch = value;
            }
        }
        public int screenshake
        {
            get
            {
                return PlayerStats.screenshake;
            }
            set
            {
                PlayerStats.screenshake = value;
            }
        }
        public int DictorVolume
        {
            get
            {
                return PlayerStats.DictorVolume;
            }
            set
            {
                PlayerStats.DictorVolume = value;
            }
        }

        public bool ShowButtons
        {
            get
            {
                return PlayerStats.ShowButtons;
            }
            set
            {
                PlayerStats.ShowButtons = value;
            }
        }

        public bool ShowTimer
        {
            get
            {
                return PlayerStats.ShowTimer;
            }
            set
            {
                PlayerStats.ShowTimer = value;
            }
        }

        public bool ShowHints
        {
            get
            {
                return PlayerStats.ShowHints;
            }
            set
            {
                PlayerStats.ShowHints = value;
            }
        }
        public bool subtitles
        {
            get
            {
                return PlayerStats.subtitles;
            }
            set
            {
                PlayerStats.subtitles = value;
            }
        }
        public string SubLanguage
        {
            get
            {
                return PlayerStats.SubLanguage;
            }
            set
            {
                PlayerStats.SubLanguage = value;
            }
        }
        public int ELO
        {
            get
            {
                return PlayerStats.ELO;
            }
            set
            {
                PlayerStats.ELO = value;
            }
        }
        public int ChallengeID
        {
            get
            {
                return PlayerStats.ChallengeID;
            }
            set
            {
                PlayerStats.ChallengeID = value;
            }
        }

        public int ChallengeProgress
        {
            get
            {
                return PlayerStats.ChallengeProgress;
            }
            set
            {
                PlayerStats.ChallengeProgress = value;
            }
        }
        public int Language
        {
            get
            {
                return PlayerStats.language;
            }
            set
            {
                PlayerStats.language = value;
            }
        }
    }
}
