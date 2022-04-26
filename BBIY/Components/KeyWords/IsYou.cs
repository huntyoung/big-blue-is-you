
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Components
{
    public class IsYou : Component
    {
        public DirectionEnum lastMove { get; set; } = DirectionEnum.Stopped;

        public Dictionary<Keys, DirectionEnum> keys;

        private BBIY.KeyboardControls m_loadedControls;
            
        public IsYou()
        {
            m_loadedControls = BBIY.KeyboardControlPersistance.m_loadedControls;

            if (m_loadedControls != null)
            {
                this.keys = 
                    new Dictionary<Keys, DirectionEnum>
                    {
                        { m_loadedControls.moveUp, DirectionEnum.Up },
                        { m_loadedControls.moveDown, DirectionEnum.Down },
                        { m_loadedControls.moveLeft, DirectionEnum.Left },
                        { m_loadedControls.moveRight, DirectionEnum.Right }
                    };
            }
            else
            {
                this.keys =
                    new Dictionary<Keys, DirectionEnum>
                    {
                        { Keys.W, DirectionEnum.Up },
                        { Keys.S, DirectionEnum.Down },
                        { Keys.A, DirectionEnum.Left },
                        { Keys.D, DirectionEnum.Right }
                    };
            }
        }
    }
}
