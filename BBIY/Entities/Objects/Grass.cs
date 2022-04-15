using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Entities
{
    public class Grass
    {
        public static Entity create(Texture2D grassSheet, int x, int y)
        {
            var grass = new Entity();
            Rectangle sourceRectangle = new Rectangle(0, 0, grassSheet.Height, grassSheet.Height);

            grass.Add(new Components.Appearance(grassSheet, new Color(92, 131, 57)));
            grass.Add(new Components.Position(x, y));
            grass.Add(new Components.Animated(sourceRectangle, sourceRectangle.Height));
            grass.Add(new Components.Background());

            return grass;
        }
    }
}