using Microsoft.Xna.Framework;
using System;
using Entities;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Systems
{
    class ParticleSystem : System
    {
        private Action<Entity> m_addEntity;
        private Action<Entity> m_removeEntity;

        private readonly int GRID_SIZE;
        private readonly int CELL_SIZE;
        private readonly int OFFSET_X;
        private readonly int OFFSET_Y;

        private BBIY.MyRandom m_random = new BBIY.MyRandom();

        private TimeSpan m_accumulated = TimeSpan.Zero;
        private TimeSpan m_rate = new TimeSpan(0, 0, 0, 0, 100);

        public ParticleSystem(Action<Entity> addEntity, Action<Entity> removeEntity, int screenWidth, int screenHeight, int gridSize) : base(typeof(Components.Particle))
        {
            m_addEntity = addEntity;
            m_removeEntity = removeEntity;
            GRID_SIZE = gridSize;
            CELL_SIZE = screenHeight / gridSize;
            OFFSET_X = (screenWidth - gridSize * CELL_SIZE) / 2;
            OFFSET_Y = (screenHeight - gridSize * CELL_SIZE) / 2;
        }

        public override void Update(GameTime gameTime)
        {
            List<Entity> removeMe = new List<Entity>();
            foreach (var entity in m_entities.Values)
            {
                var particleAttr = entity.GetComponent<Components.Particle>();

                // For any existing particles, update them, if we find ones that have expired, add them
                // to the remove list.
                particleAttr.lifetime -= gameTime.ElapsedGameTime;
                if (particleAttr.lifetime < TimeSpan.Zero)
                {
                    // Add the particle id to the remove list
                    removeMe.Add(entity);
                }

                // Update its position
                particleAttr.position += (particleAttr.direction * particleAttr.speed);

                //
                // Have it rotate proportional to its speed
                //particleAttr.rotation += particleAttr.speed / 50.0f;
                //
                // Apply some gravity
                //particleAttr.direction += this.Gravity;

            }

            // Remove any expired particles
            foreach (var entity in removeMe)
            {
                m_removeEntity(entity);
            }
        }

        public void renderParticles(SpriteBatch spriteBatch)
        {
            foreach (var entity in m_entities.Values)
            {
                var appearance = entity.GetComponent<Components.Appearance>();
                var particleAttr = entity.GetComponent<Components.Particle>();
                Rectangle area = new Rectangle();

                area.X = (int)particleAttr.position.X;
                area.Y = (int)particleAttr.position.Y;
                area.Width = (int)particleAttr.size.X;
                area.Height = (int)particleAttr.size.Y;

                spriteBatch.Draw(appearance.image, area, null,
                    appearance.fill, 0f, new Vector2(0, 0), SpriteEffects.None, 1f);
            }
        }

        public void objectIsWin(Texture2D square)
        {
            Color color = Color.Yellow;
            Vector2 position = new Vector2(500,500);
            Vector2 size = new Vector2(1, 1);
            float speed = 1;
            TimeSpan lifetime = new TimeSpan(0, 0, 0, 0, 1000);

            //
            // Generate particles at the specified rate
            //m_accumulated += gameTime.ElapsedGameTime;
            //while (m_accumulated > m_rate)
            //{
            //    m_accumulated -= m_rate;

            for (int i=0; i<10; i++)
            {
                Entity p = Particle.create(
                    square,
                    Color.Yellow,
                    position,
                    m_random.nextCircleVector(),
                    size,
                    (float)m_random.nextGaussian(speed, 1),
                    lifetime); // 1 second lifetime

                m_addEntity(p);
            }
        }

        public void playerDeath(Texture2D square, Point position)
        {
            Color color = Color.Yellow;
            Vector2 pos = new Vector2();
            Vector2 size = new Vector2(2, 2);
            float speed = 2.5f;
            TimeSpan lifetime = new TimeSpan(0, 0, 0, 0, 150);


            pos.X = (position.X * CELL_SIZE) + (CELL_SIZE / 2) + OFFSET_X;
            pos.Y = (position.Y * CELL_SIZE) + (CELL_SIZE / 2) + OFFSET_Y;

            //
            // Generate particles at the specified rate
            //m_accumulated += gameTime.ElapsedGameTime;
            //while (m_accumulated > m_rate)
            //{
            //    m_accumulated -= m_rate;

            for (int i = 0; i < 100; i++)
            {
                Entity p = Particle.create(
                    square,
                    Color.Yellow,
                    pos,
                    m_random.nextCircleVector(),
                    size,
                    (float)m_random.nextGaussian(speed, 1),
                    lifetime); // 1 second lifetime

                m_addEntity(p);
            }
        }
    }
}
