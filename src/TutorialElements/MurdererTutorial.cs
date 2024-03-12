using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    [EditorGroup("Halloween|Tutorial")]
    public class MurdererTutorial : Murder
    {
        public SinWave _puls = 0.1f;
        public MurdererTutorial(float xpos, float ypos) : base(xpos, ypos)
        {
            charges = 4;
        }
        public override void TrackerActivated()
        {
            foreach (MovingBody target in Level.CheckCircleAll<MovingBody>(_equippedDuck.position, 160))
            {               
                if (DetectedByJackal == false)
                {
                    DetectedByJackal = true;
                    if (_equippedDuck.profile.localPlayer)
                    {
                        DetectedByJackal = true;
                        if (_equippedDuck.profile.localPlayer)
                        {
                            SFX.Play(GetPath("SFX/ScannerJackal.wav"), ((float)PlayerStats.Data.DictorVolume / 100), 0.0f, 0.0f, false);
                            Level.Add(new SubtitlesManager(LanguageManager.GetPhrase("TA"), 180));
                        }
                    }
                    foreach (Duck d in Level.current.things[typeof(Duck)])
                    {
                        if (d.HasEquipment(typeof(Ghost)) && d.profile.localPlayer)
                        {
                            SFX.Play(GetPath("SFX/Tracked.wav"), ((float)PlayerStats.Data.DictorVolume / 100), 0.0f, 0.0f, false);
                            Level.Add(new SubtitlesManager(LanguageManager.GetPhrase("T"), 180));
                        }
                    }
                }
            }
        }
        public override void ScannerActivated()
        {
            foreach (MovingBody target in Level.CheckCircleAll<MovingBody>(_equippedDuck.position, 160))
            {
                Level.Add(new MurderMark(target.position.x + _puls*6f, target.position.y));
            }
        }
    }
}
