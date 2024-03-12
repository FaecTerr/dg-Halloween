using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    [EditorGroup("Halloween")]
    public class GM_Event : Thing
    {
        private SpriteMap _sprite;
        public EditorProperty<float> RoundTime;
        public EditorProperty<float> PreparationTime;
        public EditorProperty<int> Location;
        public string Name = "";
        public float time = 70f;
        public float Prep = 25f;
        public float Visor = 0f;
        public bool ctWins = false;
        public bool tWins = false;
        public string _string;
        public bool init = false;
        public string str;
        public StateBinding _time = new StateBinding("time", -1, false, false);
        public StateBinding _ctWinBinding = new StateBinding("ctWins", -1, false, false);
        public StateBinding _tWinBinding = new StateBinding("tWins", -1, false, false);
        private BitmapFont _font;
        private BitmapFont _bigfont;
        public float TimeFromStart;
        public float TimeFromAction;
        public bool PrePhaseEnded;
        public bool FFSec;
        public bool TTYSec;
        public bool FTEENSec;
        public bool TENSec;
        public bool FiveSec;

        public float specAlpha;
        public bool activateSpectator = true;

        private BitmapFont _fon;

        public int totalPlayers;
        public List<Duck> Players = new List<Duck>();
        public int selectedPlayer;

        public bool localPlayerIsGhost;
        public bool localPlayerIsMurderer;

        public SpriteMap _spectatorBox;
        public SpriteMap _hpBar;
        public SpriteMap _item;
        public SpriteMap _num;
        public Sprite vision;

        public GM_Event(float xval, float yval) : base(xval, yval)
        {
            vision = new Sprite(GetPath("Sprites/MurderCircle"));
            vision.CenterOrigin();
            _fon = new BitmapFont("biosFont", 8, -1);
            _bigfont = new BitmapFont("biosFont", 8, -1);
            _spectatorBox = new SpriteMap(GetPath("Sprites/SpectatorBox.png"), 107, 40);
            _hpBar = new SpriteMap(GetPath("Sprites/HealthBar.png"), 64, 20);
            _item = new SpriteMap(GetPath("Sprites/Item.png"), 32, 32);
            _num = new SpriteMap(GetPath("Sprites/ItemCounter.png"), 14, 14);
            _font = new BitmapFont("biosFont", 8, -1);
            _sprite = new SpriteMap(Mod.GetPath<Halloween>("Sprites/GameMode.png"), 16, 16, false);
            base.graphic = _sprite;
            center = new Vec2(8f, 8f);
            collisionOffset = new Vec2(-8f, -8f);
            collisionSize = new Vec2(15f, 15f);
            graphic = _sprite;
            layer = Layer.Foreground;
            depth = 0.9f;
            _string = Convert.ToString(time);
            RoundTime = new EditorProperty<float>(70f, this, 20f, 180f, 1f, null);
            PreparationTime = new EditorProperty<float>(25f, this, 0f, 60f, 1f, null);
            Location = new EditorProperty<int>(0, null, 0, 15, 1);
        }

        public void TerroristWin()
        {
            foreach (CTEquipment cte in Level.current.things[typeof(CTEquipment)])
            {
                if (cte != null)
                {
                    foreach (Duck d in Level.current.things[typeof(Duck)])
                    {
                        if (d != null)
                        {
                            if (d._equipment != null && d.HasEquipment(cte) == true)
                            {
                                d.Kill(new DTImpact(this));
                            }
                        }
                    }
                }
            }
            foreach(TEquipment te in Level.current.things[typeof(TEquipment)])
            {
                int pumps = 0;
                int totalpumps = 0;
                foreach (Pumpkin p in Level.current.things[typeof(Pumpkin)])
                {
                    totalpumps++;
                    if (p.actuated)
                    {
                        pumps++;
                    }
                }
                if (pumps > 0 && pumps == totalpumps)
                {
                    if (PlayerStats.ChallengeID == 14)
                    {
                        PlayerStats.ChallengeProgress++;
                        PlayerStats.Save();
                    }
                }
            }
        }
        public void CounterTerroristWin()
        {
            foreach (CTEquipment cte in Level.current.things[typeof(CTEquipment)])
            {
                if (cte != null)
                {
                    foreach (Duck d in Level.current.things[typeof(Duck)])
                    {
                        if (d != null)
                        {
                            if (d._equipment != null && d.HasEquipment(cte) == true)
                            {
                                if (d.profile.localPlayer)
                                {
                                    /*if (Halloween.upd.challengeID == 6)
                                    {
                                        if (Halloween.upd.challengeProgression < Halloween.upd.challengeTarget)
                                        {
                                            Halloween.upd.challengeProgression += 1;
                                        }
                                    }*/
                                    int pumps = 0;
                                    int totalpumps = 0;
                                    foreach(Pumpkin p in Level.current.things[typeof(Pumpkin)])
                                    {
                                        totalpumps++;
                                        if (p.actuated)
                                        {
                                            pumps++;
                                        }
                                    }
                                    if(pumps > 0 && pumps == totalpumps)
                                    {
                                        if(PlayerStats.ChallengeID == 14)
                                        {
                                            PlayerStats.ChallengeProgress++;
                                            PlayerStats.Save();
                                        }
                                    }
                                    Level.Add(new PointsGetManager(125, "Time"));
                                    cte.points += 125;
                                }
                            }
                        }
                    }
                }
            }
            foreach (TEquipment te in Level.current.things[typeof(TEquipment)])
            {
                if (te != null)
                {
                    foreach (Duck d in Level.current.things[typeof(Duck)])
                    {
                        if (d != null)
                        {
                            if (d._equipment != null && d.HasEquipment(te) == true)
                            {
                                d.Kill(new DTImpact(this));
                            }
                        }
                    }
                }
            }
        }
        public void Init()
        {
            foreach(Holdable h in Level.current.things[typeof(Holdable)])
            {
                if(h is Device)
                {
                    h.canPickUp = false;
                }
            }
            foreach (TreeTop t in Level.current.things[typeof(TreeTop)])
            {
                Level.Add(new ThingTop(t.position.x, t.position.y));
                Level.Remove(t);
            }
            foreach (Duck d in Level.current.things[typeof(Duck)])
            {
                if (d.profile.localPlayer)
                {
                    if (d.HasEquipment(typeof(Ghost)))
                    {
                        localPlayerIsGhost = true;
                    }
                    if (d.HasEquipment(typeof(Murder)))
                    {
                        localPlayerIsMurderer = true;
                    }
                }

                if (!(Players.Contains(d)) && !(d.profile.spectator))
                {
                    Players.Add(d);
                    totalPlayers++;
                }

            }
            foreach (WaterFall f in Level.current.things[typeof(WaterFall)])
            {
                f.depth = 0.5f;
            }
            foreach (WaterFallEdgeTop f in Level.current.things[typeof(WaterFallEdgeTop)])
            {
                f.depth = 0.5f;
            }
            foreach (WaterFallEdge f in Level.current.things[typeof(WaterFallEdge)])
            {
                f.depth = 0.5f;
            }
            foreach (WaterFallTile f in Level.current.things[typeof(WaterFallTile)])
            {
                f.depth = 0.5f;
            }
        }
        public override void Update()
        {
            base.Update();
            TimeFromStart += 0.01666666f;
            if (TimeFromStart >= PreparationTime && PrePhaseEnded == false)
            {
                PrePhaseEnded = true;
                SFX.Play(GetPath("SFX/PrePhaseEnded.wav"), 1f * ((float)PlayerStats.DictorVolume / 100), 0.0f, 0.0f, false);
                Level.Add(new SubtitlesManager(LanguageManager.GetPhrase("PPE"), 270));
            }

            if (init == false)
            {
                Init();
                if(Location == 1)
                {
                    Name = LanguageManager.GetPhrase("Coastline");
                }
                if (Location == 2)
                {
                    Name = LanguageManager.GetPhrase("Hospital");
                }
                if (Location == 3)
                {
                    Name = LanguageManager.GetPhrase("Prison");
                }
                if (Location == 4)
                {
                    Name = LanguageManager.GetPhrase("Vault tec.");
                }
                if (Location == 5)
                {
                    Name = LanguageManager.GetPhrase("Westwood");
                }
                if (Location == 6)
                {
                    Name = LanguageManager.GetPhrase("Mansion");
                }
                if (Location == 7)
                {
                    Name = LanguageManager.GetPhrase("Favela");
                }
                if (Location == 8)
                {
                    Name = LanguageManager.GetPhrase("Night city");
                }
                if (Location == 9)
                {
                    Name = LanguageManager.GetPhrase("Consulate");
                }
                if(Location == 10)
                {
                    Name = LanguageManager.GetPhrase("Factory");
                }
                if (Location == 11)
                {
                    Name = LanguageManager.GetPhrase("Forest");
                }
                if (Location == 12)
                {
                    Name = LanguageManager.GetPhrase("Arcade");
                }
                if (Location == 13)
                {
                    Name = LanguageManager.GetPhrase("Arctic");
                }
                init = true;
                time = RoundTime;
                Prep = PreparationTime;
                Level.Add(new HLWN());
                if(!(Level.current is Editor))
                {
                    _sprite.frame = 1;
                }
            }
            if (Prep > 0)
            {
                Prep -= 0.01666666f;
            }
            else
            {
                if (time > 0)
                {
                    time -= 0.01666666f;
                    TimeFromAction += 0.01666666f;
                }
            }
            if (time < 46)
            {
                if (FFSec == false)
                {
                    FFSec = true;
                    SFX.Play(GetPath("SFX/45Sec.wav"), 1f * ((float)PlayerStats.DictorVolume / 100), 0.0f, 0.0f, false);
                    Level.Add(new SubtitlesManager(LanguageManager.GetPhrase("R45")));
                }
                if (time < 31)
                {
                    if (TTYSec == false)
                    {
                        TTYSec = true;
                        SFX.Play(GetPath("SFX/30Sec.wav"), 1f * ((float)PlayerStats.DictorVolume / 100), 0.0f, 0.0f, false);
                        Level.Add(new SubtitlesManager(LanguageManager.GetPhrase("R30")));
                    }
                    if (time < 16)
                    {
                        if (FTEENSec == false)
                        {
                            FTEENSec = true;
                            SFX.Play(GetPath("SFX/15Sec.wav"), 1f * ((float)PlayerStats.DictorVolume / 100), 0.0f, 0.0f, false);
                            Level.Add(new SubtitlesManager(LanguageManager.GetPhrase("R15")));
                        }
                        if (time < 12)
                        {
                            if (FFSec == false)
                            {
                                FFSec = true;
                                SFX.Play(GetPath("SFX/10Sec.wav"), 1f * ((float)PlayerStats.DictorVolume / 100), 0.0f, 0.0f, false);
                                Level.Add(new SubtitlesManager("10...  9...  8..."));
                            }
                            if (time < 7)
                            {
                                if (FiveSec == false)
                                {
                                    FiveSec = true;
                                    SFX.Play(GetPath("SFX/5Sec.wav"), 1f * ((float)PlayerStats.DictorVolume / 100), 0.0f, 0.0f, false);
                                    Level.Add(new SubtitlesManager("5...  4...  3..."));
                                }
                            }
                        }
                    }
                }
            }
            int ghosts = 0;
            foreach(Ghost g in Level.current.things[typeof(Ghost)])
            {
                if(g._equippedDuck != null)
                {
                    ghosts += 1;
                }
            }
            if(ghosts == 0 && time <= 75)
            {
                if (tWins == false && ctWins == false)
                {
                    SFX.Play(GetPath("SFX/NoGhostRemaining.wav"), 1f * ((float)PlayerStats.DictorVolume / 100), 0.0f, 0.0f, false);
                    Level.Add(new SubtitlesManager(LanguageManager.GetPhrase("HW"), 150));
                    TerroristWin();
                    time = 0f;
                }
                tWins = true;
            }
            if (time <= 0f)
            {
                if (ctWins == false && tWins == false)
                {
                    SFX.Play(GetPath("SFX/TimeIsUp.wav"), 1f * ((float)PlayerStats.DictorVolume / 100), 0.0f, 0.0f, false);
                    Level.Add(new SubtitlesManager(LanguageManager.GetPhrase("TO"), 150));
                    CounterTerroristWin();
                }
                ctWins = true;
            }
        }

        public void DrawTimer()
        {
            float localTime = 0;
            if(Prep > 0)
            {
                localTime = Prep;
            }
            else
            {
                localTime = time;
            }
            Vec2 pos = new Vec2(Level.current.camera.position.x + Level.current.camera.width / 2, Level.current.camera.position.y + Level.current.camera.height * 13 / 256);
            _font.scale = new Vec2(Level.current.camera.size.x / 256 * 0.5625f, Level.current.camera.size.y / 256);
            int TimeInInt = (int)localTime;
            int mins = 0;
            for (int i = 0; TimeInInt > 59; i++)
            {
                mins += 1;
                TimeInInt -= 60;
            }
            string text = "";
            if (TimeInInt > 9)
                text = Convert.ToString(mins) + ":" + Convert.ToString(TimeInInt);
            else if (TimeInInt < 10)
                text = Convert.ToString(mins) + ":0" + Convert.ToString(TimeInInt);
            float xposit = pos.x - _font.GetWidth(text, false, null) / 2f;
            Color c = Color.White;
            if (TimeInInt % 2 == 1 && TimeInInt < 11 && mins == 0)
                c = Color.Red;
            _font.DrawOutline(text, new Vec2(xposit - 1f, pos.y - 1f), c, Color.Black, 0.9f);

        }
        public void DrawPhase()
        {
            if (Prep > 0)
            {
                str = LanguageManager.GetPhrase("Prep");
            }
            else
            {
                str = LanguageManager.GetPhrase("Act");
            }
            if (str != null && str != "")
            {
                BitmapFont p = new BitmapFont("biosFont", 8, -1);
                Vec2 pos = new Vec2(Level.current.camera.position.x + Level.current.camera.width / 2, Level.current.camera.position.y + Level.current.camera.height * 5 / 256);
                p.scale = new Vec2(Level.current.camera.size.x / 256 * 0.5625f, Level.current.camera.size.y / 256);
                string text = "";
                text = str;
                Color col = Color.Orange;
                float xpos = pos.x - _font.GetWidth(text, false, null) / 2f;
                p.DrawOutline(text, new Vec2(xpos - 1f, pos.y - 1f), col, Color.Black, 0.9f);
            }
            if (TimeFromStart < 4)
            {
                BitmapFont p = new BitmapFont("biosFont", 8, -1);
                string text = "";
                p.alpha = 4 - TimeFromStart;
                Vec2 pos = new Vec2(Level.current.camera.position.x + Level.current.camera.width / 2, Level.current.camera.position.y + Level.current.camera.height / 2);
                p.scale = new Vec2(Level.current.camera.size.x / 64 * 0.5625f, Level.current.camera.size.y / 64);
                str = Name;
                if (str != null && str != "")
                {
                    text = str;
                    Color col = Color.White;
                    float xpos = pos.x - p.GetWidth(text, false, null) / 2f;
                    p.DrawOutline(text, new Vec2(xpos, pos.y), col, Color.Black, 0.9f);
                }
            }
            if(TimeFromAction < 5f && PrePhaseEnded)
            {
                BitmapFont p = new BitmapFont("biosFont", 8, -1);
                Vec2 pos = new Vec2(Level.current.camera.position.x + Level.current.camera.width / 2, Level.current.camera.position.y + Level.current.camera.height * 5 / 32);
                p.scale = new Vec2(Level.current.camera.size.x / 128 * 0.5625f, Level.current.camera.size.y / 128);
                Color col = Color.White;      

                string text = "";
                if (localPlayerIsGhost)
                {
                    text = "Survive";
                }
                if (localPlayerIsMurderer)
                {
                    text = "Kill the ghosts";
                }
                float xpos = pos.x - p.GetWidth(text, false, null) / 2f;
                p.DrawOutline(text, new Vec2(xpos - 1f, pos.y - 1f), col, Color.Black, 0.9f);
            }
            if(Prep < 10f && !PrePhaseEnded)
            {
                BitmapFont p = new BitmapFont("biosFont", 8, -1);
                Vec2 pos = new Vec2(Level.current.camera.position.x + Level.current.camera.width / 2, Level.current.camera.position.y + Level.current.camera.height * 5 / 32);
                p.scale = new Vec2(Level.current.camera.size.x / 128 * 0.5625f, Level.current.camera.size.y / 128);
                Color col = Color.White;

                string text = Convert.ToString((int)Prep);
               
                float xpos = pos.x - _font.GetWidth(text, false, null) / 2f;
                p.DrawOutline(text, new Vec2(xpos - 1f, pos.y - 1f), col, Color.Black, 0.9f);
            }
        }
        public void DrawSpectator()
        {
            int camSmollness = 600;
            Vec2 camSize = new Vec2(Level.current.camera.width, Level.current.camera.width);
            Vec2 realCamSize = new Vec2(Level.current.camera.width, Level.current.camera.height);
            Vec2 camPos = new Vec2(Level.current.camera.position.x, Level.current.camera.position.y);

            int mCount = 0;

            foreach (Murder m in Level.current.things[typeof(Murder)])
            {
                string name = "";
                if (m.theduck != null)
                {
                    if (m.theduck.profile != null)
                    {
                        if (m.theduck.profile.name != null)
                        {
                            name = m.theduck.profile.name;
                        }
                    }
                }


                if (m.reload <= 0f)
                {
                    _item.frame = 0;
                }
                else
                {
                    _item.frame = (int)(24 - m.reload * 3) + 1;
                }
                _spectatorBox.frame = 0;

                if (m.health > 0)
                {
                    _hpBar.frame = 50 - m.health;
                }
                else
                {
                    _hpBar.frame = 50;
                }

                _item.scale = camSize / camSmollness;
                _spectatorBox.scale = camSize / camSmollness;
                _hpBar.scale = camSize / camSmollness;
                _num.scale = camSize / camSmollness;
                _fon.scale = camSize / (camSmollness * 2.4f);

                Graphics.Draw(_hpBar, camPos.x, camPos.y + (1 + mCount * 5) / 32 * camSize.y + _spectatorBox.scale.y * 22 + _spectatorBox.scale.y * 42f * mCount, 0.9f);
                Graphics.Draw(_item, camPos.x + _spectatorBox.scale.x * 71, camPos.y + (1 + mCount * 5) / 32 * realCamSize.y + _spectatorBox.scale.y*4 + _spectatorBox.scale.y * 42f * mCount, 0.85f);
                Graphics.Draw(_spectatorBox, camPos.x, camPos.y + (1 + mCount * 5) / 32 * camSize.y + _spectatorBox.scale.y * 42f * mCount, 0.86f);

                _item.frame = 30 + m.abilityNum;

                Graphics.Draw(_item, camPos.x + _spectatorBox.scale.x * 71, camPos.y + (1 + mCount * 5) / 32 * realCamSize.y + _spectatorBox.scale.y * 4 + _spectatorBox.scale.y * 42f * mCount, 0.87f);

                _fon.Draw(name, camPos.x + _spectatorBox.scale.x * 6, camPos.y + (1 + mCount * 5) / 32 * realCamSize.y + _spectatorBox.scale.y * 6 + _spectatorBox.scale.y * 42f * mCount, Color.Black, 0.9f);

                _num.frame = m.charges;
                Graphics.Draw(_num, camPos.x + _spectatorBox.scale.x * 52, camPos.y + (1 + mCount * 5) / 32 * realCamSize.y + _spectatorBox.scale.y * 10 + _spectatorBox.scale.y * 42f * mCount, 0.88f);

                mCount++;
            }
            int gCount = 0;
            foreach (Ghost g in Level.current.things[typeof(Ghost)])
            {
                string name = "";
                if (g.theduck != null)
                {
                    if (g.theduck.profile != null)
                    {
                        if (g.theduck.profile.name != null)
                        {
                            name = g.theduck.profile.name;
                        }
                    }
                }

                _spectatorBox.frame = 1;
                if (g.itemCooldown <= 0f)
                {
                    _item.frame = 0;
                }
                else
                {
                    _item.frame = (int)(24 - g.itemCooldown * 2) + 1;
                }

                _item.scale = camSize / camSmollness;
                _spectatorBox.scale = camSize / camSmollness;
                _num.scale = camSize / camSmollness;
                _fon.scale = camSize / (camSmollness * 2.4f);

                Graphics.Draw(_item, camPos.x + realCamSize.x - _spectatorBox.scale.x * 105, camPos.y + (1 + gCount * 5) / 32 * realCamSize.y + _spectatorBox.scale.y * 4 + _spectatorBox.scale.y * 42f * gCount, 0.85f);

                if (g.reload <= 0f)
                {
                    _item.frame = 0;
                }
                else
                {
                    _item.frame = (int)(24 - g.reload * 2.4) + 1;
                }

                Graphics.Draw(_item, camPos.x + realCamSize.x - _spectatorBox.scale.x * 73, camPos.y + (1 + gCount * 5) / 32 * realCamSize.y + _spectatorBox.scale.y * 4 + _spectatorBox.scale.y * 42f * gCount, 0.85f);
                Graphics.Draw(_spectatorBox, camPos.x + realCamSize.x  - _spectatorBox.scale.x * 107, camPos.y + (1 + gCount * 5) / 32 * realCamSize.y + _spectatorBox.scale.y * 42f * gCount, 0.86f);

                _item.frame = 25 + g.itemNum;

                Graphics.Draw(_item, camPos.x + realCamSize.x - _spectatorBox.scale.x * 105, camPos.y + (1 + gCount * 5) / 32 * realCamSize.y + _spectatorBox.scale.y * 4 + _spectatorBox.scale.y*42f*gCount, 0.87f);

                _item.frame = 35;

                Graphics.Draw(_item, camPos.x + realCamSize.x - _spectatorBox.scale.x * 73, camPos.y + (1 + gCount * 5) / 32 * realCamSize.y + _spectatorBox.scale.y * 4 + _spectatorBox.scale.y * 42f * gCount, 0.87f);

                _fon.Draw(name, camPos.x + realCamSize.x - _spectatorBox.scale.x * 35, camPos.y + (1 + gCount * 5) / 32 * realCamSize.y + _spectatorBox.scale.y * 6 + _spectatorBox.scale.y * 42f * gCount, Color.LightBlue, 0.9f);

                _num.frame = g.storage;
                Graphics.Draw(_num, camPos.x + realCamSize.x - _spectatorBox.scale.x * 36, camPos.y + (1 + gCount * 5) / 32 * realCamSize.y + _spectatorBox.scale.y * 20 + _spectatorBox.scale.y * 42f * gCount, 0.88f);
                _num.frame = g.charges;
                Graphics.Draw(_num, camPos.x + realCamSize.x - _spectatorBox.scale.x * 19, camPos.y + (1 + gCount * 5) / 32 * realCamSize.y + _spectatorBox.scale.y * 20 + _spectatorBox.scale.y * 42f * gCount, 0.88f);

                gCount++;
            }
        }

        public void DrawVisionCircle()
        {
            if (selectedPlayer > 0)
            {
                Init();
                Duck d = Players[selectedPlayer - 1] as Duck;
                if (d != null)
                {
                    if (d.HasEquipment(typeof(Murder)))
                    {
                        Murder m = d.GetEquipment(typeof(Murder)) as Murder;
                        m.DrawVisionCircle();
                        /*vision.depth = 0.75f;
                        vision.alpha = 1.10f - m.num;
                        m.numb = m.num + m._pulse * 0.002f * m.intensivity;
                        m.thick = m.numb * m.mod * 1800 * 0.5f;
                        Graphics.Draw(vision, m.pos.x, m.pos.y, m.numb * 20f * m.mod, m.numb * 20f * m.mod);
                        Graphics.DrawRect(new Vec2(m.pos.x + m.thick - 1, m.pos.y + m.thick - 1), new Vec2(m.pos.x - m.thick, m.pos.y + m.thick + 2000), Color.Black, 0.75f);
                        Graphics.DrawRect(new Vec2(m.pos.x + m.thick - 1, m.pos.y - m.thick + 1), new Vec2(m.pos.x - m.thick - 1, m.pos.y - m.thick - 2000), Color.Black, 0.75f);
                        Graphics.DrawRect(new Vec2(m.pos.x + m.thick - 1, m.pos.y + m.thick + 2000), new Vec2(m.pos.x + m.thick + 2000, m.pos.y - m.thick - 2000), Color.Black, 0.75f);
                        Graphics.DrawRect(new Vec2(m.pos.x - m.thick + 1, m.pos.y + m.thick + 2000), new Vec2(m.pos.x - m.thick - 2000, m.pos.y - m.thick - 2000), Color.Black, 0.75f);*/
                    }
                    if (d.HasEquipment(typeof(Ghost)))
                    {
                        Ghost g = d.GetEquipment(typeof(Ghost)) as Ghost;
                        g.DrawVisionCircle();
                        /*vision.depth = 0.75f;
                        vision.alpha = 1.10f - g.num;
                        g.numb = g.num + g._pulse * 0.002f * g.intensivity;
                        g.thick = g.numb * 2700 * 0.5f * g.mod;
                        Graphics.Draw(vision, g.pos.x, g.pos.y, g.numb * 30f, g.numb * 30f);
                        Graphics.DrawRect(new Vec2(g.pos.x + g.thick - 1, g.pos.y + g.thick - 1), new Vec2(g.pos.x - g.thick, g.pos.y + g.thick + 1000), Color.Black, 0.75f);
                        Graphics.DrawRect(new Vec2(g.pos.x + g.thick - 1, g.pos.y - g.thick + 1), new Vec2(g.pos.x - g.thick - 1, g.pos.y - g.thick - 1000), Color.Black, 0.75f);
                        Graphics.DrawRect(new Vec2(g.pos.x + g.thick - 1, g.pos.y + g.thick + 1000), new Vec2(g.pos.x + g.thick + 1000, g.pos.y - g.thick - 1000), Color.Black, 0.75f);
                        Graphics.DrawRect(new Vec2(g.pos.x - g.thick + 1, g.pos.y + g.thick + 1000), new Vec2(g.pos.x - g.thick - 1000, g.pos.y - g.thick - 1000), Color.Black, 0.75f);*/
                    }
                    if(d.profile != null)
                    {
                        Vec2 camSize = new Vec2(Level.current.camera.width, Level.current.camera.width);
                        Vec2 realCamSize = new Vec2(Level.current.camera.width, Level.current.camera.height);
                        Vec2 camPos = new Vec2(Level.current.camera.position.x, Level.current.camera.position.y);
                        _bigfont.scale = camSize/400;
                        string name = d.profile.name;
                        if (d.profile.name.Length > 9)
                        {
                            name = d.profile.name.Substring(0, 9);
                        }
                        string str = "< Spectating: " + d.profile.name + " >";
                        Vec2 posit = new Vec2(camPos.x + realCamSize.x*1/2 - _bigfont.scale.x*str.Length*4, camPos.y + realCamSize.y*11/13);
                        _bigfont.Draw(str, posit, Color.White, 0.88f);
                    }
                }
            }
        }

        public void DrawPoints()
        {
            SpriteMap _points = new SpriteMap(GetPath("Sprites/Loading.png"), 14, 14);
            BitmapFont p = new BitmapFont("biosFont", 8, -1);

            int camSmollness = 480;
            Vec2 camSize = new Vec2(Level.current.camera.width, Level.current.camera.width);
            Vec2 realCamSize = new Vec2(Level.current.camera.width, Level.current.camera.height);
            Vec2 camPos = new Vec2(Level.current.camera.position.x, Level.current.camera.position.y);

            _points.scale = camSize / (camSmollness/2);
            p.scale = camSize / (camSmollness * 2.5f);

            foreach (Fuse_armor f in Level.current.things[typeof(Fuse_armor)])
            {
                
                if (f._equippedDuck != null)
                {
                    if (f._equippedDuck.profile.localPlayer)
                    {
                        SpriteMap _rank = new SpriteMap(GetPath("Sprites/RankIcons.png"), 16, 16);
                        _rank.frame = (int)Halloween.upd.ELO/100;
                        _rank.scale = new Vec2(camSize / (camSmollness));
                        string txt = "ELO: " + Convert.ToString(Halloween.upd.ELO);

                        //p.DrawOutline(txt, new Vec2(camPos.x + camSize.x - _points.scale.x * 28f, camPos.y + (camSize.y / camSmollness) + _points.scale.y * 14f), Color.White, Color.Black, 0.8f);
                        //Graphics.Draw(_rank, camPos.x - _points.scale.x * 26 + camSize.x, camPos.y + _points.scale.y * 2 + (camSize.y / camSmollness), 0.8f);

                        if (!f.coinGet)
                        {
                            _points.frame = 8;
                            _points.alpha = 0.1f;
                            string text = Convert.ToString(f.points);
                            Graphics.Draw(_points, camPos.x + _points.scale.x * 2 + camSize.x * 3 / camSmollness, camPos.y + _points.scale.y * 3 + (camSize.y / camSmollness), 0.79f);

                            _points.alpha = 0.9f;
                            _points.frame = f.points / (150 / 9);
                            Graphics.Draw(_points, camPos.x + _points.scale.x * 2 + camSize.x * 3 / camSmollness, camPos.y + _points.scale.y * 3 + (camSize.y / camSmollness), 0.8f);

                            p.DrawOutline(text, new Vec2(camPos.x + camSize.x * 3 / camSmollness + _points.scale.x * 9f - _font.scale.x * 2 * text.Length, camPos.y + (camSize.y / camSmollness) + _points.scale.y * 9.5f), Color.White, Color.Black, 0.8f);
                        }
                        else
                        {

                            SpriteMap _coin = new SpriteMap(GetPath("Sprites/coin.png"), 22, 22);
                            _coin.scale = new Vec2(camSize / (camSmollness));
                            _coin.center = new Vec2(11f, 11f);
                            string text = "+ 50$";
                            if (PlayerStats.Data.ELO > 900)
                            {
                                text = "+ 60$";
                            }
                            if (PlayerStats.Data.ELO > 1300)
                            {
                                _coin.frame = 0;
                                Graphics.Draw(_coin, camPos.x + _points.scale.x * 12 + camSize.x * 3 / camSmollness, camPos.y + _points.scale.y * 15 + (camSize.y / camSmollness), 0.8f);
                            }
                            if (PlayerStats.Data.ELO > 1700)
                            {
                                _coin.frame = 1;
                                Graphics.Draw(_coin, camPos.x + _points.scale.x * 16 + camSize.x * 3 / camSmollness, camPos.y + _points.scale.y * 15 + (camSize.y / camSmollness), 0.81f);
                            }

                            _coin.frame = 0;
                            p.DrawOutline(text, new Vec2(camPos.x + camSize.x * 3 / camSmollness + _points.scale.x * 3f, camPos.y + (camSize.y / camSmollness) + _points.scale.y * 24f), Color.White, Color.Black, 0.8f);
                            Graphics.Draw(_coin, camPos.x + _points.scale.x * 8 + camSize.x * 3 / camSmollness, camPos.y + _points.scale.y * 15 + (camSize.y / camSmollness), 0.79f);
                        }
                    }
                }
            }
        }

        public override void Draw()
        {
            base.Draw();
            bool isSpectator = false;
            foreach (Profile p in Profiles.active)
            {
                if (p.localPlayer)
                {
                    Visor -= 0.01f;
                    if (Visor < 0)
                    {
                        Visor = 0f;
                    }
                    if (p.spectator)
                    {
                        isSpectator = true;
                        if (p.inputProfile.Down("UP"))
                        {
                            Visor += 0.1f;
                        }

                        foreach (Profile profile in Profiles.active)
                        {
                            if (profile.duck == null)
                            {
                                return;
                            }
                            if (Visor > 3)
                            {
                                Visor = 3f;
                            }
                            Duck duck = profile.duck;
                            BitmapFont font = profile.font;
                            string name = profile.name;
                            font.scale = Vec2.One;
                            font.alpha = Visor;
                            Vec2 pos = new Vec2(duck.position.x - font.GetWidth(name, false, null) / 2f, duck.top - 16f);
                            font.DrawOutline(name, pos, profile.persona.colorUsable, Color.Black, 1f);
                        }
                        
                        if (p.inputProfile.Pressed("STRAFE"))
                        {
                            activateSpectator = !activateSpectator;
                        }
                        if (p.inputProfile.Pressed("LEFT"))
                        {
                            selectedPlayer--;
                            if (selectedPlayer < 0)
                            {
                                selectedPlayer = totalPlayers;
                            }
                        }
                        if (p.inputProfile.Pressed("RIGHT"))
                        {
                            selectedPlayer++;
                            if (selectedPlayer > totalPlayers)
                            {
                                selectedPlayer = 0;
                            }
                        }
                        if (p.inputProfile.Pressed("QUACK"))
                        {
                            selectedPlayer = 0;
                        }

                        if (selectedPlayer != 0)
                        {
                            DrawVisionCircle();
                        }
                        if (!activateSpectator)
                        {
                            if (specAlpha > 0f)
                            {
                                specAlpha -= 0.03f;
                            }
                            else
                            {
                                specAlpha = 0;
                            }
                        }
                        if (activateSpectator)
                        {
                            if (specAlpha < 1f)
                            {
                                specAlpha += 0.03f;
                            }
                            else
                            {
                                specAlpha = 1;
                            }
                        }
                        if (p.duck.dead)
                        {
                            _hpBar.alpha = specAlpha / 2;
                            _spectatorBox.alpha = specAlpha / 2;
                            _item.alpha = specAlpha / 2;
                            _fon.alpha = specAlpha / 2;
                            _num.alpha = specAlpha / 2;
                        }
                        else
                        {
                            _hpBar.alpha = specAlpha;
                            _spectatorBox.alpha = specAlpha;
                            _item.alpha = specAlpha;
                            _fon.alpha = specAlpha;
                            _num.alpha = specAlpha;
                        }
                        DrawSpectator();
                    }
                }
                if (PlayerStats.ShowHints && !isSpectator)
                {
                    DrawPoints();
                }
            }
            if (PlayerStats.ShowTimer && !(Level.current is Editor))
            {
                DrawTimer();
                DrawPhase();
            }

        }
    }
}