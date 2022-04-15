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
            bigBlue.Add(new Components.IsYou(
                new Dictionary<Keys, Components.DirectionEnum>
                {
                        { BBIY.KeyboardControls.moveUp, Components.DirectionEnum.Up },
                        { BBIY.KeyboardControls.moveDown, Components.DirectionEnum.Down },
                        { BBIY.KeyboardControls.moveLeft, Components.DirectionEnum.Left },
                        { BBIY.KeyboardControls.moveRight, Components.DirectionEnum.Right }
                }));

            return bigBlue;
        }
    }
}