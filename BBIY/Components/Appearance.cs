using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Components
{
    public class Appearance : Component
    {
        public Texture2D image;
        public Color fill;

        public Appearance(Texture2D image, Color fill)
        {
            this.image = image;
            this.fill = fill;
        }
    }
}
