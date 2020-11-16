using System;
using System.Linq;

namespace DuckGame.PortalQuack
{

    class PortalBulletCustom : Bullet
    {
        PortalGun _ownerGun;
        bool _blue;
        // True states for right portal, false for left
        public PortalBulletCustom(float xval, float yval, AmmoType type, bool blue, float ang = -1f, Thing owner = null, bool rbound = false, float distance = -1f, bool tracer = false, bool network = false) : base(xval, yval, type, ang, owner, rbound, distance, tracer, network)
        {
            _ownerGun = owner as PortalGun;
            _blue = blue;
        }


        public override void OnCollide(Vec2 pos, Thing thing, bool willBeStopped)
        {
            // IF collided with block
            if (thing is Block)
            {
                Block block = thing as Block;
                // If travel Dir > 0, direction is right
                int portalAngle = 0;

                //ImpactedFrom from;

                if (pos.y >= thing.top + 1f && pos.y <= thing.bottom - 1f)
                {

                    portalAngle = travelDirNormalized.x > 0f ? 180 : 0;
                }
                else
                {
                    portalAngle = travelDirNormalized.y > 0f ? 270 : 90;
                }

                PortalGun gun = _ownerGun;

                // Check if gun exists and there is enough space on blocks
                // Basically, if there is more than two blocks
                if (gun != null)
                {
                    // Getting existing Owner's Portal with target color
                    // PortalCustom.gun == owner && PortalCustom.blue == _blue
                    PortalCustom oldPortal = Level.current.things[typeof(PortalCustom)].FirstOrDefault((Thing p) => (p as PortalCustom).gun == owner && (p as PortalCustom).blue == _blue) as PortalCustom;
                   

                    // Creating new portal at target destination
                    // Y is thing's y to link portal to block
                    PortalCustom portal = new PortalCustom(pos.x, pos.y, _blue, portalAngle, _ownerGun);

                    
                    DevConsole.Log(block.top.ToString(), Color.Aqua);
                    DevConsole.Log(portal.centery.ToString(), Color.Aqua);

                    
                    // Fixing Collision
                    portal = FixCollision(portal, block);

                    // Check if portal already exists
                    if (oldPortal != null && portal != null)
                    {
                        // Delete it
                        Level.Remove(oldPortal);
                        Level.Add(portal);

                    }
                    // If portal can be created, create it
                    else if (portal != null)
                    {
                        Level.Add(portal);
                    }

                }
            }
        }

        private PortalCustom FixCollision(PortalCustom portal, Block block)
        {
            // Method to fix collision portal problems 

            // If portal is on right,
            // Check existence of up block and downblock
            // Also check blocks under and above portal
            Block downwall, downblock, upwall, upblock;
            downblock = upblock = null;

            DevConsole.Log(portal.offDir.ToString(), Color.Gold);

            if (portal.angleDegrees == 180 || portal.angleDegrees == 0)
            {
                downwall = block.downBlock;
                upwall = block.upBlock;
                portal.y = block.y;

                portal.verticalK = 0;
            }

            else
            {
                DevConsole.Log("JOPA", Color.OrangeRed);
                downwall = block.leftBlock;
                upwall = block.rightBlock;

                portal.x = block.x;

                portal.horizontalK = 0;
            }


            if (portal.angleDegrees == 180)
            {
                if (downwall != null) downblock = downwall.leftBlock;
                if (upwall != null) upblock = upwall.leftBlock;

                portal.right = block.left;

                portal.horizontalK = -1;
                portal.angleIn = 0;
                portal.angleOut = 180;
            }

            else if (portal.angleDegrees == 0)
            {
                if (downwall != null) downblock = downwall.rightBlock;
                if (upwall != null) upblock = upwall.rightBlock;

                portal.left = block.right;

                portal.horizontalK = 1;
                portal.angleIn = 180;
                portal.angleOut = 0;
            }

            else if (portal.angleDegrees == 90)
            {
                if (downwall != null) downblock = downwall.downBlock;
                if (upwall != null) upblock = upwall.downBlock;

                portal.top = block.bottom;

                portal.verticalK = 1;
                portal.angleIn = 90;
                portal.angleOut = 270;
            }

            else
            {
                if (downwall != null) downblock = downwall.upBlock;
                if (upwall != null) upblock = upwall.upBlock;

                portal.bottom = block.top;

                portal.verticalK = -1;
                portal.angleIn = 270;
                portal.angleOut = 90;
            }

            if (downwall != null && upwall != null && downblock == null && upblock == null)
            {
                return portal;
            }
            else if (downwall != null && downblock == null)
            {

                if (portal.angleDegrees == 180 || portal.angleDegrees == 0) portal.y = downwall.y;
                else portal.x = downwall.x;

            }
            else if (upwall != null && upblock == null)
            {

                if (portal.angleDegrees == 180 || portal.angleDegrees == 0) portal.y = upwall.y;
                else portal.x = upwall.x;
            }
            else
            {
                return null;
            }

            return portal;
        }
    }
}
