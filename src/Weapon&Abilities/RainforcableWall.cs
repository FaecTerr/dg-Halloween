using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    [EditorGroup("Halloween|Reinforcment")]
    public class RainforcableWall : Block
    {
        public SpriteMap _sprite;
        public EditorProperty<int> Height;
        public EditorProperty<int> Health;
        public EditorProperty<int> Style;
        public EditorProperty<int> Type;
        public bool Unbreakable;
        private bool _init=false;
        public bool rainforced;
        public float health = 1f;
        public StateBinding _rainf = new StateBinding("rainforced", -1, false, false);
        public StateBinding _health = new StateBinding("health", -1, false, false);
        public RainforcableWall(float xpos, float ypos) : base(xpos, ypos)
        {
            Height = new EditorProperty<int>(2, this, 1f, 4f, 1f, null, false, false);
            Health = new EditorProperty<int>(5, this, 1f, 20f, 1f, null, false, false);
            Style = new EditorProperty<int>(0, this, 0f, 1f, 1f, null, false, false);
            Type = new EditorProperty<int>(0, this, 0f, 1f, 1f, null, false, false);
            _sprite = new SpriteMap(GetPath("Sprites/ReinforcedWall.png"), 16, 32);
            base.graphic = _sprite;
            base.sequence = new SequenceItem(this);
            base.sequence.type = SequenceItemType.Goody;
            center = new Vec2(8f, 16f);
            collisionSize = new Vec2(16f, 32f);
            collisionOffset = new Vec2(-8f, -16f);
            base.depth = -0.5f;
            _editorName = "Weak wall";
            thickness = 0.3f;
            _canFlip = false;
            hugWalls = (WallHug.Left | WallHug.Right);
            _translucent = true;
            hugWalls = WallHug.Floor;
            UpdateHeight();
        }
        public virtual void UpdateHeight()
        {
            float high = (float)Height.value * 16f;
            //center = new Vec2(high/2, 4f);
            collisionSize = new Vec2(8, high);
            collisionOffset = new Vec2(-4, -high/2);
            if (high == 0)
            {
                collisionSize = new Vec2(8, 32);
                collisionOffset = new Vec2(-4f, -16f);
                yscale = 1f;
            }
            else
            {
                yscale = high / 32f;
            }
        }
        public override void EditorPropertyChanged(object property)
        {
            UpdateHeight();
        }
        public override void Initialize()
        {
            UpdateHeight();
        }
        public override void Update()
        {
            foreach(HForceWave fw in Level.CheckRectAll<HForceWave>(topLeft, bottomRight))
            {
                if (fw != null && Unbreakable == false && rainforced == false)
                {
                    health = 0;
                }
            }
            float high = (float)Height.value * 16f;
            if (high != 0)
                _sprite.yscale = high / 32f;
            if (health <= 0f && !Unbreakable)
            {
                Break();
            }
            if(_init==false)
            {
                Unbreakable = (GameMode.numMatchesPlayed % 2 == Type ? true : false);
                _init = true;
                health = Health;
                _sprite.frame = Unbreakable ?  1 : 0;
            }
            base.Update();
        }
        public virtual void Break()
        {
            Level.Remove(this);
            for (int i = 0; i < 30; i++)
            {
                Thing ins = WoodDebris.New(base.x - 8f + Rando.Float(16f), base.y - 8f + Rando.Float(16f));
                ins.hSpeed = ((Rando.Float(1f) > 0.5f) ? 1f : -1f) * Rando.Float(3f);
                ins.vSpeed = -Rando.Float(1f);
                Level.Add(ins);
            }
        }
    }
}
