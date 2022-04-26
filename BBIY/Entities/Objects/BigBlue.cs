using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Entities
{
    public class BigBlue
    {
        private const int MOVE_INTERVAL = 150; // milliseconds
        public static Entity create(Texture2D bigBlueSheet, int x, int y)
        {
            var bigBlue = new Entity();

            bigBlue.Add(new Components.Appearance(bigBlueSheet, Color.White));
            bigBlue.Add(new Components.Position(x, y));
            bigBlue.Add(new Components.ChangeableObject("baba"));
            //bigBlue.Add(new Components.IsYou());

            return bigBlue;
        }
    }
}