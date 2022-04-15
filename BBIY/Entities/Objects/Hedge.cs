using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Entities
{
    public class Hedge
    {
        public static Entity create(Texture2D hedgeSheet, int x, int y)
        {
            var hedge = new Entity();
            Rectangle sourceRectangle = new Rectangle(0, 0, hedgeSheet.Height, hedgeSheet.Height);

            hedge.Add(new Components.Appearance(hedgeSheet, new Color(75, 92, 28)));
            hedge.Add(new Components.Position(x, y));
            hedge.Add(new Components.Animated(sourceRectangle, sourceRectangle.Height));
            hedge.Add(new Components.IsStop());

            return hedge;
        }
    }
}