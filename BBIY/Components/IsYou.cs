
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Components
{
    public class IsYou : Component
    {
        public DirectionEnum lastMove { get; set; } = DirectionEnum.Stopped;

        public Dictionary<Keys, DirectionEnum> keys;

        public IsYou(Dictionary<Keys, DirectionEnum> keys)
        {
            this.keys = keys;
        }
    }
}
