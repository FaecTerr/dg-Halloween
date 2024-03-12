using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    [EditorGroup("Halloween")]
    public class TUT_Mode : Thing
    {

        private SpriteMap _sprite;

        public string Name = "";
        public string _string;
        public bool init = false;
        public string str;

        public SpriteMap _mark = new SpriteMap(Mod.GetPath<Halloween>("Sprites/TaskMark.png"), 16, 16);
        public SpriteMap _bg = new SpriteMap(Mod.GetPath<Halloween>("Sprites/TaskBackGround"), 64, 20);

        public StateBinding _time = new StateBinding("time", -1, false, false);
        public StateBinding _ctWinBinding = new StateBinding("ctWins", -1, false, false);
        public StateBinding _tWinBinding = new StateBinding("tWins", -1, false, false);
        private BitmapFont _font;
       // private BitmapFont _bigfont;

        public int taskNum = 1;
        public string description = "";

       // private BitmapFont _fon;

        public int totalPlayers;
        public Fuse_armor f;
        public Duck duck;
        public List<Duck> Players = new List<Duck>();
        public int selectedPlayer;

        public int timeFromStart;

        public bool localPlayerIsGhost;
        public bool localPlayerIsMurderer;

        public EditorProperty<bool> isMurderer = new EditorProperty<bool>(false);

        public int changeTask;

        public TUT_Mode(float xval, float yval) : base(xval, yval)
        {
            _font = new BitmapFont("biosFont", 8, -1);
            _sprite = new SpriteMap(Mod.GetPath<Halloween>("Sprites/TutorialMode.png"), 16, 16, false);
            base.graphic = _sprite;
            center = new Vec2(8f, 8f);
            collisionOffset = new Vec2(-8f, -8f);
            collisionSize = new Vec2(15f, 15f);
            graphic = _sprite;
            layer = Layer.Foreground;
            depth = 0.9f;
            _editorName = "Tutorial";
        }

        public override void Update()
        {
            if (!(Level.current is Editor))
            {
                _sprite.frame = 1;
            }
            base.Update();
            int liveCounter = 0;
            if(f == null)
            {
                foreach(Fuse_armor fuse in Level.current.things[typeof(Fuse_armor)])
                {
                    if(fuse.theduck != null)
                    {
                        f = fuse;
                        duck = fuse.theduck;
                    }
                }
            }
            if(timeFromStart <= 60)
            {
                timeFromStart++;
            }
            foreach(Duck d in Level.current.things[typeof(Duck)])
            {
                if (!d.dead)
                {
                    liveCounter += 1;
                }
            }
            if (isMurderer)
            {
                if (liveCounter == 0 && timeFromStart > 60)
                {
                    Level.current = new ChallengeLevel(Mod.GetPath<Halloween>("Levels/Tutorial/[Halloween] [Tutorial Murderer] Southwest mansion.lev")); ;
                }
                if (taskNum == 1)
                {
                    description = "Break the wall and enter in the house";
                }
                if (taskNum == 2)
                {
                    description = "Destroy 3 traps";
                }
                if (taskNum == 3)
                {
                    description = "Kill the ghost";
                }
            }

            if (!isMurderer)
            {
                if (liveCounter == 0 && timeFromStart > 60)
                {
                    Level.current = new ChallengeLevel(Mod.GetPath<Halloween>("Levels/Tutorial/[Halloween] [Tutorial Ghost] Southwest mansion.lev"));
                }
                if (taskNum == 1)
                {
                    description = "Select grzmot mine";
                }
                if (taskNum == 2)
                {
                    description = "Deploy 5 traps";
                }
                if (taskNum == 3)
                {
                    description = "Use your ability";
                }
            }

            if (isMurderer)
            {

                if ((Level.current.things[typeof(RainforcableFloor)].Count() < 1 || Level.current.things[typeof(RainforcableWall)].Count() < 5) && taskNum == 1 && changeTask <= 0)
                {
                    changeTask = 120;
                }
                if ((Level.current.things[typeof(Device)].Count() < 7) && taskNum == 2 && changeTask <= 0)
                {
                    changeTask = 120;
                }
                if ((Level.current.things[typeof(Ghost)].Count() < 3) && taskNum == 3 && changeTask <= 0)
                {
                    changeTask = 120;
                }
                if (taskNum == 4 && changeTask <= 0)
                {
                    Level.current = new HalloweenLevel();
                }
            }
            if (!isMurderer)
            {
                if (f != null)
                {
                    Ghost g = f as Ghost;
                    if (g.itemNum == 1 && taskNum == 1 && changeTask <= 0)
                    {
                        changeTask = 120;
                    }
                }
                if (f != null)
                {
                    Ghost g = f as Ghost;
                    if (g.TakenDevices.Count >= 5  &&taskNum == 2 && changeTask <= 0)
                    {
                        changeTask = 120;
                    }
                }
                if (f != null)
                {
                    Ghost g = f as Ghost;
                    if (g.duration > 0 && taskNum == 3 && changeTask <= 0)
                    {
                        changeTask = 120;
                    }
                }
                if (taskNum == 4 && changeTask <= 0)
                {
                    Level.current = new HalloweenLevel();
                }
            }
            if (changeTask > 0)
            {
                if(changeTask == 1)
                {
                    taskNum += 1;
                }
                changeTask--;
                _mark.frame = 2;
            }
            else
            {
                _mark.frame = 0;
            }
        }

        public override void Draw()
        {
            DrawTask();
            base.Draw();
        }

        public void DrawTask()
        {
            int camSmollness = 400;
            Vec2 camSize = new Vec2(Level.current.camera.width, Level.current.camera.width);
            Vec2 realCamSize = new Vec2(Level.current.camera.width, Level.current.camera.height);
            Vec2 camPos = new Vec2(Level.current.camera.position.x, Level.current.camera.position.y);

            _mark.scale = camSize / camSmollness;
            _bg.scale = camSize / camSmollness;
            _font.scale = camSize / (camSmollness * 3f);

            Graphics.Draw(_mark, camPos.x + camSize.x - _bg.scale.x * 56f, camPos.y + _bg.scale.y * 16f, 0.99f);
            Graphics.Draw(_bg, camPos.x + camSize.x - _bg.scale.x * 64f, camPos.y + _bg.scale.y * 16f, 0.98f);
            _font.DrawOutline(description, new Vec2(camPos.x + camSize.x - _bg.scale.x * 50f, camPos.y + _bg.scale.y * 16f), Color.White, Color.Black, 1f);

        }
    }
}
