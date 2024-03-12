using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    public class Subtitles
    {
        public string TimeOut = "";
        public string hunterWin = "";
        public string firstMonologue = "";
        public string LastGhost = "";

        public string PrePhaseEnded = "";
        public string Remaining15 = "";
        public string Remaining30 = "";
        public string Remaining45 = "";

        public string GhostNear = "";
        public string HunterNear = "";

        public string ScannerActivate = "";
        public string TrackerActivate = "";
        public string Scanned = "";
        public string Tracked = "";

        public string ActionPhase = "Action phase";
        public string PreparationPhase = "Preparation phase";
        public string PlacableAdvice = "Hold @SHOOT@ to place device";
        public string PlacableBad = "Not enough space to deploy";
        public string ThrowableAdvice = "Throw to deploy device";

        //Map
        public string Coastline = "Coastline"; 
        public string Hospital = "Hospital";
        public string Prison = "Prison";
        public string Vault = "Vault tec.";
        public string Westwood = "Westwood";
        public string Mansion = "Mansion";
        public string Favela = "Favela";
        public string Nightcity = "Night city";
        public string Consulate = "Consulate"; 
        public string Factory = "Factory";
        public string Forest = "Forest"; 
        public string Arcade = "Arcade";
        public string Arctic = "Arctic";

        //Point Manager
        public string PMTrapDeployed = "";
        public string PMEnemyDied = "";
        public string PMTrapDestroyed = "";
        public string PMTimeIsOut = "";
        public string PMInvis = "";
        public string PMDetect = "";
        public string PMScan = "";
        public string PMTrack = "";
        public string PMLantern = "";
        public string PMTrapActivated = "";
        //Bonuses
        public string PMBSilentKiller = "";
        public string PMBAvoidHunter = "";

        //Hints
        public string HINTHammer = "";
        public string HINTSpear = "";
        public string HINTChainsaw = "";

        public string Hammer;
        public string Spear;
        public string Chainsaw;

        public string HINTEDD = "";
        public string HINTKapkan = "";
        public string HINTClaymore = "";
        public string HINTGasSmoke = "";
        public string HINTGrzmot = "";

        public string EDD;
        public string Kapkan;
        public string Claymore;
        public string GasSmoke;
        public string Grzmot;

        public string Lamp;
        public string HLamp;

        public string Scan;
        public string HScan;

        public string Tracker;
        public string HTracker;

        public string Invis;
        public string HInvis;

        public string Challenge1Name;
        public string Challenge1Desc; 
        public string Challenge2Name;
        public string Challenge2Desc; 
        public string Challenge3Name;
        public string Challenge3Desc;
        public string Challenge4Name;
        public string Challenge4Desc; 
        public string Challenge5Name;
        public string Challenge5Desc; 
        public string Challenge6Name;
        public string Challenge6Desc; 
        public string Challenge7Name;
        public string Challenge7Desc; 
        public string Challenge8Name;
        public string Challenge8Desc;
        public string Challenge9Name;
        public string Challenge9Desc;
        public string Challenge10Name;
        public string Challenge10Desc;
        public string Challenge11Name;
        public string Challenge11Desc;
        public string Challenge12Name;
        public string Challenge12Desc;
        public string Challenge13Name;
        public string Challenge13Desc;
        public string Challenge14Name;
        public string Challenge14Desc; 
        public string Challenge15Name;
        public string Challenge15Desc; 
        public string Challenge16Name;
        public string Challenge16Desc;

        //Settings
        public string Screenshake;
        public string DictorVolume;
        public string ShowHints;
        public string ShowKeys;
        public string ShowTimer;
        public string subt;
        public string textLang;
        public string exit;

        //Words
        public string damage;
        public string lethal;
        public Subtitles()
        {
            TimeOut = "The world would never be the same";
            hunterWin = "The world is safe for now";
            firstMonologue = "This halloween terror has a new name. Yours";
            LastGhost = "The monsters were downed to one";

            PrePhaseEnded = "The hunters are coming - bait them into your traps. But above all survive";
            Remaining15 = "15 seconds remaining";
            Remaining30 = "30 seconds";
            Remaining45 = "45 seconds remaining";

            GhostNear = "They see you";
            HunterNear = "The're here";

            ScannerActivate = "You've got some time to kill";
            TrackerActivate = "You can run. But you can't hide";
            Scanned = "You have been spotted";
            Tracked = "One of you is being tracked";

            PMTrapDeployed = "Trap deployed";
            PMEnemyDied = "Enemy died";
            PMTrapDestroyed = "Trap destroyed";
            PMTimeIsOut = "Time is out";
            PMInvis = "Invisibility activated";
            PMDetect = "Enemy detected";
            PMScan = "Scan activated";
            PMTrack = "Tracker activated";
            PMLantern = "Lantern deployed";
            PMTrapActivated = "Trap activated";

            PMBSilentKiller = "Silent killer";
            PMBAvoidHunter = "Avoided hunter";

            HINTHammer = "Hammer-like Cleaver, deadly even for guys in ghost costumes";
            HINTChainsaw = "Easily overheats and hitted target will die after some time";
            HINTSpear = "Provides long dash and ability of staying invisible, but slows you down";

            Hammer = "Cleaver";
            Chainsaw = "Chainsaw";
            Spear = "Beast claws";

            HINTEDD = "Has to be placed on door frames and causes enemy bleeding";
            EDD = "Wire trap";

            HINTGasSmoke = "Depletes gas after being thrown, which deals DPS and slows down hunters";
            GasSmoke = "Gas canister";

            HINTClaymore = "Small mine, that leaves lasers if uncovered";
            Claymore = "Claymore mine";

            HINTKapkan = "Deals damage to trapped murderer, until they die or be saved by their teammate";
            Kapkan = "Bear trap";

            HINTGrzmot = "After sticking to surface, flashes hunter for short amount of time";
            Grzmot = "Flash mine";

            Scan = "Movement scanner";
            HScan = "If targets is moving, they will be revealed to you";

            Tracker = "Footprints analyzer";
            HTracker = "Reveals target's footprints and pings the freshest one in your radius";

            Lamp = "Ghost lamp";
            HLamp = "Slows down target if they are close and pings them";

            Invis = "Retreat";
            HInvis = "Become faster and invisible for short amount of time";

            Challenge1Name = "James Miller's day";
            Challenge1Desc = "Kill 10 ghosts with cleaver";

            Challenge2Name = "Leather face's day";
            Challenge2Desc = "Kill 10 ghosts with saw";

            Challenge3Name = "Frederick Krueger's day";
            Challenge3Desc = "Kill 10 ghosts with claws";

            Challenge4Name = "Watch your steps";
            Challenge4Desc = "Destroy 50 traps";

            Challenge5Name = "Ghost pursuer";
            Challenge5Desc = "Detect 25 ghosts";

            Challenge6Name = "Master of single";
            Challenge6Desc = "Set 5 same traps in 1 round 5 times";

            Challenge7Name = "The clock is ticking";
            Challenge7Desc = "Win 5 rounds as ghost in timeout";

            Challenge8Name = "Uncatchable";
            Challenge8Desc = "Escape 10 hunters using invisibility";

            Challenge9Name = "Preparation";
            Challenge9Desc = "Deploy 50 traps";

            Challenge10Name = "Diversity day";
            Challenge10Desc = "Set each type of traps in 1 round 5 times";

            Challenge11Name = "Hunted hunter";
            Challenge11Desc = "Get 20 kills with traps";

            Challenge12Name = "Caustics day";
            Challenge12Desc = "Get 3 kills with gas trap";

            Challenge13Name = "Lone bears";
            Challenge13Desc = "Get 3 kills with bear trap";

            Challenge14Name = "Trickster";
            Challenge14Desc = "Get kill with flash mine";

            Challenge15Name = "Hellraiser";
            Challenge15Desc = "Light all pumpkins and win";

            Screenshake = "Screenshake";
            DictorVolume = "Announcer vol.";
            ShowHints = "Show hints";
            ShowKeys = "Show key binds";
            ShowTimer = "Show timer";
            subt = "Use subtitles";
            textLang = "Language";
            exit = "EXIT";

            Coastline = "Coastline";
            Hospital = "Hospital";
            Prison = "Prison";
            Vault = "Vault tec.";
            Westwood = "Westwood";
            Mansion = "Mansion";
            Favela = "Favela";
            Nightcity = "Night city";
            Consulate = "Consulate";
            Factory = "Factory";
            Forest = "Forest";
            Arcade = "Arcade";
            Arctic = "Arctic";

            ActionPhase = "Action phase";
            PreparationPhase = "Preparation phase";
            PlacableAdvice = "Hold @SHOOT@ to place device";
            PlacableBad = "Not enough space to deploy";
            ThrowableAdvice = "Throw to deploy device";

            damage = "Damage";
            lethal = "Lethal";
        }
    }
}
