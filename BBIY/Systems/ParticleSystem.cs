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

        private readonly int WINDOW_WIDTH;
        private readonly int WINDOW_HEIGHT;
        private readonly int CELL_SIZE;
        private readonly int OFFSET_X;
        private readonly int OFFSET_Y;

        private BBIY.MyRandom m_random = new BBIY.MyRandom();

        public ParticleSystem(Action<Entity> addEntity, Action<Entity> removeEntity, int screenWidth, int screenHeight, int gridWidth, int gridHeight) 
            : base(typeof(Components.Particle))
        {
            WINDOW_WIDTH = screenWidth;
            WINDOW_HEIGHT = screenHeight;

            m_addEntity = addEntity;
            m_removeEntity = removeEntity;
            CELL_SIZE = screenHeight / gridHeight;
            OFFSET_X = (screenWidth - gridWidth * CELL_SIZE) / 2;
            OFFSET_Y = (screenHeight - gridHeight * CELL_SIZE) / 2;
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

        public void objectEmphasizedParticles(Texture2D square, Point objectposition)
        {
            Color color = Color.LightYellow;
            Vector2 pos = new Vector2();
            Vector2 size = new Vector2(1, 1);
            float speed = 0.5f;
            TimeSpan lifetime = new TimeSpan(0, 0, 0, 0, 500);

            pos.X = (objectposition.X * CELL_SIZE) + OFFSET_X + 4;
            pos.Y = (objectposition.Y * CELL_SIZE) + CELL_SIZE + OFFSET_Y;

            float distanceToMove = (CELL_SIZE / 8) - 0.5f;
            double doubleNum = 0.65;
            for (int i=0; i<24; i++)
            {
                float angle = (float)(doubleNum * 2.0 * Math.PI);
                float circleX = (float)Math.Cos(angle);
                float circleY = (float)Math.Sin(angle);

                Entity p = Particle.create(
                    square,
                    color,
                    pos,
                    new Vector2(circleX, circleY),
                    size,
                    (float)m_random.nextGaussian(speed, 0.1f),
                    lifetime);

                m_addEntity(p);

                if (i % 8 == 0 && i != 0) doubleNum += 0.1;

                if (i >= 0 && i < 8)
                {
                    pos.Y -= distanceToMove;
                }
                else if (i >= 8 && i < 16)
                {
                    pos.X += distanceToMove;
                }
                else if (i >= 16 && i < 24)
                {
                    pos.Y += distanceToMove;
                }
            }
        }

        public void destroyedEntityParticles(Texture2D square, Point position)
        {
            Color color = Color.Yellow;
            Vector2 pos = new Vector2();
            Vector2 size = new Vector2(2, 2);
            float speed = 2f;
            TimeSpan lifetime = new TimeSpan(0, 0, 0, 0, 200);


            pos.X = (position.X * CELL_SIZE) + (CELL_SIZE / 2) + OFFSET_X;
            pos.Y = (position.Y * CELL_SIZE) + (CELL_SIZE / 2) + OFFSET_Y;

            while (speed > 0.5)
            {
                for (int i = 0; i < 50; i++)
                {
                    Entity p = Particle.create(
                        square,
                        Color.Yellow,
                        pos,
                        m_random.nextCircleVector(),
                        size,
                        //(float)m_random.nextGaussian(speed, 1),
                        speed,
                        lifetime); // 1 second lifetime

                    m_addEntity(p);
                }
                speed -= 0.2f;
            }
        }

        private TimeSpan levelWonParticleRate = new TimeSpan(0, 0, 0, 0, 300);
        private TimeSpan levelWonAccumulatedTime = TimeSpan.Zero;
        private double doubleNum = 0;
        private bool colorSwitch = false;

        public void levelWonParticles(GameTime gameTime, Texture2D square, Point winnerPosition)
        {
            levelWonAccumulatedTime += gameTime.ElapsedGameTime;

            Vector2 pos = new Vector2();
            Vector2 size = new Vector2(4, 4);
            float speed = 2;
            TimeSpan lifetime = new TimeSpan(0, 0, 0, 2, 0);

            float centerX = (winnerPosition.X * CELL_SIZE) + (CELL_SIZE / 2) + OFFSET_X;
            float centerY = (winnerPosition.Y * CELL_SIZE) + (CELL_SIZE / 2) + OFFSET_Y;

            while (levelWonAccumulatedTime > levelWonParticleRate)
            {
                levelWonAccumulatedTime -= levelWonParticleRate;

                do
                {
                    float angle = (float)(doubleNum * 2.0 * Math.PI);
                    float circleX = (float)Math.Cos(angle);
                    float circleY = (float)Math.Sin(angle);

                    pos.X = centerX + ((CELL_SIZE / 2) * circleX); //center + (radius * angle)
                    pos.Y = centerY + ((CELL_SIZE / 2) * circleY); //center + (radius * angle)

                    Entity p = Particle.create(
                        square,
                        colorSwitch ? Color.Yellow : Color.Orange,
                        pos,
                        new Vector2(circleX, circleY),
                        size,
                        speed,
                        lifetime); // 1 second lifetime

                    m_addEntity(p);

                    if (doubleNum < 1) doubleNum += 0.025;
                    else doubleNum = 0;

                    colorSwitch = !colorSwitch;
                } while (doubleNum > 0);
            }
        }
    }
}
