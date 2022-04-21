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
                }
                pressAvailable = false;
            }
            if (Keyboard.GetState().GetPressedKeys().Length == 0)
            {
                pressAvailable = true;
            }
        }
    }
}
