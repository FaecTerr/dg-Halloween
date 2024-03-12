﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuckGame.Halloween
{
    public class MurdererSpawner : Thing
    {
        public bool init;
		public MurdererSpawner(float xpos, float ypos) : base(xpos, ypos)
        {
            graphic = new SpriteMap(GetPath("Sprites/SpecialStuff/MurdererSpawner.png"), 32, 32, false)
            {
                depth = 0.9f
            };
            _editorName = "Hunters Spawn";
            center = new Vec2(16f, 23f);
            collisionSize = new Vec2(16f, 16f);
            collisionOffset = new Vec2(-8f, -16f);
            _visibleInGame = false;
        }
        public override void Update()
        {
            base.Update();
            if (!init && !(Level.current is Editor))
            {
                foreach(Duck d in Level.current.things[typeof(Duck)])
                {
                    if (d.team.name == "Murderer")
                    {
                        d.position = position;
                    }
                }
                init = true;
            }
        }
    }
}
