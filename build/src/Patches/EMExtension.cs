namespace DuckGame.PortalQuack
{
    // EM stands for emancipation
    public static class EMExtension
    {
        public static void Disintegrate(this Thing thing)
        {
            thing.material = new MaterialEmancipation(thing);
            (thing as PhysicsObject).gravMultiplier = 0.7f;

        }

    }
}
