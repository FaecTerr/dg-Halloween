using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace DuckGame.Halloween
{
    public class HLWN : Thing
    {
        public bool init;
        public SpriteMap _overlay;
        public SinWave pulse1 = 0.1f;
        public SinWave pulse2 = 0.1f;
        //public SpriteMap _sprite = new SpriteMap(Mod.GetPath<C44P>("MurderVisionCircle.png"), 160, 90);
        public int heart = 12;
        public bool doubl;
        public bool IsLocalDuckAffected;

        public bool OneGhostRemaining;
        public bool NoGhostRemaining;

        public HLWN() : base()
        {
            _overlay = new SpriteMap(Mod.GetPath<Halloween>("Sprites/Fogparallax.png"), 90, 90);
            _overlay.center = new Vec2(45, 45);
            _overlay.alpha = 0.5f;
            _overlay.scale = new Vec2(2f, 2f);
        }

        public override void Update()
        {
            base.Update();
            if (heart > 0)
            {
                heart -= 1;
            }
            else
            {
                if (doubl)
                {
                    heart = 45;
                }
                else
                {
                    heart = 75;
                }
                doubl = !doubl;
                SFX.Play(GetPath("SFX/Heartbeats.wav"));
            }
            int ghostsAlive = 0;
            foreach (Duck d in Level.current.things[typeof(Duck)])
            {
                if (d.dead == false && d.HasEquipment(typeof(CTEquipment)))
                {
                    ghostsAlive++;
                }
            }
            
            if(OneGhostRemaining == false && ghostsAlive == 1)
            {
                //SFX.Play(GetPath("SFX/OneGhostremaining.wav"), 0.6f, 0.0f, 0.0f, false);
                OneGhostRemaining = true;
            }
            if (NoGhostRemaining == false && ghostsAlive == 0)
            {
                
                NoGhostRemaining = true;
            }
            if (init == false)
            {
                if(GameMode.numMatchesPlayed == 0)
                {
                    SFX.Play(GetPath("SFX/FirstMonologue1.wav"), 1f*((float)PlayerStats.DictorVolume/100), 0.0f, 0.0f, false);
                    Level.Add(new SubtitlesManager(LanguageManager.GetPhrase("FM"), 240));
                }
                int count = 0;
                foreach (Duck d in Level.current.things[typeof(Duck)])
                {
                    if (Array.Exists(Halloween.VIPid, e => e == d.profile.steamID))
                    {
                        count++;
                    }
                }
                Music.Load(Mod.GetPath<Halloween>("Music/ScaryAmbient.ogg"), true);
                Music.Play(Mod.GetPath<Halloween>("Music/ScaryAmbient.ogg"));

                init = true;
            }
        }
        public override void Draw()
        {
            base.Draw();
            Graphics.Draw(_overlay, 0, Level.current.camera.position.x + Level.current.camera.width/2 + pulse1, Level.current.camera.position.y + Level.current.camera.height/2 + pulse2, Level.current.camera.width/90, Level.current.camera.height/90);
        }
    }
}
