namespace DuckGame.PortalQuack
{
    [EditorGroup("Portal Quack|Stuff|Blocks")]

    [BaggedProperty("canSpawn", false)]
    [BaggedProperty("isOnlineCapable", false)]
    class Grill : MaterialThing
    {
        public Grill(float xpos, float ypos) : base(xpos, ypos)
        {
            graphic = new SpriteMap(Mod.GetPath<PortalQuack>("stuff/blocks/meg"), 10, 32);
            
            center = new Vec2(5f, 15f);
            collisionOffset = new Vec2(-5f, -15f);
            collisionSize = new Vec2(9f, 31f);

        }

        public override void Impact(MaterialThing with, ImpactedFrom from, bool solidImpact)
        {
            if (with is PortalGun) (with as PortalGun).Reset();
            if (with is Duck || with is Gun) return;

            with.Disintegrate();

            base.Impact(with, from, solidImpact);
        }
    }
}
