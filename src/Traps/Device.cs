using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    public class Device : Holdable, IDrawToDifferentLayers
    {
        private float setting = 0f;
        public EditorProperty<bool> PrePlaced;
        public bool setted; //deployed or ready to using
        public bool placeable = true; //need to deploy before using
        public bool overridePlacement = false;
        public bool zeroSpeed = true; //after throwing horizontal speed is 0
        public int team; //CTE is 2, TE is 1
        public Vec2 CheckRect = new Vec2(0f, 0f); //Needed place to deploy device
        public float setTime; //Time needed to deploy
        public string text = ""; 
        public bool wallSet;
        public bool canPlace = true;
        public Fuse_armor main;
        public Duck mainer;
        public bool onlyCT; 
        public bool onlyT;
        public string name;
        public string description;
        public int damage;
        public bool lethal;


        public Device(float xpos, float ypos) : base(xpos, ypos)
        {
            thickness = 0f;
            depth = -0.5f;
            _canFlip = false;
            weight = 0f;
            _canRaise = false;
            text = "hold @SHOOT@ to place device";
            PrePlaced = new EditorProperty<bool>(false);
        }

        public virtual void DeviceActive(Duck d)
        {
            
        }

        public virtual void Explode()
        {

        }

        public virtual void Break() 
        {
            float dist = 0f;
            float dir = 60f + Rando.Float(-10f, 10f);
            ExplosionPart ins = new ExplosionPart(position.x + (float)(Math.Cos((double)Maths.DegToRad(dir)) * (double)dist), position.y - (float)(Math.Sin((double)Maths.DegToRad(dir)) * (double)dist), true);
            Level.Add(ins);
            SFX.Play("explode", 1f, 0f, 0f, false);
            DuckNetwork.SendToEveryone(new NMBreakDevice(this));
            Level.Remove(this);
        }
        public override bool Hurt(float points) 
        {
            if(setted == true)
            {
                Break();
                return false;
                //return base.Hurt(points);
            }
            return false;
        }

        public virtual void Set() //Once, after device set
        {
            if (Halloween.upd.challengeID == 8)
            {
                if (Halloween.upd.challengeProgression < Halloween.upd.challengeTarget)
                {
                    Halloween.upd.challengeProgression += 1;
                }
            }
        }

        public virtual void PreSet()
        {
            team = 1;
        }
        public override void Update()
        {
            text = LanguageManager.GetPhrase("Place");
            base.Update();
            if(zeroSpeed == true)
                hSpeed = 0f;
            if (owner != null)
            {
                Duck d = owner as Duck;
                if (main == null && d.HasEquipment(typeof(Fuse_armor)))
                {
                    Fuse_armor f = d.GetEquipment(typeof(Fuse_armor)) as Fuse_armor;
                    main = f;
                }
            }
            if (main != null && mainer == null)
            {
                if (main._equippedDuck != null)
                {
                    mainer = main._equippedDuck;
                }
            }
            if (onlyCT == true) //T can't pickup device
            {
                if (owner != null)
                {
                    Duck d = owner as Duck;
                    if (!d.HasEquipment(typeof(CTEquipment)))
                    {
                        d.doThrow = true;
                    }
                }
                if (prevOwner != null)
                {
                    Duck d = prevOwner as Duck;
                    if (!d.HasEquipment(typeof(CTEquipment)))
                    {
                        hSpeed = 0f;
                    }
                }
            }
            if (onlyT == true) //CT can't pickup device
            {
                if (owner != null)
                {
                    Duck d = owner as Duck;
                    if (!d.HasEquipment(typeof(TEquipment)))
                    {
                        d.doThrow = true;
                    }
                }
                if (prevOwner != null)
                {
                    Duck d = prevOwner as Duck;
                    if (!d.HasEquipment(typeof(TEquipment)))
                    {
                        hSpeed = 0f;
                    }
                }
            }
            if (PrePlaced && !setted)
            {
                setting = 99f;
                setted = true;
                PreSet();
                Set();
                canPickUp = false;
            }
            if (owner != null && placeable == true)
            {             
                Duck own = owner as Duck;
                Vec2 dir = new Vec2(0f, 0f);
                Block b = Level.CheckLine<Block>(own.position + CheckRect, own.position - CheckRect);
                if (!overridePlacement)
                {
                    canPlace = (b == null ? true : false);
                }
                if (own.inputProfile.Down("SHOOT") && canPlace && own.holdObject == this && !setted && own.grounded && b == null && !own.HasEquipment(typeof(Murder)))
                {
                    owner.hSpeed *= 0.3f;
                    setting += 0.01666666f;
                }
                else
                {
                    setting = 0f;
                }
                if (own.inputProfile.Down("SHOOT") && !setted && own.grounded && b != null)
                {
                    text = LanguageManager.GetPhrase("PlaceBAD");
                }
                if (setting > setTime)
                {
                    setted = true;
                    Set();
                    if (own.HasEquipment(typeof(Fuse_armor)))
                    {
                        Fuse_armor armor = own.GetEquipment(typeof(Fuse_armor)) as Fuse_armor;
                        team = armor.team;
                    }
                    own.doThrow = true;
                    canPickUp = false;
                }
            }
        }
        public override void Draw()
        {
            base.Draw();
        }

        public void OnDrawLayer(Layer pLayer)
        {
            if(pLayer == Layer.Foreground)
            {
                if (owner != null)
                {
                    Duck d = owner as Duck;
                    if (text != "" && d.profile.localPlayer && PlayerStats.ShowHints)
                    {
                        Graphics.DrawString(text, new Vec2(d.position.x - text.Length * 6, d.position.y - 16f), Color.White, 1f, null, 1f);
                    }
                    if (setting > 0f && d.profile.localPlayer)
                    {
                        Graphics.DrawRect(position + new Vec2(-16f, -6f), position + new Vec2(16f, -12f), Color.White, 1f, false, 1f);
                        Graphics.DrawRect(position + new Vec2(-14f, -8f), position + new Vec2(-14f + setting * 28f / setTime, -10f), Color.White, 1f, true, 1f);
                    }
                }
            }
        }
    }
}
