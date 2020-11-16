using Microsoft.Xna.Framework.Graphics;
using System.Runtime.InteropServices;

namespace DuckGame.PortalQuack
{
    public class MaterialEmancipation : Material
    {
        Thing _thing;

        public MaterialEmancipation(Thing t)
        {
            _effect = Content.Load<MTEffect>("PortalQuack/stuff/shaders/meg");
            _thing = t;
        }

        public override void Apply()
        {
            foreach(EffectPass pass in this._effect.effect.CurrentTechnique.Passes)
            {
                pass.Apply();
            }
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
