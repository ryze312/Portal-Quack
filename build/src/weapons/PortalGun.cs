using System.Linq;
using System.Collections.Generic;
namespace DuckGame.PortalQuack
{
    [EditorGroup("Portal Quack|Guns")]

    [BaggedProperty("isSuperWeapon", false)]
    [BaggedProperty("canSpawn", true)]
    [BaggedProperty("isOnlineCapable", false)]
    [BaggedProperty("isFatal", true)]

    public class PortalGun : Gun
    {
     
        private SpriteMap _sprite;
        private bool portalColor = true; // True states for blue, false for orange

        // Shooting sounds
        private readonly string soundBlue = Mod.GetPath<PortalQuack>("SFX/portalgun/pgfireblue");
        private readonly string soundOrange = Mod.GetPath<PortalQuack>("SFX/portalgun/pgfireorange");

        public PortalGun(float xval, float yval) : base(xval, yval)
        {
            _editorName = "Portal Gun";
            
            // 99 to prevent from running out of ammo
            ammo = 99;
            // Custom AmmoType for creating portal
            _ammoType = new ATPortalCustom(true, this);
            // Low kick force to prevent recoil
            _kickForce = 0.1f;

            // Allow Transparency to properly show particles
            _sprite = new SpriteMap(GetPath<PortalQuack>("weapons\\ducksciencehandheldportaldevice"), 32, 32, true);
            
            // Animations for Blue and Orange Portal Guns
            _sprite.AddAnimation("Blue", 1f, false, 0);
            _sprite.AddAnimation("Orange", 1f, false, 1);
            _sprite.SetAnimation("Blue");
            
            // Graphic to show in Editor Menu
            graphic = _sprite;
            
            // Custom flare sprite
            // Shows after shoot
            _flare = new SpriteMap(Mod.GetPath<PortalQuack>("projectiles\\spherequackblue"), 6, 6, true);
            
            // Collision Settings
            center = new Vec2(15f, 18f);
            collisionOffset = new Vec2(-11f, -9f); // 4 9
            _collisionSize = new Vec2(25f, 12f); // 29 21
            
            _barrelOffsetTL = new Vec2(25f, 15f);
            _holdOffset = new Vec2(5f, 3f);
            handOffset = new Vec2(2f, 0f);
            
            // Setting Base Material
            // Portal Gun is made of metal. Am I Right?
            physicsMaterial = PhysicsMaterial.Metal;
            // Setting standard fire sound
            _fireSound = soundBlue;

        }


        public override void OnPressAction()
        {
            base.OnPressAction();   
            // Reset Ammo
            ammo = 99;
        }


        public override void Update()
        {
            Duck d = this.owner as Duck;

            if (d != null)
            {
                // If Quack Button Pressed
                // Change Color to opposite
                if (d.inputProfile.Pressed("QUACK", false))
                {
                    portalColor = !portalColor;
                }
            }

            // Change sprite and fire sound if color has changed
            if (portalColor && _sprite.currentAnimation == "Orange")
            {
                //DevConsole.Log("Changing color to Blue", new Color(255, 0, 0, 0));
                _ammoType = new ATPortalCustom(true, this);
                _fireSound = soundBlue;
                _sprite.SetAnimation("Blue");
            }
            else if (!portalColor && _sprite.currentAnimation == "Blue")
            {
                //DevConsole.Log("Changing color to Orange", new Color(255, 0, 0, 0));
                _ammoType = new ATPortalCustom(false, this);
                _fireSound = soundOrange;
                _sprite.SetAnimation("Orange");
            }
            base.Update();
        }

        public void Reset()
        {
            foreach (PortalCustom portal in Level.current.things[typeof(PortalCustom)].Where(t => t.owner == this)) {
                Level.Remove(portal);
            }
        }

    }
}
