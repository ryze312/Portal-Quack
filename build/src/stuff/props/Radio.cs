using DuckGame;

namespace DuckGame.PortalQuack
{
    [EditorGroup("Portal Quack|Stuff|Props")]
    [BaggedProperty("canSpawn", true)]
    [BaggedProperty("isFatal", false)] 

    public class Radio : Holdable
    {
        public string stillAlive = GetPath<PortalQuack>("SFX/other/stillalive");
        Sound sfx;
        bool playing;

        SpriteMap _sprite;

        public Radio(float xpos, float ypos) : base(xpos, ypos)
        {
            _sprite = new SpriteMap(GetPath<PortalQuack>("stuff/props/stillalive"), 16, 12);
            graphic = _sprite;

            center = new Vec2(7f, 7f);
            collisionOffset = new Vec2(-7f, -4f);
            collisionSize = new Vec2(15f, 8f);
        }

        public override void OnPressAction()
        {
            if (!playing)
            { 
                sfx = SFX.Play(stillAlive, 1, 0, 0, true);
                playing = true;
            }
            else 
            {
                SFX.UnpoolSound(sfx);
                playing = false;
            }
        }


    }
}
