using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Entities
{
    public class Lava
    {
        public static Entity create(Texture2D lavaSheet, int x, int y)
        {
            var lava = new Entity();
            Rectangle sourceRectangle = new Rectangle(0, 0, lavaSheet.Height, lavaSheet.Height);

            lava.Add(new Components.Appearance(lavaSheet, new Color(130, 38, 28)));
            lava.Add(new Components.Position(x, y));
            lava.Add(new Components.Animated(sourceRectangle, sourceRectangle.Height));
            lava.Add(new Components.ChangeableObject("lava"));

            return lava;
        }
    }
}