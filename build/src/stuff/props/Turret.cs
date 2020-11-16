xusing DuckGame;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame.PortalQuack
{
    [EditorGroup("Portal Quack|Stuff|Props")]

    [BaggedProperty("isSuperWeapon", true)]
    [BaggedProperty("canSpawn", false)]
    [BaggedProperty("isOnlineCapable", false)]
    [BaggedProperty("isFatal", true)]

    public class Turret : Holdable
    {
        float angleToSet;
        float gunAngle;
        AmmoType type = new AT9mm();

        SpriteMap _sprite = new SpriteMap(GetPath<PortalQuack>("stuff/props/turret"), 10, 15);

        public Turret(float xpos, float ypos) : base(xpos, ypos)
        {
            graphic = _sprite; 
      
            center = new Vec2(5f, 5f);
            collisionOffset = new Vec2(-5f, -5f);
            collisionSize = new Vec2(10f, 15f);

            gunAngle = offDir == 1 ? 0f : 180f;


        }



        public Duck Scan()
        {
            IList <Duck> targets = Level.CheckCircleAll<Duck>(position, 150).ToList(); 

            if (targets.Count != 0)
            {
                Duck nearestTarget = targets[0];
                
                // Distance to Nearest Target
                float NTDistance = 0f;

                // Finding nearest target
                foreach (Duck target in targets)
                {
                    float distance = target.position.x - this.position.x;
                    NTDistance = nearestTarget.x - this.x;

                    angleToSet = (float)Math.Atan((target.y - position.y) / (target.x - position.x));
                    angleToSet = Maths.RadToDeg(angleToSet);

                    // If distance to the target less than to nearestTarget and it's in the right angle gap and offDir is equal
                    if (Math.Abs(distance) < Math.Abs(NTDistance) && Math.Sign(distance) == offDir && angleToSet > -70 && angleToSet < 70)
                    {
                        nearestTarget = target;                 
                    }
        
                }
                
                // Checking one more time but for nearest target
                angleToSet = (float)Math.Atan((nearestTarget.y - position.y) / (nearestTarget.x - position.x));
                angleToSet = Maths.RadToDeg(angleToSet);

                if (Math.Sign(NTDistance) == offDir && angleToSet > -70 && angleToSet < 70)
                {
                    // Wrapping angle to suit offDir
                    if (offDir == -1) angleToSet += 180;
                    
                    return nearestTarget;
                }
            }
            return null;
        }

        public bool Aim()
        {   
            // For more accurate and steady firing  
            if (Math.Abs(gunAngle - angleToSet) < 5) return true;

            gunAngle = MathHelper.Lerp(gunAngle, angleToSet, 0.1f);
            return false;
        }

        public void Fire()
        {
            type.FireBullet(position, this, gunAngle, this);
        }

        public override void Update()
        {
            Duck found = Scan();
            bool aimed = false;
            if (found != null)
            {
/*              DevConsole.Log("Aiming...", Color.Blue);*/
                aimed = Aim();
            }
            if (aimed && found != null)
            {
                /*DevConsole.Log("Firing...", Color.Blue);*/
                Fire();
            }
            base.Update();
        }
    }
}
