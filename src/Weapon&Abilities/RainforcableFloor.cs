using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    [EditorGroup("Halloween|Reinforcment")]
    public class RainforcableFloor : Block
    {
        public SpriteMap _sprite;
        public EditorProperty<int> Width;
        public EditorProperty<int> Health;
        public EditorProperty<int> Style;
        public bool Unbreakable;
        public EditorProperty<int> Type;
        private bool _init = false;
        public bool rainforced;
        public float health = 1f;
        public StateBinding _health = new StateBinding("health", -1, false, false);
        public StateBinding _rainf = new StateBinding("rainforced", -1, false, false);
        public RainforcableFloor(float xpos, float ypos) : base(xpos, ypos)
        {
            Width = new EditorProperty<int>(2, this, 1f, 4f, 1f, null, false, false);
            Health = new EditorProperty<int>(5, this, 1f, 20f, 1f, null, false, false);
            Style = new EditorProperty<int>(0, this, 0f, 1f, 1f, null, false, false);
            Type = new EditorProperty<int>(0, this, 0f, 1f, 1f, null, false, false);
            _sprite = new SpriteMap(GetPath("Sprites/ReinforcedFloor.png"), 32, 16);
            base.graphic = _sprite;
            _editorName = "Weak floor";
            center = new Vec2(16f, 8f);
            collisionSize = new Vec2(32f, 16f);
            collisionOffset = new Vec2(-16f, -12f);
            base.depth = -0.5f;
            thickness = 0.3f;
            _canFlip = false;
            hugWalls = (WallHug.Left | WallHug.Right);
            _translucent = true;
            UpdateHeight();
        }
        public virtual void UpdateHeight()
        {
            float high = Width * 16f;
            collisionSize = new Vec2(high, 8f);
            collisionOffset = new Vec2(-high / 2, -8f);
            if (high == 0)
            {
                collisionSize = new Vec2(32f, 8f);
                collisionOffset = new Vec2(-16f, -8f);
                xscale = 1f;
            }
            else
            {
                xscale = high / 32f;
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
            float high = (float)Width.value * 16f;
            if (high != 0)
                _sprite.xscale = high / 32f;
            foreach (HForceWave fw in Level.CheckRectAll<HForceWave>(topLeft, bottomRight))
            {
                if (fw != null && Unbreakable == false && rainforced == false)
                {
                    health = 0;
                }
            }
            if (health <= 0f && !Unbreakable)
            {
                Level.Remove(this);
                Break();
            }
            if (_init == false)
            {
                Unbreakable = (GameMode.numMatchesPlayed % 2 == Type ? true : false);
                _init = true;
                health = Health;
                _sprite.frame = Unbreakable ? 1 : 0;
            }
            base.Update();
        }
        public virtual void Break()
        {
            Level.Remove(this);
            IEnumerable<PhysicsObject> things = Level.CheckLineAll<PhysicsObject>(base.topLeft + new Vec2(-2f, -3f), base.topRight + new Vec2(2f, -3f));
            using (IEnumerator<PhysicsObject> enumerator = things.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    PhysicsObject thing = enumerator.Current;
                    thing._sleeping = false;
                    thing.vSpeed -= 2f;
                }
            }
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
