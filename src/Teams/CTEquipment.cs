using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuckGame;

namespace DuckGame.Halloween
{
    public class CTEquipment : Fuse_armor
    {
        public CTEquipment(float xval, float yval) : base(xval, yval)
        {
            _pickupSprite = new SpriteMap(GetPath("Sprites/CTE"), 32, 32);
            _sprite = new SpriteMap(GetPath("Sprites/CTE"), 32, 32, false);
            center = new Vec2(16f, 16f);
            team = 1;
            base.graphic = _sprite;
            _wearOffset = new Vec2(1f, 1f);
            _sprite.AddAnimation("idle", 1f, true, new int[1]);
        }

        public override void Update()
        {
            base.Update();    
            if(_equippedDuck != null)
            {
                Duck d = _equippedDuck as Duck;          
                if(d.holdObject is TEquipment)
                {
                    d.doThrow = true;
                }
            }
            if (prevOwner != null)
            {
                Duck d = prevOwner as Duck;
                if (d.dead == true)
                {
                    canPickUp = false;
                }
            }
        }
    }
}
