using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Entities
{
    public class Text
    {
        public static Entity create(Texture2D textSheet, string word, int x, int y, Color color)
        {
            var text = new Entity();
            Rectangle sourceRectangle = new Rectangle(0, 0, textSheet.Height, textSheet.Height);

            text.Add(new Components.Appearance(textSheet, color));
            text.Add(new Components.Position(x, y));
            text.Add(new Components.Animated(sourceRectangle, sourceRectangle.Height));
            text.Add(new Components.IsPush());
            text.Add(new Components.Text(word));

            return text;
        }
    }
}