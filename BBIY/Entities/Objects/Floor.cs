using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Entities
{
    public class Floor
    {
        public static Entity create(Texture2D floorSheet, int x, int y)
        {
            var floor = new Entity();
            Rectangle sourceRectangle = new Rectangle(0, 0, floorSheet.Height, floorSheet.Height);

            floor.Add(new Components.Appearance(floorSheet, new Color(36, 36, 36)));
            floor.Add(new Components.Animated(sourceRectangle, sourceRectangle.Height));
            floor.Add(new Components.Position(x, y));
            floor.Add(new Components.Background());

            return floor;
        }
    }
}