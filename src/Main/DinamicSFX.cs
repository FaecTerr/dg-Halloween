using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DuckGame.Halloween
{
    public class DinamicSFX 
    {
        public static void PlayDef(Vec2 targetPos, Vec2 centerPos, float volumeMod, float range, string sample, bool looped = false)
        {    
            float volume = (range/(targetPos - centerPos).length) * volumeMod - 0.1f;
            if (volume > 1)
            {
                volume = 1;
            }
            if (volume < 0)
            {
                volume = 0;
            }
            SFX.Play(Mod.GetPath<Halloween>(sample), volume, 0f, 0f, looped);
        }
        public static void Play(Vec2 targetPos, Vec2 centerPos, float volumeMod, float pitchMod, string sample, float range = 320f, bool looped = false)
        {
            float silence = volumeMod;
            float pitch = pitchMod;
            float panning =  targetPos.x - centerPos.x;
            if (Math.Abs(panning) <= 16f)
            {
                panning = 0;
            }
            if (Math.Abs(panning) <= 32f)
            {
                if (panning < 0)
                {
                    panning = 0.35f;
                }
                else
                {
                    panning = -0.35f;
                }
            }
            if (Math.Abs(panning) <= 48f)
            {
                if (panning < 0)
                {
                    panning = 0.5f;
                }
                else
                {
                    panning = -0.5f;
                }
            }
            else
            {
                if (panning < 0)
                {
                    panning = 0.75f;
                }
                else
                {
                    panning = -0.75f;
                }
            }
            float volume = (range / (targetPos - centerPos).length) * silence - 0.1f;
            foreach (Block b in Level.CheckLineAll<Block>(targetPos, centerPos))
            {
                volume *= 0.7f;
                pitch -= 0.5f;
                if (centerPos.x > targetPos.x)
                {
                    panning -= 0.1f;
                }
                else
                {
                    panning += 0.1f;
                }
            }
            if (volume > 1)
            {
                volume = 1;
            }
            if (pitch < -1)
            {
                pitch = -1;
            }
            if (pitch > 1)
            {
                pitch = 1;
            }
            if (panning < -1)
            {
                panning = -1;
            }
            if (panning > 1)
            {
                panning = 1;
            }
            if (volume < 0)
            {
                volume = 0;
            }
            SFX.Play(Mod.GetPath<Halloween>(sample), volume, pitch, panning, looped);
        }
    }
}
