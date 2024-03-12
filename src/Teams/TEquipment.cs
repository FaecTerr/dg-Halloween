using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuckGame;

namespace DuckGame.Halloween
{
    public class TEquipment : Fuse_armor
    {
        public TEquipment(float xval, float yval) : base(xval, yval)
        {
            _pickupSprite = new SpriteMap(GetPath("Sprites/TE"), 32, 32);
            _sprite = new SpriteMap(GetPath("Sprites/TE"), 32, 32, false);
            center = new Vec2(16f, 16f);
            team = 2;
            base.graphic = _sprite;
            _wearOffset = new Vec2(1f, 1f);
            _sprite.AddAnimation("idle", 1f, true, new int[1]);
        }
        public override void Update()
        {
            base.Update();

            if (_equippedDuck != null)
            {
                Duck d = _equippedDuck as Duck;
                if (d.holdObject is CTEquipment)
                {
                    d.doThrow = true;
                }
            }
            if(prevOwner != null)
            {
                Duck d = prevOwner as Duck;
                if(d.dead == true)
                {
                    canPickUp = false;
                }
            }
        }
    }
}