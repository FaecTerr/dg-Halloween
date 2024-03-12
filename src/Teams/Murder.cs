using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckGame.Halloween
{
    [BaggedProperty("canSpawn", false)]
    [EditorGroup("Halloween")]
    public class Murder : TEquipment
    {
        public int sunset = 180;

        public SpriteMap slots;
        public SpriteMap numer;
        public SpriteMap icons;
        public Sprite vision;

        public int advice = 480;

        public int CircleSize = 2100;

        public bool scan = false;
        public int charges = 4;
        public float reload = 8f;
        public float duration;
        public int abilityNum;
        public bool DetectedByHeartbeat;
        public bool DetectedByJackal;
        public bool DetectedByLion;

        private int scanAgain;

        public Murder(float xpos, float ypos) : base(xpos, ypos)
        {
            slots = new SpriteMap(GetPath("Sprites/Item"), 32, 32);
            slots.CenterOrigin();
            numer = new SpriteMap(GetPath("Sprites/ItemCounter"), 14, 14);
            numer.CenterOrigin();
            icons = new SpriteMap(GetPath("Sprites/PlayerIcons"), 18, 18);
            icons.CenterOrigin();
            vision = new Sprite((GetPath("Sprites/MurderCircle")));
            vision.CenterOrigin();
            _pickupSprite = new SpriteMap(GetPath("Sprites/Blank"), 32, 32);
            _sprite = new SpriteMap(GetPath("Sprites/Blank"), 32, 32, false);
            base.graphic = _sprite;
            layer = Layer.Foreground;
            depth = 0.6f;
            _editorName = ("Hunter");
            _visibleInGame = true;
            _editorName = "Murderer";
        }
        public override void loadTextures()
        {
            ReplaceSprite = sprite("Sprites/murderer");
            ReplaceQuack = sprite("Sprites/murderer");
            ReplaceArm = armsprite("Sprites/murderArms");
            ReplaceControlled = sprite("Sprites/murderer");
            recolor = true;
        }
        public virtual void TrackerActivated()
        {
            foreach (Step target in Level.current.things[typeof(Step)])
            {
                if (_equippedDuck != null)
                {
                    if(_equippedDuck.profile.localPlayer)
                        target.alpha = 1f;
                }
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
        public virtual void ScannerActivated()
        {
            if(scanAgain > 0)
            {
                scanAgain--;
            }
            foreach (Duck target in Level.current.things[typeof(Duck)])
            {
                if (target != null)
                {
                    if (target.HasEquipment(typeof(Ghost)) && duration <= 5.1f && (target.hSpeed > 0.1f || target.hSpeed < -0.1f || target.vSpeed > 0.2f || target.vSpeed < -0.2f) && scanAgain <= 0)
                    {
                        scanAgain = 3;
                        Level.Add(new MurderMark(target.position.x, target.position.y));
                        if (DetectedByLion == false && _equippedDuck != null)
                        {
                            DetectedByLion = true;
                            points += 25;
                            if (_equippedDuck.profile.localPlayer)
                            {
                                /*if (Halloween.upd.challengeID == 4)
                                {
                                    if (Halloween.upd.challengeProgression < Halloween.upd.challengeTarget)
                                    {
                                        Halloween.upd.challengeProgression += 1;
                                    }
                                }*/
                                Level.Add(new PointsGetManager(25, "Detect"));
                                SFX.Play(GetPath("SFX/ScannerLion.wav"), ((float)PlayerStats.Data.DictorVolume / 100), 0.0f, 0.0f, false);
                                Level.Add(new SubtitlesManager(LanguageManager.GetPhrase("SA"), 180));
                            }
                            if (target.profile.localPlayer)
                            {
                                SFX.Play(GetPath("SFX/ScannedByLion.wav"), ((float)PlayerStats.Data.DictorVolume/100), 0.0f, 0.0f, false);
                                Level.Add(new SubtitlesManager(LanguageManager.GetPhrase("S"), 160));
                            }
                        }
                    }
                }
            }
        }
        public virtual void LanternActivated()
        {
            if (isServerForObject)
            {
                Level.Add(new DetectorNode(position.x, position.y) { main = this, mainer = theduck});
            }
        }
        public override void Update()
        {
            if (theduck != null)
            {
                if (theduck.profile.localPlayer)
                {
                    foreach (Holdable h in Level.current.things[typeof(Holdable)])
                    {
                        if (h is HWSpear || h is HWSledgeHammer || h is HWChainsaw)
                        {
                            h.canPickUp = true;
                        }
                        else
                        {
                            h.canPickUp = false;
                        }
                    }
                }
            }
            if (duration > 0f)
            {
                targetMode = 1.5f;
                duration -= 0.01666666f;
                if(abilityNum == 0)
                {
                    if (duration > 4.98f && duration < 5f)
                    {
                        SFX.Play(GetPath("SFX/Scan.wav"), 1f, 0.0f, 0.0f, false);
                        Level.Add(new Screenshake(5f, 2f * ((float)PlayerStats.Data.screenshake/100)));
                    }
                }
            }

            if (duration <= 0f)
            {
                scan = false;
                targetMode = 1.15f;
            }
            if (reload > 0f)
            {
                reload -= 0.01666666f;
            }

            
            if (scan == true)
            {
                if (abilityNum == 0)
                {
                    ScannerActivated();
                }
                if (abilityNum == 1 && _equippedDuck != null)
                {
                    TrackerActivated();
                }
                if(abilityNum == 2)
                {

                }
            }
            if (_equippedDuck != null)
            {
                _equippedDuck._disarmDisable = 30;
                if (_equippedDuck.inputProfile.Released("QUACK"))
                {
                    if (reload <= 0f && charges > 0)
                    {
                        if (abilityNum == 0)
                        {
                            reload = 8f;
                            duration = 6f;
                            /*PlayerStats.Load();
                            PlayerStats.ScannerActivated += 1;
                            PlayerStats.Save();*/
                            points += 20;
                            if (_equippedDuck.profile.localPlayer)
                            {                              
                                Level.Add(new PointsGetManager(20, "Scan"));
                            }
                            SFX.Play(GetPath("SFX/GetReadyToScan.wav"), 1f, 0.0f, 0.0f, false);
                        }
                        if (abilityNum == 1)
                        {
                            SFX.Play(GetPath("SFX/Tracker.wav"), 1f, 0.0f, 0.0f, false);
                            reload = 8f;
                            bool changed = false;
                            Step step = new Step(position.x, position.y) { time = 0};
                            foreach(Step s in Level.CheckCircleAll<Step>(position, 160f))
                            {
                                if(s.time > step.time)
                                {
                                    step = s;
                                    changed = true;
                                }
                            }
                            if (changed)
                            {
                                Level.Add(new OverlayMarker(step.position.x, step.position.y, 7) { existTime = 1f, outTime = 0.5f, scaleIncrease = 0.1f, alphaIncrease = -0.04f });
                                points += 25;
                                if (_equippedDuck.profile.localPlayer)
                                {
                                    /*if (Halloween.upd.challengeID == 4)
                                    {
                                        if (Halloween.upd.challengeProgression < Halloween.upd.challengeTarget)
                                        {
                                            Halloween.upd.challengeProgression += 1;
                                        }
                                    }*/
                                    Level.Add(new PointsGetManager(25, "Detect"));
                                }

                            }
                            duration = 5f;
                            points += 20;
                            if (_equippedDuck.profile.localPlayer)
                            {
                                
                                Level.Add(new PointsGetManager(20, "Track"));
                            }
                            /*PlayerStats.Load();
                            PlayerStats.TrackerActivated += 1;
                            PlayerStats.Save();*/
                        }
                        if(abilityNum == 2)
                        {
                            foreach (Duck du in Level.current.things[typeof(Duck)])
                            {
                                DinamicSFX.PlayDef(du.position, position, 2f, 60f, "SFX/GhostLantern.wav");
                            }
                            reload = 5f;
                            LanternActivated();
                            duration = 7f;
                            points += 20;
                            if (_equippedDuck.profile.localPlayer)
                            {
                                
                                Level.Add(new PointsGetManager(20, "Lantern"));
                            }
                        }
                        charges--;
                        scan = true;
                    }
                }
                if (_equippedDuck.inputProfile.Pressed("STRAFE") && duration <= 0f)
                {
                    abilityNum++;
                    if (abilityNum > 2)
                    {
                        abilityNum = 0;
                    }
                }
                _equippedDuck.hSpeed *= 0.95f;
                Duck d = Level.Nearest<Duck>(x, y, _equippedDuck);
                if (d != null)
                {
                    if (d.HasEquipment(typeof(Murder)))
                    {
                        d = null;
                    }
                }
                if(d != null)
                {
                    if((d.position - position).length < 64)
                    {
                        intensivity = 4f;
                        if (DetectedByHeartbeat == false)
                        {
                            DetectedByHeartbeat = true;
                            if (_equippedDuck.profile.localPlayer)
                            {
                                SFX.Play(GetPath("SFX/MurdererHeartbeat.wav"), ((float)PlayerStats.Data.DictorVolume / 100), 0.0f, 0.0f, false);
                                Level.Add(new SubtitlesManager(LanguageManager.GetPhrase("HN")));
                            }
                        }
                    }
                    else if ((d.position - position).length < 128)
                    {
                        intensivity = 2.5f;
                    }
                    else if ((d.position - position).length < 196)
                    {
                        intensivity = 1f;
                    }
                    else { intensivity = 0.5f; }
                }
            }
            else
            {
                foreach (Murder m in Level.current.things[typeof(Murder)])
                {
                    if (m.theduck != null)
                    {
                        if (Network.isActive)
                        {
                            pos = m.theduck.position;
                            if (m.theduck.ragdoll != null)
                            {
                                targetMode = 0.8f;
                                pos = m.theduck.ragdoll.position;
                                pos.y += -4;
                            }
                            //DrawVisionCircle();
                        }
                    }
                }
            }
            base.Update();

            if (theduck != null)
            {
                if (theduck.profile.localPlayer)
                {
                    foreach (Holdable h in Level.current.things[typeof(Holdable)])
                    {
                        if (h is HWSpear || h is HWSledgeHammer || h is HWChainsaw)
                        {
                            h.canPickUp = true;
                        }
                        else
                        {
                            h.canPickUp = false;
                        }
                    }
                }
            }
        }

        public void DrawVisionCircle()
        {
            vision.alpha = 1;
            numb = 0.04f + num + _pulse * 0.002f * intensivity;
            thick = numb * CircleSize * 0.5f * mod;
            vision.depth = 0.74f;

            Graphics.Draw(vision, pos.x, pos.y, numb * 26 * mod, numb * 26 * mod);
            Graphics.DrawRect(new Vec2(pos.x + thick - 1, pos.y + thick - 1), new Vec2(pos.x - thick, pos.y + thick + 2000), Color.Black, 0.75f);
            Graphics.DrawRect(new Vec2(pos.x + thick - 1, pos.y - thick + 1), new Vec2(pos.x - thick - 1, pos.y - thick - 2000), Color.Black, 0.75f);
            Graphics.DrawRect(new Vec2(pos.x + thick - 1, pos.y + thick + 2000), new Vec2(pos.x + thick + 2000, pos.y - thick - 2000), Color.Black, 0.75f);
            Graphics.DrawRect(new Vec2(pos.x - thick + 1, pos.y + thick + 2000), new Vec2(pos.x - thick - 2000, pos.y - thick - 2000), Color.Black, 0.75f);
        }

        public override void OnDrawLayer(Layer pLayer)
        {
            if (pLayer == Layer.Foreground)
            {
                if (theduck != null && theduck.profile != null && 
                    (theduck.profile.name != "Player1" && theduck.profile.name != "Player2" && theduck.profile.name != "Player3" && theduck.profile.name != "Player4"))
                {
                    //Lion scan UI
                    if (abilityNum == 0 && duration > 0.05f)
                    {
                        SpriteMap _alert = new SpriteMap(GetPath("Sprites/movementScanerAlert.png"), 32, 32);
                        _alert.CenterOrigin();

                        _alert.alpha = 0.75f;
                        if (duration > 5.1f)
                        {
                            _alert.alpha = 0.4f + _pulse * 0.05f;
                        }

                        Vec2 pos = Level.current.camera.position;
                        Vec2 siz = Level.current.camera.size;

                        _alert.scale = new Vec2(siz.x / 320, siz.x / 320);

                        Graphics.Draw(_alert, pos.x + siz.x / 2, pos.y + siz.y / 2, 1f);
                    }

                    Vec2 posi = Level.current.camera.position;
                    Vec2 Unit = Level.current.camera.size / new Vec2(320, 180);
                    if (theduck != null)
                    {
                        pos = theduck.position;
                        if (theduck.ragdoll != null)
                        {
                            pos = theduck.ragdoll.position;
                            pos.y += -4;
                        }
                        if (theduck.profile.localPlayer)
                        {
                            //Description at the start
                            if (advice > 0 && PlayerStats.ShowHints)
                            {
                                advice--;

                                SpriteMap _swap = new SpriteMap(GetPath("Sprites/adviceH.png"), 320, 180);
                                _swap.CenterOrigin();
                                _swap.scale = new Vec2(Unit.x, Unit.x) * 0.6f;

                                if (advice < 100)
                                {
                                    _swap.alpha = advice / 100;
                                }

                                Graphics.Draw(_swap, posi.x + 160 * Unit.x, posi.y + 90 * Unit.y, 1.02f);
                            }

                            //Health
                            if (health > 0)
                            {
                                _hp.frame = 50 - health;
                                if (bleeding)
                                    _hp.frame = 51;
                            }
                            else
                            {
                                _hp.frame = 50;
                            }
                            _hp.scale = new Vec2(Level.current.camera.size.x / 200 * 0.5625f, Level.current.camera.size.y / 200);
                            _hp.position = new Vec2(posi.x + Unit.x * 30, posi.y + Unit.y * 170);
                            Graphics.Draw(_hp, _hp.position.x, _hp.position.y, 0.76f);

                            //Teammate ping
                            foreach (Murder m in Level.current.things[typeof(Murder)])
                            {
                                if (m != this)
                                {
                                    icons.frame = 0;
                                    if (m.equippedDuck == null)
                                    {
                                        icons.frame = 1;
                                    }
                                    icons.alpha = ((position - m.position).length - 36f * mod) / (80f + 36 * mod);
                                    Graphics.Draw(icons, m.position.x, m.position.y - 4f, 0.755f);
                                }
                            }

                            slots.position = new Vec2(posi.x + 160 * Unit.x, posi.y + 162 * Unit.y);

                            //Keys meaning-icons
                            if (PlayerStats.ShowButtons)
                            {
                                SpriteMap _swap = new SpriteMap(GetPath("Sprites/KeysMeaning.png"), 14, 14);
                                _swap.CenterOrigin();
                                _swap.scale = new Vec2(Unit.x, Unit.x) * 0.4f;
                                Graphics.Draw(_swap, slots.position.x - 18f * Unit.x, slots.position.y + -2 * Unit.y, 1.02f);

                                _swap.CenterOrigin();
                                _swap.frame = 1;
                                _swap.scale = new Vec2(Unit.x, Unit.x) * 0.4f;
                                Graphics.Draw(_swap, slots.position.x + 18 * Unit.x, slots.position.y + -2 * Unit.y, 1.02f);


                                Graphics.DrawString("@STRAFE@", new Vec2(slots.position.x + (-18 - 2) * Unit.x, slots.position.y + -8 * Unit.y), Color.White, 0.79f, null, Unit.x);
                                Graphics.DrawString("@QUACK@", new Vec2(slots.position.x + (18 - 2) * Unit.x, slots.position.y + -8 * Unit.y), Color.White, 0.79f, null, Unit.x);
                            }

                            //amount of uses
                            numer.scale = new Vec2(Level.current.camera.size.x / 256 * 0.5625f, Level.current.camera.size.y / 256);
                            numer.frame = charges;
                            numer.position = slots.position + new Vec2(0f, slots.scale.y / 2 * 32);
                            Graphics.Draw(numer, numer.position.x, numer.position.y, 0.78f);
                       
                            //Selected slot reload
                            if (reload <= 0f)
                            {
                                slots.frame = 0;
                            }
                            else
                            {
                                slots.frame = (int)(24 - reload * 3) + 1;
                            }
                            slots.scale = new Vec2(Level.current.camera.size.x / 256 * 0.5625f, Level.current.camera.size.y / 256);
                            Graphics.Draw(slots, slots.position.x, slots.position.y, 0.76f);
                            //Selected ability
                            slots.frame = 30 + abilityNum;
                            Graphics.Draw(slots, slots.position.x, slots.position.y, 0.77f);

                            Vec2 localHintPos = slots.position + new Vec2(-156 * Unit.x, -14 * Unit.y);
                            if (PlayerStats.ShowHints)
                            {
                                if (theduck.holdObject is HWSledgeHammer)
                                {
                                    string name = LanguageManager.GetPhrase("HAM");
                                    string desc = LanguageManager.GetPhrase("H HAM");

                                    desc = LanguageManager.SplitInLines(desc, 36);

                                    Graphics.DrawStringOutline(name, localHintPos, Color.White, Color.Black, 0.8f, null, slots.scale.x);
                                    Graphics.DrawStringOutline(desc, localHintPos + new Vec2(0, slots.scale.y * 8), Color.White, Color.Black, 0.8f, null, slots.scale.x * 0.3f);
                                }
                                if (theduck.holdObject is HWChainsaw)
                                {
                                    string name = LanguageManager.GetPhrase("CH");
                                    string desc = LanguageManager.GetPhrase("H CH");

                                    desc = LanguageManager.SplitInLines(desc, 36);

                                    Graphics.DrawStringOutline(name, localHintPos, Color.White, Color.Black, 0.8f, null, slots.scale.x);
                                    Graphics.DrawStringOutline(desc, localHintPos + new Vec2(0, slots.scale.y * 8), Color.White, Color.Black, 0.8f, null, slots.scale.x * 0.3f);
                                }
                                if (theduck.holdObject is HWSpear)
                                {
                                    string name = LanguageManager.GetPhrase("SP");
                                    string desc = LanguageManager.GetPhrase("H SP");

                                    desc = LanguageManager.SplitInLines(desc, 36);

                                    Graphics.DrawStringOutline(name, localHintPos, Color.White, Color.Black, 0.8f, null, slots.scale.x);
                                    Graphics.DrawStringOutline(desc, localHintPos + new Vec2(0, slots.scale.y * 8), Color.White, Color.Black, 0.8f, null, slots.scale.x * 0.3f);
                                }
                                localHintPos = slots.position + new Vec2(16 * Unit.x, 4 * Unit.y);
                                if(abilityNum == 0)
                                {
                                    string name = LanguageManager.GetPhrase("SCAN");
                                    string desc = LanguageManager.GetPhrase("HSCAN");

                                    desc = LanguageManager.SplitInLines(desc, 36);

                                    Graphics.DrawStringOutline(name, localHintPos, Color.White, Color.Black, 0.8f, null, slots.scale.x);
                                    Graphics.DrawStringOutline(desc, localHintPos + new Vec2(0, slots.scale.y * 8), Color.White, Color.Black, 0.8f, null, slots.scale.x * 0.3f);
                                }
                                if (abilityNum == 1)
                                {
                                    string name = LanguageManager.GetPhrase("TREK");
                                    string desc = LanguageManager.GetPhrase("HTREK");

                                    desc = LanguageManager.SplitInLines(desc, 36);

                                    Graphics.DrawStringOutline(name, localHintPos, Color.White, Color.Black, 0.8f, null, slots.scale.x);
                                    Graphics.DrawStringOutline(desc, localHintPos + new Vec2(0, slots.scale.y * 8), Color.White, Color.Black, 0.8f, null, slots.scale.x * 0.3f);
                                }
                                if (abilityNum == 2)
                                {
                                    string name = LanguageManager.GetPhrase("LAMP");
                                    string desc = LanguageManager.GetPhrase("HLAMP");

                                    desc = LanguageManager.SplitInLines(desc, 36);

                                    Graphics.DrawStringOutline(name, localHintPos, Color.White, Color.Black, 0.8f, null, slots.scale.x);
                                    Graphics.DrawStringOutline(desc, localHintPos + new Vec2(0, slots.scale.y * 8), Color.White, Color.Black, 0.8f, null, slots.scale.x * 0.3f);
                                }
                            }

                            //Draws vision restriction
                            if (/*Network.isActive*/true)
                            {
                                DrawVisionCircle();
                            }

                        }
                    }
                }
            }
            base.OnDrawLayer(pLayer);
        }
    }
}
