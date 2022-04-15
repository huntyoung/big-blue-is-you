using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Entities
{
    public class Rock
    {
        public static Entity create(Texture2D rockSheet, int x, int y)
        {
            var rock = new Entity();
            Rectangle sourceRectangle = new Rectangle(0, 0, rockSheet.Height, rockSheet.Height);

            rock.Add(new Components.Appearance(rockSheet, new Color(144, 103, 62)));
            rock.Add(new Components.Position(x, y));
            rock.Add(new Components.Animated(sourceRectangle, sourceRectangle.Height));

            return rock;
        }
    }
}