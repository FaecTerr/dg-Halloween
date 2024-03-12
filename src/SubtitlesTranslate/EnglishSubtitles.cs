using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    public class EnglishSubtitles : Subtitles
    {
        public EnglishSubtitles()
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
            
            HINTHammer = "Classic sledge hammer, deadly even for guys in ghost costumes";
            HINTChainsaw = "Easily overheats and hitted target will die after some time";
            HINTSpear = "Provides long dash and ability of staying invisible, but slows you down";

            Hammer = "Hammer";
            Chainsaw = "Chainsaw";
            Spear = "Beast claws";

            HINTEDD = "Has to be placed on door frames and causes enemy bleeding";
            EDD = "Wire trap";

            HINTGasSmoke = "Depletes gas after being thrown, which deals DPS and slows down hunters";
            GasSmoke = "Gas canister";

            HINTClaymore = "Small mine, that leaves lasers if uncovered";
            Claymore = "Claymore mine";

            HINTKapkan = "Deals damage to trapped murderer, until they die or be saved by their teammate"; 
            Kapkan = "Kapkan";

            HINTGrzmot = "After sticking to surface, flashes hunter for short amount of time";
            Grzmot = "Flash mine";
        }
    }
}

