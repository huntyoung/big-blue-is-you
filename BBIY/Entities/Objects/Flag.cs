using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Entities
{
    public class Flag
    {
        public static Entity create(Texture2D flagSheet, int x, int y)
        {
            var flag = new Entity();
            Rectangle sourceRectangle = new Rectangle(0, 0, flagSheet.Height, flagSheet.Height);

            flag.Add(new Components.Appearance(flagSheet, new Color(237, 226, 133)));
            flag.Add(new Components.Position(x, y));
            flag.Add(new Components.Animated(sourceRectangle, sourceRectangle.Height));
            flag.Add(new Components.ChangeableObject("flag"));

            return flag;
        }
    }
}