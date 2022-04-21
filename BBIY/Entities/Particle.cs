using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Entities
{
    class Particle
    {
        public static Entity create(
            Texture2D square, Color color, Vector2 position, Vector2 direction, 
            Vector2 size, float speed, TimeSpan lifetime
        )
        {
            var particle = new Entity();
            Rectangle sourceRectangle = new Rectangle(0, 0, square.Height, square.Height);

            particle.Add(new Components.Appearance(square, color));
            particle.Add(new Components.Particle(position, direction, size, speed, lifetime));

            return particle;
        }
    }
}
