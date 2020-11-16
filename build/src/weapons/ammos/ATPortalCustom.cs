namespace DuckGame.PortalQuack
{
    // Based on official Duck Game Portal Gun
    // That actually has been canceled. 
    //IDK what could happen to cancel this wonderful weapon

    internal class ATPortalCustom: AmmoType
    {
        private SpriteMap spriteMap;
        private PortalGun _ownerGun;
        private bool _blue;

        public ATPortalCustom(bool blue, PortalGun ownerGun)
        {
            // Maximum Accuracy, because in original game it never misses
            accuracy = 1f;
            
            // Maximum Range, because in original game it has infinite range
            range = 10000f;
            
            // TODO
            // Check if bullet speed ruins game balance
            // Maximum speed, because in original game portal creates instantly
            bulletSpeed = 100f;
            //deadly = false;
            
            // Big Thickness for easy aim
            bulletThickness = 2f;
            
            // Getting owner gun to link portals to
            this._ownerGun = ownerGun;
            
            // Setting bullet color
            this._blue = blue;

            // Zero penetration to never shoot through crates, door etc
            penetration = 0f;

            // Setting Sprite Map according to color
            if (blue)
            {
                //DevConsole.Log("Changing ammo to Blue", new Color(255, 0, 0, 0));
                spriteMap = new SpriteMap(Mod.GetPath<PortalQuack>("projectiles\\spherequackblue"), 6, 6, true);
            }
            else
            {
                //DevConsole.Log("Changing ammo to Orange", new Color(255, 0, 0, 0));
                spriteMap = new SpriteMap(Mod.GetPath<PortalQuack>("projectiles\\spherequackorange"), 6, 6, true);
            }

            sprite = spriteMap;
            sprite.CenterOrigin();
        }

        public override Bullet FireBullet(Vec2 position, Thing owner = null, float angle = 0f, Thing firedFrom = null)
        {
            angle *= -1f;
            Bullet bullet = new PortalBulletCustom(position.x, position.y, this, _blue, angle, _ownerGun, false, -1f, false, true);
            bullet.firedFrom = firedFrom;
            Level.Add(bullet);
            return bullet;
        }

    }
}
