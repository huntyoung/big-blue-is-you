using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace Systems
{
    /// <summary>
    /// This system knows how to accept keyboard input and use that
    /// to move an entity, based on the entities 'IsYou'
    /// component settings.
    /// </summary>
    class KeyboardInput : System
    {
        private bool pressAvailable = true;

        public KeyboardInput()
            : base(typeof(Components.IsYou), typeof(Components.Position))
        {
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entity in m_entities.Values)
            {
                moveControlledEntities(entity, gameTime);
            }

            if (Keyboard.GetState().IsKeyDown(BBIY.KeyboardControls.reset) && pressAvailable)
            {
                Debug.WriteLine("Reset Level");
                pressAvailable = false;
            }
        }

        private void moveControlledEntities(Entities.Entity entity, GameTime gameTime)
        {
            var you = entity.GetComponent<Components.IsYou>();

            if (Keyboard.GetState().GetPressedKeys().Length > 0 && pressAvailable)
            {
                var key = Keyboard.GetState().GetPressedKeys()[0];

                if (you.keys.ContainsKey(key))
                {
                    you.lastMove = you.keys[key];

                    //switch (input.lastMove)
                    //{
                    //    case Components.Direction.Up:
                    //        move(entity, 0, -1);
                    //        break;
                    //    case Components.Direction.Down:
                    //        move(entity, 0, 1);
                    //        break;
                    //    case Components.Direction.Left:
                    //        move(entity, -1, 0);
                    //        break;
                    //    case Components.Direction.Right:
                    //        move(entity, 1, 0);
                    //        break;
                    //}
                }
                pressAvailable = false;
            }
            if (Keyboard.GetState().GetPressedKeys().Length == 0)
            {
                pressAvailable = true;
            }
        }

        private void move(Entities.Entity entity, int xIncrement, int yIncrement)
        {
            var position = entity.GetComponent<Components.Position>();

            position.x += xIncrement;
            position.y += yIncrement;
        }
    }
}
