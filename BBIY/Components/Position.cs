using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Components
{
    public class Position : Component
    {
        public int x { get; set; }
        public int y { get; set; }
        public float layerDepth { get; set; } = 0.5f;

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
