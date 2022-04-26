using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Entities
{
    public class Water
    {
        public static Entity create(Texture2D waterSheet, int x, int y)
        {
            var water = new Entity();
            Rectangle sourceRectangle = new Rectangle(0, 0, waterSheet.Height, waterSheet.Height);

            water.Add(new Components.Appearance(waterSheet, new Color(95, 157, 209)));
            water.Add(new Components.Position(x, y));
            water.Add(new Components.Animated(sourceRectangle, sourceRectangle.Height));
            water.Add(new Components.ChangeableObject("water"));

            return water;
        }
    }
}