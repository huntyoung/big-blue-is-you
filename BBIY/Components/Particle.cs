using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Components
{
    public class Particle : Component
    {
        public Vector2 position;
        public Vector2 direction;
        public Vector2 size;
        public float speed;
        public TimeSpan lifetime;

        public Particle(Vector2 position, Vector2 direction, Vector2 size, float speed, TimeSpan lifetime)
        {
            this.position = position;
            this.direction = direction;
            this.size = size;
            this.speed = speed;
            this.lifetime = lifetime;
        }
    }
}
