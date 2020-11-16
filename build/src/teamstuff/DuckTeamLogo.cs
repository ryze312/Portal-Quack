namespace DuckGame.PortalQuack
{
    public class DuckTeamLogo : Level
    {
        private Sprite _logo;
        bool _fading;
		float _wait = 1f;

        public override void Initialize()
        {
            _logo = new SpriteMap(Mod.GetPath<PortalQuack>("teamstuff/title"), 328, 184);
			Graphics.fade = 0f;
        }

        public override void Update()
        {
			if (!this._fading)
			{
				if (Graphics.fade < 1f)
				{
					Graphics.fade += 0.013f;
				}
				else
				{
					Graphics.fade = 1f;
				}
			}
			else if (Graphics.fade > 0f)
			{
				Graphics.fade -= 0.013f;
			}
			else
			{
				Graphics.fade = 0f;
				if (MonoMain.startInEditor)
				{
					Level.current = Main.editor;
				}
				else
				{
					Level.current = new TitleScreen();
				}
			}
			this._wait -= 0.006f;

			if (_wait < 0f) _fading = true;
		}


        public override void PostDrawLayer(Layer layer)
        {
            if (layer == Layer.Game)
            {
                //this._logo.scale = new Vec2(0.1f, 0.1f);
                Graphics.Draw(this._logo, 0, 0f);
            }
        }
    }
}
