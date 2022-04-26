using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Systems
{
    class Collision : System
    {

        private Action<Entities.Entity> m_addEntity;
        private Action<Entities.Entity> m_removeEntity;
        private Action<Point> m_playerDeathParticles;

        public Collision(Action<Entities.Entity> addEntity, Action<Entities.Entity> removeEntity, Action<Point> playerDeathParticles) : base(typeof(Components.Position))
        {
            m_addEntity = addEntity;
            m_removeEntity = removeEntity;
            m_playerDeathParticles = playerDeathParticles;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entity in m_entities.Values)
            {
                var position = entity.GetComponent<Components.Position>();
                List<Entities.Entity> collidedEntities;
                if (entity.ContainsComponent<Components.IsYou>())
                {
                    var you = entity.GetComponent<Components.IsYou>();
                    if (you.lastMove != Components.DirectionEnum.Stopped)
                    {
                        Point nextPosition = getNextPosition(position, you.lastMove);
                        //Debug.WriteLine("You: " + nextPosition.X.ToString() + ", " + nextPosition.Y.ToString());
                        collidedEntities = getEntitiesAtPosition(nextPosition.X, nextPosition.Y);
                        handleYouCollisionAtNextPosition(collidedEntities, entity);
                    }
                    collidedEntities = getEntitiesAtPosition(position.x, position.y);
                    handleYouCollisionAtCurrentPosition(collidedEntities, entity);
                }
                else if (entity.ContainsComponent<Components.IsPush>() && !entity.ContainsComponent<Components.Text>())
                {
                    collidedEntities = getEntitiesAtPosition(position.x, position.y);
                    pushableEntitySink(collidedEntities, entity);
                }
            }
        }

        private Point getNextPosition(Components.Position position, Components.DirectionEnum lastMove)
        {
            switch (lastMove)
            {
                case Components.DirectionEnum.Up:
                    return new Point(position.x, position.y - 1);
                case Components.DirectionEnum.Down:
                    return new Point(position.x, position.y + 1);
                case Components.DirectionEnum.Left:
                    return new Point(position.x - 1, position.y);
                case Components.DirectionEnum.Right:
                    return new Point(position.x + 1, position.y);
                default:
                    return new Point(position.x, position.y);
            }
        }

        private List<Entities.Entity> getEntitiesAtPosition(int x, int y)
        {
            List<Entities.Entity> entities = new List<Entities.Entity>();

            foreach (var entity in m_entities.Values)
            {
                var position = entity.GetComponent<Components.Position>();
                if (position.x == x && position.y == y)
                {
                    entities.Add(entity);
                }
            }

            return entities;
        }

        private void handleYouCollisionAtNextPosition(List<Entities.Entity> entities, Entities.Entity youEntity)
        {
            var youComponent = youEntity.GetComponent<Components.IsYou>();
            foreach (var entity in entities)
            {
                if (entity.ContainsComponent<Components.IsStop>())
                {
                    collisionIsStop(youComponent);
                }
                else if (entity.ContainsComponent<Components.IsPush>())
                {
                    collisionIsPush(entities, youComponent);
                }
            }
        }

        private void handleYouCollisionAtCurrentPosition(List<Entities.Entity> entities, Entities.Entity youEntity)
        {
            var youComponent = youEntity.GetComponent<Components.IsYou>();
            foreach (var entity in entities)
            {
                if (entity.ContainsComponent<Components.IsSink>())
                {
                    collisionIsSink(entity, youEntity);
                }
                else if (entity.ContainsComponent<Components.IsKill>())
                {
                    collisionIsKill(youEntity);
                }
                else if (entity.ContainsComponent<Components.IsWin>())
                {
                    collisionIsWin();
                    youComponent.lastMove = Components.DirectionEnum.Stopped;
                }
            }
        }

        private void collisionIsStop(Components.IsYou youComponent)
        {
            youComponent.lastMove = Components.DirectionEnum.Stopped;
        }

        private void collisionIsPush(List<Entities.Entity> entities, Components.IsYou youComponent)
        {
            bool didPush = collisionIsPushHelper(entities, youComponent);
            if (!didPush) collisionIsStop(youComponent);
        }
        private bool collisionIsPushHelper(List<Entities.Entity> entities, Components.IsYou youComponent)
        {
            List<Entities.Entity> entitiesToPush = new List<Entities.Entity>();

            bool entityIsPush = false;
            foreach (var entity in entities)
            {
                if (entity.ContainsComponent<Components.IsStop>()) return false;
                else if (entity.ContainsComponent<Components.IsPush>())
                {
                    entitiesToPush.Add(entity);
                    entityIsPush = true;
                }
            }
            if (!entityIsPush) return true;

            var position = entities[0].GetComponent<Components.Position>();
            Point nextPosition = getNextPosition(position, youComponent.lastMove);
            List<Entities.Entity> nextPositionEntities = getEntitiesAtPosition(nextPosition.X, nextPosition.Y);

            bool continuePushing = collisionIsPushHelper(nextPositionEntities, youComponent);
            if (continuePushing)
            {
                foreach (var entity in entitiesToPush)
                {
                    var isPush = entity.GetComponent<Components.IsPush>();
                    isPush.lastPush = youComponent.lastMove;
                }
            }

            return continuePushing;
        }

        private void collisionIsSink(Entities.Entity sinkEntity, Entities.Entity sunkEntity)
        {
            m_removeEntity(sinkEntity);
            m_removeEntity(sunkEntity);
        }

        private void collisionIsKill(Entities.Entity youEntity)
        {
            var position = youEntity.GetComponent<Components.Position>();
            var youComponent = youEntity.GetComponent<Components.IsYou>();

            m_removeEntity(youEntity);
            m_playerDeathParticles(new Point(position.x, position.y));
        }

        private void collisionIsWin()
        {

        }

        private void pushableEntitySink(List<Entities.Entity> entities, Entities.Entity pushableEntity)
        {
            foreach (var entity in entities)
            {
                if (entity.ContainsComponent<Components.IsSink>())
                {
                    collisionIsSink(entity, pushableEntity);
                }
            }
        }
    }
}
