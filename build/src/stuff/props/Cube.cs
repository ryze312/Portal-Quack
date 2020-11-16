namespace DuckGame.PortalQuack
{
    [EditorGroup("Portal Quack|Stuff|Props")]

    [BaggedProperty("isSuperWeapon", false)]
    [BaggedProperty("canSpawn", true)]
    [BaggedProperty("isOnlineCapable", false)]
    [BaggedProperty("isFatal", false)]
    public class Cube : Holdable, IPlatform
    {
        private readonly SpriteMap sprite;

        public Cube(float xpos, float ypos) : base(xpos, ypos)
        {
            _editorName = "Cube";

            sprite = new SpriteMap(GetPath<PortalQuack>("stuff\\props\\cube"), 23, 22);
            graphic = sprite;

            center = new Vec2(11f, 10f);
            collisionOffset = new Vec2(-11f, -10f);
            collisionSize = new Vec2(22f, 21f);
            thickness = 5f;
            physicsMaterial = PhysicsMaterial.Plastic;

        }
	}

}
