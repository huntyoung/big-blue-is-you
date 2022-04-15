using Microsoft.Xna.Framework;
using System;

namespace Systems
{
    /// <summary>
    /// This system is responsible for handling the movement of any
    /// entity with a movable & position components.
    /// </summary>
    class Movement : System
    {
        public Movement()
            : base(typeof(Components.Position))
        {
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entity in m_entities.Values)
            {
                moveEntity(entity, gameTime);
            }
        }

        private void moveEntity(Entities.Entity entity, GameTime gameTime)
        {
            Components.DirectionEnum moveDirection = Components.DirectionEnum.Stopped;
            if (entity.ContainsComponent<Components.IsYou>())
            {
                moveDirection = entity.GetComponent<Components.IsYou>().lastMove;
                entity.GetComponent<Components.Position>().layerDepth = 1f; // all movable entities (you and push) get put on top layer
            }
            else if (entity.ContainsComponent<Components.IsPush>())
            {
                moveDirection = entity.GetComponent<Components.IsPush>().lastPush;
                entity.GetComponent<Components.Position>().layerDepth = 1f; // all movable entities (you and push) get put on top layer
            }
            else if (entity.ContainsComponent<Components.Background>())
            {
                entity.GetComponent<Components.Position>().layerDepth = 0f; // all background entities get put on top layer
            }
            else
            {
                entity.GetComponent<Components.Position>().layerDepth = 0.5f; // everything else is on the neutral layer
            }

            switch (moveDirection)
            {
                case Components.DirectionEnum.Up:
                    move(entity, 0, -1);
                    break;
                case Components.DirectionEnum.Down:
                    move(entity, 0, 1);
                    break;
                case Components.DirectionEnum.Left:
                    move(entity, -1, 0);
                    break;
                case Components.DirectionEnum.Right:
                    move(entity, 1, 0);
                    break;
            }

            if (entity.ContainsComponent<Components.IsYou>())
                entity.GetComponent<Components.IsYou>().lastMove = Components.DirectionEnum.Stopped;
            else if (entity.ContainsComponent<Components.IsPush>())
                entity.GetComponent<Components.IsPush>().lastPush = Components.DirectionEnum.Stopped;
        }

        private void move(Entities.Entity entity, int xIncrement, int yIncrement)
        {
            var position = entity.GetComponent<Components.Position>();

            position.x += xIncrement;
            position.y += yIncrement;
        }
    }
}
