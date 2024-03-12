using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckGame.Halloween
{
    [BaggedProperty("canSpawn", false)]
    [EditorGroup("Halloween")]
    public class Ghost : CTEquipment
    {
        public SpriteMap numer;
        public SpriteMap slots;
        public SpriteMap icons;
        public Sprite vision;

        public int advice = 300;

        public bool invis = true;
        public int charges = 3; 
        public float reload = 10f;
        public float duration = 0f;
        public int itemNum;
        public int maxStorage = 5;
        public float delay = 12f;
        //Total round length by default is 95 seconds
        public float itemCooldown = 9f; //First - 9 sec, Second - 21 sec, Third - 33 sec, Fourth - 45 sec, Fifth - 57 sec, Sixth - 69 ))), Seventh - 81, Eighth - 93
        public int totalItems;
        public int storage;

        public bool avoidedHunter;

        public List<int> TakenDevices = new List<int>();

        public int CircleSize = 2400;
        
        public Step previStep;
        public int stepFrame;

        public bool usedInvisibility;
        public bool DetectedByHeartbeat;

        public Ghost(float xpos, float ypos) : base(xpos, ypos)
        {
            numer = new SpriteMap(GetPath("Sprites/ItemCounter"), 14, 14);
            numer.CenterOrigin();
            slots = new SpriteMap(GetPath("Sprites/Item"), 32, 32);
            slots.CenterOrigin();
            icons = new SpriteMap(GetPath("Sprites/PlayerIcons"), 18, 18);
            icons.CenterOrigin();
            vision = new Sprite((GetPath("Sprites/MurderCircle")));
            vision.CenterOrigin();
            _pickupSprite = new SpriteMap(GetPath("Sprites/Blank"), 32, 32);
            _sprite = new SpriteMap(GetPath("Sprites/Blank"), 32, 32, false);
            base.graphic = _sprite;
            layer = Layer.Foreground;
            depth = 0.6f;
            _visibleInGame = true;
        }

        public override void loadTextures()
        {
            ReplaceSprite = sprite("Sprites/ghost");
            ReplaceQuack = sprite("Sprites/ghost");
            ReplaceArm = armsprite("Sprites/Blank");
            ReplaceControlled = sprite("Sprites/ghost");
            recolor = true;
        }

        public override void Update()
        {
            //Countdowns
            if (itemCooldown < 0f)
            {
                itemCooldown = 0f;
            }
            if (stepFrames > 0)
            {
                stepFrames--;
            }
            if (duration > 0f)
            {
                duration -= 0.01666666f;
            }
            if (duration <= 0f)
            {
                invis = false;
                avoidedHunter = false;
            }
            if(reload > 0f)
            {
                reload -= 0.01666666f;
            }


            if (_equippedDuck != null)
            {
                if (Level.CheckRect<ThingTop>(topLeft, bottomRight) != null)
                {
                    targetMode = 0.75f;
                }
                else
                {
                    targetMode = 1;
                }
                if (Math.Abs(_equippedDuck.hSpeed) > 0.1f && _equippedDuck.grounded && duration <= 0f) //Duck leaving steps
                {
                    stepFrame--;
                    if (stepFrame <= 0)
                    {
                        stepFrame = 30;
                        Step st = new Step(position.x, position.y) { prevStep = previStep };
                        Level.Add(st);
                        previStep = st;
                    }
                }

                _equippedDuck.alpha = 0.5f - _pulse*0.2f; //Half invisibility for ghosts

                if (_equippedDuck.ragdoll != null) //Half invisibility for ghost ragdolls
                {
                    _equippedDuck.ragdoll.part1.alpha = 0.5f;
                    _equippedDuck.ragdoll.part2.alpha = 0.5f;
                    _equippedDuck.ragdoll.part3.alpha = 0.5f;
                }
                if (_equippedDuck.inputProfile.Pressed("QUACK")) //Using ghost ability - Invisibility
                {
                    if(reload <= 0f && charges > 0)
                    {
                        duration = 8f;
                        reload = 10f;
                        /*PlayerStats.Load();
                        PlayerStats.InvisActivated += 1;
                        PlayerStats.Save();*/
                        if(Level.CheckCircle<Murder>(position, 128f) != null)
                        {
                            avoidedHunter = true;
                        }
                        if (avoidedHunter)
                        {
                            points += 40;
                            if (_equippedDuck.profile.localPlayer)
                            {
                                Level.Add(new PointsGetManager(40, "Invis", "+20 : " + LanguageManager.GetPhrase("PMAH")));
                                /*if (Halloween.upd.challengeID == 7)
                                {
                                    if (Halloween.upd.challengeProgression < Halloween.upd.challengeTarget)
                                    {
                                        Halloween.upd.challengeProgression += 1;
                                    }
                                }*/
                            }
                        }
                        else
                        {
                            points += 20;
                            if (_equippedDuck.profile.localPlayer)
                            {
                                Level.Add(new PointsGetManager(20, "Invis"));
                            }
                        }
                        charges--;
                        invis = true;
                        if(usedInvisibility == false)
                        {
                            usedInvisibility = true;
                        }
                    }
                }

                if (_equippedDuck.holdObject != null)
                {
                    if (_equippedDuck.holdObject is HWChainsaw || _equippedDuck.holdObject is HWSledgeHammer || _equippedDuck.holdObject is HWSpear)
                    {
                        _equippedDuck.doThrow = true;
                    }
                }

                if (_equippedDuck.inputProfile.Pressed("STRAFE")) //Swapping item in inventory
                {
                    itemNum++;
                    if(itemNum > 4)
                    {
                        itemNum = 0;
                    }
                }
                if(TakenDevices.Count >= 5 && theduck.profile.localPlayer)
                {
                    if (TakenDevices.Contains(0) && TakenDevices.Contains(1) && TakenDevices.Contains(2) && TakenDevices.Contains(3) && TakenDevices.Contains(4))
                    {
                        /*if (Halloween.upd.challengeID == 9)
                        {
                            if (Halloween.upd.challengeProgression < Halloween.upd.challengeTarget)
                            {
                                Halloween.upd.challengeProgression += 1;
                            }
                        }*/
                    }
                    int count = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        for(int j = 1; j < 5; j++)
                        {
                            if(TakenDevices[i] == TakenDevices[j])
                            {
                                count += 1;
                            }
                        }
                    }
                    if(count == 5)
                    {
                        /*if (Halloween.upd.challengeID == 5)
                        {
                            if (Halloween.upd.challengeProgression < Halloween.upd.challengeTarget)
                            {
                                Halloween.upd.challengeProgression += 1;
                            }
                        }*/
                    }
                }
                if (_equippedDuck.inputProfile.Pressed("SHOOT") && totalItems <= maxStorage && storage > 0 && _equippedDuck.holdObject == null) //Picking up item from inventory
                {
                    Device dev = null;
                    if(itemNum == 0)
                    {
                        dev = new Claymore(-9999999f, -9999999f) { onlyCT = true };
                    }
                    if (itemNum == 1)
                    {
                        dev = new Grzmot(-9999999f, -9999999f) { onlyCT = true};
                    }
                    if (itemNum == 2)
                    {
                        dev = new Kapkan(-9999999f, -9999999f) { onlyCT = true};
                    }
                    if (itemNum == 3)
                    {
                        dev = new EDD(-9999999f, -9999999f) { onlyCT = true };
                    }
                    if (itemNum == 4)
                    {
                        dev = new GasCanister(-9999999f, -9999999f) { onlyCT = true };
                    }
                    if (dev != null)
                    {
                        Fondle(dev);
                        TakenDevices.Add(itemNum);
                        if (isServerForObject)
                        {
                            Level.Add(dev);
                        }
                        _equippedDuck.GiveHoldable(dev);
                        storage--;
                    }
                }
                if(itemCooldown <= 0f && totalItems < maxStorage) //Delay for items in inventory
                {
                    itemCooldown = delay;
                    storage++;
                    totalItems++;
                }
                else if (totalItems < maxStorage)
                {
                    itemCooldown -= 0.01666666f;
                }
                if(totalItems >= maxStorage)
                {
                    itemCooldown = 0f;
                }
                if (invis == true) //Invisibility makes ghost faster and invisible
                {
                    _equippedDuck.alpha = 0.05f;
                    _equippedDuck.hSpeed *= 0.95f;
                }
                else //Default ghost speed is much lower than regular
                {
                    _equippedDuck.hSpeed *= 0.75f;
                }
                if(_equippedDuck.ragdoll != null)
                {
                    _equippedDuck.ragdoll.alpha = _equippedDuck.alpha;
                }
                intensivity = 1f;
                Duck d = Level.Nearest<Duck>(x, y, _equippedDuck);
                if (d != null)
                {
                    if (d.HasEquipment(typeof(Ghost)))
                    {
                        d = null;
                    }
                }
                if (d != null)
                {
                    if (d.HasEquipment(typeof(Murder)))
                    {
                        if ((d.position - position).length < 64)
                        {
                            intensivity = 4f;
                            if (DetectedByHeartbeat == false)
                            {
                                DetectedByHeartbeat = true;
                                if (_equippedDuck.profile.localPlayer)
                                {
                                    SFX.Play(GetPath("SFX/GhostHeartbeat.wav"), ((float)PlayerStats.Data.DictorVolume / 100), 0.0f, 0.0f, false);
                                    Level.Add(new SubtitlesManager(LanguageManager.GetPhrase("GN")));
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
            }
            else
            {
                foreach (Ghost g in Level.current.things[typeof(Ghost)])
                {
                    if (g.theduck != null)
                    {
                        if (Network.isActive)
                        {
                            pos = g.theduck.position;
                            if (g.theduck.ragdoll != null)
                            {
                                targetMode = 0.8f;
                                pos = g.theduck.ragdoll.position;
                                pos.y += -4;
                            }
                            //DrawVisionCircle();
                        }
                    }
                }
            }
            //Maths.LerpTowards(mod, targetMode, 0.01666666f);
            base.Update();
            if (theduck != null)
            {
                if (theduck.profile.localPlayer && Network.isActive)
                {
                    foreach (Holdable h in Level.current.things[typeof(Holdable)])
                    {
                        if (h is HWSpear || h is HWSledgeHammer || h is HWChainsaw)
                        {
                            //h.canPickUp = false;
                        }
                    }
                }
            }
        }

        public void DrawVisionCircle()
        {
            vision.alpha = 1;
            numb = num + _pulse * 0.002f * intensivity;
            thick = numb * CircleSize * 0.5f * mod;
            vision.depth = 0.74f;

            Graphics.Draw(vision, pos.x, pos.y, numb * 34 * mod, numb * 34 * mod );
            Graphics.DrawRect(new Vec2(pos.x + thick - 1, pos.y + thick - 1), new Vec2(pos.x - thick, pos.y + thick + 2000), Color.Black, 0.75f);
            Graphics.DrawRect(new Vec2(pos.x + thick - 1, pos.y - thick + 1), new Vec2(pos.x - thick - 1, pos.y - thick - 2000), Color.Black, 0.75f);
            Graphics.DrawRect(new Vec2(pos.x + thick - 1, pos.y + thick + 2000), new Vec2(pos.x + thick + 2000, pos.y - thick - 2000), Color.Black, 0.75f);
            Graphics.DrawRect(new Vec2(pos.x - thick + 1, pos.y + thick + 2000), new Vec2(pos.x - thick - 2000, pos.y - thick - 2000), Color.Black, 0.75f);
        }

        public override void OnDrawLayer(Layer pLayer)
        {
            if (theduck != null && theduck.profile != null && 
                (theduck.profile.name != "Player1" && theduck.profile.name != "Player2" && theduck.profile.name != "Player3" && theduck.profile.name != "Player4"))
            {
                if (pLayer == Layer.Foreground)
                {
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
                            if (advice > 0 && PlayerStats.ShowHints)
                            {
                                advice--;

                                SpriteMap _swap = new SpriteMap(GetPath("Sprites/adviceG.png"), 320, 180);
                                _swap.CenterOrigin();
                                _swap.scale = new Vec2(Unit.x, Unit.x) * 0.6f;

                                if (advice < 60)
                                {
                                    _swap.alpha = advice / 60;
                                }

                                Graphics.Draw(_swap, posi.x + 160 * Unit.x, posi.y + 90 * Unit.y, 1.02f);
                            }

                            //Health block
                            if (health > 0)
                            {
                                _hp.frame = 52;
                                if (bleeding)
                                    _hp.frame = 53;
                            }
                            else
                            {
                                _hp.frame = 54;
                            }
                            _hp.scale = new Vec2(Level.current.camera.size.x / 200 * 0.5625f, Level.current.camera.size.y / 200);
                            _hp.position = new Vec2(posi.x + Unit.x * 30, posi.y + Unit.y * 170);
                            Graphics.Draw(_hp, _hp.position.x, _hp.position.y, 0.76f);

                            //Other ghosts marker block
                            foreach (Ghost g in Level.current.things[typeof(Ghost)])
                            {
                                if (g != this)
                                {
                                    icons.frame = 2;
                                    if (g._equippedDuck == null)
                                    {
                                        icons.frame = 3;
                                    }
                                    icons.alpha = ((position - g.position).length - 48f) / 128f;
                                    if (targetMode < 1f)
                                    {
                                        icons.alpha *= (targetMode - 0.75f) / 0.25f;
                                    }
                                    Graphics.Draw(icons, g.position.x, g.position.y - 4f, 0.755f);
                                }
                            }

                            Vec2 trapUIpos = new Vec2(posi.x + Unit.x * 96, posi.y + Unit.y * 162);
                            Vec2 invisUIpos = new Vec2(posi.x + Unit.x * 296, posi.y + Unit.y * 162);
                            //Show buttons block
                            if (PlayerStats.ShowButtons)
                            {
                                SpriteMap _swap = new SpriteMap(GetPath("Sprites/KeysMeaning.png"), 14, 14);
                                _swap.CenterOrigin();
                                _swap.scale = new Vec2(Unit.x, Unit.x) * 0.4f;
                                Graphics.Draw(_swap, trapUIpos.x - 18 * Unit.x, trapUIpos.y - 2 * Unit.y, 1.02f);

                                _swap.CenterOrigin();
                                _swap.frame = 1;
                                _swap.scale = new Vec2(Unit.x, Unit.x) * 0.4f;
                                Graphics.Draw(_swap, trapUIpos.x + 18 * Unit.x, trapUIpos.y - 2 * Unit.y, 1.02f);

                                _swap.CenterOrigin();
                                _swap.frame = 2;
                                _swap.scale = new Vec2(Unit.x, Unit.x) * 0.4f;
                                Graphics.Draw(_swap, invisUIpos.x + -18 * Unit.x, trapUIpos.y + -2 * Unit.y, 1.02f);

                                Graphics.DrawString("@STRAFE@", new Vec2(trapUIpos.x + (-18 - 2) * Unit.x, trapUIpos.y + -8 * Unit.y), Color.White, 0.79f, null, Unit.x);
                                Graphics.DrawString("@SHOOT@", new Vec2(trapUIpos.x + (18 - 2) * Unit.x, trapUIpos.y + -8 * Unit.y), Color.White, 0.79f, null, Unit.x);
                                Graphics.DrawString("@QUACK@", new Vec2(invisUIpos.x + (-18 - 2) * Unit.x, invisUIpos.y + -8 * Unit.y), Color.White, 0.79f, null, Unit.x);
                            }

                            //Items cooldown block
                            if (itemCooldown <= 0f)
                            {
                                slots.frame = 0;
                            }
                            else
                            {
                                slots.frame = (int)(24 - itemCooldown * 2) + 1;
                            }
                            numer.scale = new Vec2(Level.current.camera.size.x / 256 * 0.5625f, Level.current.camera.size.y / 256);
                            slots.scale = new Vec2(Level.current.camera.size.x / 256 * 0.5625f, Level.current.camera.size.y / 256);
                            Graphics.Draw(slots, trapUIpos.x, trapUIpos.y, 0.76f);
                            Vec2 sScale = slots.scale;
                            //Type of device
                            slots.frame = 25 + itemNum;
                            Graphics.Draw(slots, trapUIpos.x, trapUIpos.y, 0.77f);
                            float salp = slots.alpha;
                            slots.scale *= new Vec2(0.5f, 0.5f);
                            for (int i = 1; i < 5; i++)
                            {
                                slots.alpha = 0.7f - 0.1f * i;
                                slots.frame = 25 + (itemNum + i) % 5;
                                Graphics.Draw(slots, trapUIpos.x + -18f * Unit.x, trapUIpos.y + (-2 + 24f * i) * slots.scale.y, 0.76f);
                                slots.scale *= new Vec2(0.8f, 0.8f);
                            }
                            slots.alpha = salp;
                            slots.scale = sScale;

                            Vec2 localHintPos = trapUIpos + new Vec2(16 * Unit.x, 4 * Unit.y);
                            if (PlayerStats.ShowHints)
                            {
                                if (itemNum == 0)
                                {
                                    string name = LanguageManager.GetPhrase("CL");
                                    string desc = LanguageManager.GetPhrase("H CL");

                                    desc = LanguageManager.SplitInLines(desc, 36);

                                    Graphics.DrawStringOutline(name, localHintPos, Color.White, Color.Black, 0.8f, null, slots.scale.x);
                                    Graphics.DrawStringOutline(desc, localHintPos + new Vec2(0, slots.scale.y * 8), Color.White, Color.Black, 0.8f, null, slots.scale.x * 0.3f);
                                }
                                if (itemNum == 1)
                                {
                                    string name = LanguageManager.GetPhrase("GR");
                                    string desc = LanguageManager.GetPhrase("H GR");

                                    desc = LanguageManager.SplitInLines(desc, 36);

                                    Graphics.DrawStringOutline(name, localHintPos, Color.White, Color.Black, 0.8f, null, slots.scale.x);
                                    Graphics.DrawStringOutline(desc, localHintPos + new Vec2(0, slots.scale.y * 8), Color.White, Color.Black, 0.8f, null, slots.scale.x * 0.3f);
                                }
                                if (itemNum == 2)
                                {
                                    string name = LanguageManager.GetPhrase("KAP");
                                    string desc = LanguageManager.GetPhrase("H KAP");

                                    desc = LanguageManager.SplitInLines(desc, 36);

                                    Graphics.DrawStringOutline(name, localHintPos, Color.White, Color.Black, 0.8f, null, slots.scale.x);
                                    Graphics.DrawStringOutline(desc, localHintPos + new Vec2(0, slots.scale.y * 8), Color.White, Color.Black, 0.8f, null, slots.scale.x * 0.3f);
                                }
                                if (itemNum == 3)
                                {
                                    string name = LanguageManager.GetPhrase("EDD");
                                    string desc = LanguageManager.GetPhrase("H EDD");

                                    desc = LanguageManager.SplitInLines(desc, 36);

                                    Graphics.DrawStringOutline(name, localHintPos, Color.White, Color.Black, 0.8f, null, slots.scale.x);
                                    Graphics.DrawStringOutline(desc, localHintPos + new Vec2(0, slots.scale.y * 8), Color.White, Color.Black, 0.8f, null, slots.scale.x * 0.3f);
                                }
                                if (itemNum == 4)
                                {
                                    string name = LanguageManager.GetPhrase("GS");
                                    string desc = LanguageManager.GetPhrase("H GS");

                                    desc = LanguageManager.SplitInLines(desc, 36);

                                    Graphics.DrawStringOutline(name, localHintPos, Color.White, Color.Black, 0.8f, null, slots.scale.x);
                                    Graphics.DrawStringOutline(desc, localHintPos + new Vec2(0, slots.scale.y * 8), Color.White, Color.Black, 0.8f, null, slots.scale.x * 0.3f);
                                }
                            }

                            //Amount of devices in storage block
                            numer.frame = storage;
                            numer.position = trapUIpos + new Vec2(0f, slots.scale.y / 2 * 32);
                            Graphics.Draw(numer, numer.position.x, numer.position.y, 0.78f);

                            //Invisibility cooldown block
                            if (reload <= 0f)
                            {
                                slots.frame = 0;
                            }
                            else
                            {
                                slots.frame = (int)(24 - reload * 2.4) + 1;
                            }
                            Graphics.Draw(slots, invisUIpos.x, invisUIpos.y, 0.76f);
                            //Invisibility icon
                            slots.position = invisUIpos;
                            slots.frame = 35;
                            Graphics.Draw(slots, invisUIpos.x, invisUIpos.y, 0.77f);
                            if (PlayerStats.ShowHints)
                            {
                                localHintPos = invisUIpos + new Vec2(-80 * Unit.x, 4 * Unit.y);
                                string name = LanguageManager.GetPhrase("INV");
                                string desc = LanguageManager.GetPhrase("HINV");

                                desc = LanguageManager.SplitInLines(desc, 36);

                                Graphics.DrawStringOutline(name, localHintPos, Color.White, Color.Black, 0.8f, null, slots.scale.x);
                                Graphics.DrawStringOutline(desc, localHintPos + new Vec2(0, slots.scale.y * 8), Color.White, Color.Black, 0.8f, null, slots.scale.x * 0.3f);
                            }        
                            

                            Graphics.Draw(slots, invisUIpos.x, invisUIpos.y, 0.76f);

                            //Invisibilities left
                            numer.frame = charges;
                            numer.position = invisUIpos + new Vec2(0f, slots.scale.y / 2 * 32);
                            Graphics.Draw(numer, numer.position.x, numer.position.y, 0.78f);

                            //Vision circle
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