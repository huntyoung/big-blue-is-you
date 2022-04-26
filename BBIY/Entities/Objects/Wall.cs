using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Entities
{
    public class Wall
    {
        public static Entity create(Texture2D wallSheet, int x, int y)
        {
            var wall = new Entity();
            Rectangle sourceRectangle = new Rectangle(0, 0, wallSheet.Height, wallSheet.Height);

            wall.Add(new Components.Appearance(wallSheet, new Color(41, 49, 65)));
            wall.Add(new Components.Position(x, y));
            wall.Add(new Components.Animated(sourceRectangle, sourceRectangle.Height));
            wall.Add(new Components.ChangeableObject("wall"));

            return wall;
        }
    }
}