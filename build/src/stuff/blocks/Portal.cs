using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DuckGame.PortalQuack
{
    [BaggedProperty("canSpawn", false)]

    class PortalCustom : MaterialThing
    {
        private SpriteMap spriteMap;
        
        // Owner Gun
        private PortalGun _gun;
        private bool _blue;
        private float _portalAngle;
        private sbyte _horizontalK, _verticalK;
        public bool drawBox;
        public float angleIn, angleOut;


        public PortalGun gun
        {
            get
            {
                return this._gun;
            }
        }

        public bool blue
        {
            get
            {
                return this._blue;
            }
        }

        public float portalAngle
        {
            get
            {
                return this._portalAngle;
            }
        }

        public sbyte horizontalK
        {
            get
            {
                return _horizontalK;
            }
            set
            {
                _horizontalK = value;
            }
        }

        public sbyte verticalK
        {
            get
            {
                return _verticalK;
            }
            set
            {
                _verticalK = value;
            }
        }

        public PortalCustom(float xpos, float ypos, bool blue, float portalAngle, PortalGun gun) : base(xpos, ypos)
        {
            this._gun = gun;
            this._blue= blue;

            DevConsole.Log("Portal: " + angleDegrees.ToString(), Color.Green);
            // Setting max velocity to zero to prevent moving after explosions
            thickness = 100f;

            // Setting Sprite Map according to color and direction
            // And also collision center
            if (blue)
            {
                    spriteMap = new SpriteMap(Mod.GetPath<PortalQuack>("stuff\\blocks\\portalblueleft"), 7, 32, true);
                
            }
            else spriteMap = new SpriteMap(Mod.GetPath<PortalQuack>("stuff\\blocks\\portalorangeleft"), 7, 32, true);

            // Disable Gravity
            
            graphic = spriteMap;
            
            // Collision Settings
            

            angleDegrees = portalAngle;
            _portalAngle = portalAngle;
            

            center = new Vec2(0f, 15f);

            if (angleDegrees == 0)
            {
                collisionOffset = new Vec2(0f, -15f);
                collisionSize = new Vec2(2f, 32f);
            }
            else if (angleDegrees == 180)
            {
                collisionOffset = new Vec2(-2f, -17f);
                collisionSize = new Vec2(2f, 32f);
            }
            else if (angleDegrees == 90)
            {
                collisionOffset = new Vec2(-17f, 0f);
                collisionSize = new Vec2(32f, 2f);
            }
            else
            {
                collisionOffset = new Vec2(-15f, -4f);
                collisionSize = new Vec2(32f, 4f);
            }
        }

        public override bool DoHit(Bullet bullet, Vec2 hitPos)
        {
            DevConsole.Log("Bullet!", Color.Aquamarine);
            DevConsole.Log(bullet.ToString(), Color.Green);

            PortalCustom targetPortal = Level.current.things[typeof(PortalCustom)].FirstOrDefault((Thing p) => (p as PortalCustom).gun == _gun && (p as PortalCustom).blue == !_blue) as PortalCustom;

            if (targetPortal != null)
            {
                Vec2 newPos = targetPortal.position;
                float newAngle = bullet.angle;

                if (newAngle < 0)
                {
                    newAngle += 360;
                }

                newAngle = newAngle - angleIn;
                
                if (horizontalK == targetPortal.horizontalK && verticalK == targetPortal.verticalK)
                {
                    newAngle *= -1f;
                }
                newAngle = targetPortal.angleOut + newAngle;

                
                DevConsole.Log(angleDegrees.ToString(), Color.Red);
                DevConsole.Log(targetPortal.angleDegrees.ToString(), Color.Red);
                DevConsole.Log(bullet.angle.ToString(), Color.Red);
                DevConsole.Log(newAngle.ToString(), Color.Red);

                if (targetPortal.angleDegrees == 0)
                {
                    newPos.x += bullet.width + 5f;
                }

                else if (targetPortal.angleDegrees == 180)
                {
                    newPos.x -= bullet.width + 5f;
                }

                else if (targetPortal.angleDegrees == 90)
                {
                    newPos.y += bullet.height + 5f;
                }

                else if (targetPortal.angleDegrees == 270)
                {
                    newPos.y -= bullet.height + 5f;

                }

                bullet.ammo.FireBullet(newPos, null, newAngle*-1f, bullet.firedFrom);
            }

            return true;
        }

        public override void Impact(MaterialThing with, ImpactedFrom from, bool solidImpact)
        {
            DevConsole.Log("Collision", new Color(255, 0, 255));

            // Getting opposite portal
            PortalCustom targetPortal = Level.current.things[typeof(PortalCustom)].FirstOrDefault((Thing p) => (p as PortalCustom).gun == _gun && (p as PortalCustom).blue == !_blue) as PortalCustom;

            if (targetPortal != null)
            {
                Vec2 newPos = targetPortal.position;
                float speedH = with.hSpeed;
                float speedV = with.vSpeed;

                if ( (targetPortal.verticalK == 0 && this.verticalK != 0) || (targetPortal.horizontalK == 0 && this.horizontalK != 0) )
                {
                    speedH = with.vSpeed;
                    speedV = with.hSpeed;
                }


                if (targetPortal.angleDegrees == 0)
                {
                    newPos.x += with.collisionSize.x;
                }
                
                else if (targetPortal.angleDegrees == 180)
                {
                    newPos.x -= with.collisionSize.x;
                }

                else if (targetPortal.angleDegrees == 90)
                {
                    newPos.y += with.collisionSize.y;
                }

                else if (targetPortal.angleDegrees == 270)
                {
                    newPos.y -= with.collisionSize.y;

                }

                if (targetPortal.horizontalK != 0)
                {
                    speedH = System.Math.Abs(speedH);
                    speedH *= targetPortal.horizontalK;
                }
                
                if (targetPortal.verticalK != 0)
                {
                    speedV = System.Math.Abs(speedV);
                    speedV *= targetPortal.verticalK;
                }


                if (with is RagdollPart)
                {
                    Ragdoll doll = (with as RagdollPart)._doll;
                    RagdollPart[] parts = { doll.part1, doll.part2, doll.part3 };

                    foreach (RagdollPart part in parts)
                    {
                        part.position = newPos;
                        part.hSpeed = speedH;
                        part.vSpeed = speedV;
                    }
                    return;

                }
                
                with.position = newPos;
                with.hSpeed = speedH;
                with.vSpeed = speedV;

                with.OnTeleport();

            }
        }

        public override void Draw()
        {
            if (drawBox)
            {
                Graphics.DrawRect(topLeft, bottomRight, Color.Red, 1f, false);
            }
            base.Draw();
        }

    }
}
