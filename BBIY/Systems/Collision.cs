using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Systems
{
    class Collision : System
    {
        public Collision()
            : base(
                  typeof(Components.Position)
                  )
        {
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entity in m_entities.Values)
            {
                var position = entity.GetComponent<Components.Position>();
                if (entity.ContainsComponent<Components.IsYou>())
                {
                    var you = entity.GetComponent<Components.IsYou>();
                    if (you.lastMove != Components.DirectionEnum.Stopped)
                    {
                        Point nextPosition = getNextPosition(position, you.lastMove);
                        //Debug.WriteLine("You: " + nextPosition.X.ToString() + ", " + nextPosition.Y.ToString());
                        List<Entities.Entity> collidedEntities = getEntitiesAtPosition(nextPosition.X, nextPosition.Y);
                        handleCollision(collidedEntities, you);
                    }
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

        private void handleCollision(List<Entities.Entity> entities, Components.IsYou you)
        {
            foreach (var entity in entities)
            {
                if (entity.ContainsComponent<Components.IsStop>())
                {
                    //Debug.WriteLine("Colided: " + entity.GetComponent<Components.Position>().x.ToString() + ", " + entity.GetComponent<Components.Position>().y.ToString());
                    collisionIsStop(you);
                }
                else if (entity.ContainsComponent<Components.IsPush>())
                {
                    collisionIsPush(entities, you);
                }
            }
        }

        private void collisionIsStop(Components.IsYou you)
        {
            you.lastMove = Components.DirectionEnum.Stopped;
        }

        private void collisionIsPush(List<Entities.Entity> entities, Components.IsYou you)
        {
            bool didPush = collisionIsPushHelper(entities, you);
            if (!didPush) collisionIsStop(you);
        }
        private bool collisionIsPushHelper(List<Entities.Entity> entities, Components.IsYou you)
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
            Point nextPosition = getNextPosition(position, you.lastMove);
            List<Entities.Entity> nextPositionEntities = getEntitiesAtPosition(nextPosition.X, nextPosition.Y);

            bool continuePushing = collisionIsPushHelper(nextPositionEntities, you);
            if (continuePushing)
            {
                foreach (var entity in entitiesToPush)
                {
                    var isPush = entity.GetComponent<Components.IsPush>();
                    isPush.lastPush = you.lastMove;
                }
            }

            return continuePushing;
        }

        private void collisionIsKill()
        {
        }
        private void collisionIsSink()
        {

        }
    }
}
