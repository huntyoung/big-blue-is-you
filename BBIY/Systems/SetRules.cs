using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Systems
{
    class SetRules : System
    {
        private List<string> rulesList = new List<string>();
        public SetRules()
            : base(
                  typeof(Components.Text)
                  )
        {
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entity in m_entities.Values)
            {
                var text = entity.GetComponent<Components.Text>();
                var position = entity.GetComponent<Components.Position>();

                checkForRule(text, position, Components.DirectionEnum.Right);
                checkForRule(text, position, Components.DirectionEnum.Down);
            }
        }

        private void checkForRule(Components.Text text, Components.Position position, Components.DirectionEnum direction)
        {
            string firstWord = text.word;
            if (firstWord == "is") return;

            Point secondPosition = getNextPosition(new Point(position.x, position.y), direction);
            string secondWord = getWordAtPosition(secondPosition.X, secondPosition.Y);
            if (secondWord == "is")
            {
                Point thirdPosition = getNextPosition(secondPosition, direction);
                string thirdWord = getWordAtPosition(thirdPosition.X, thirdPosition.Y);
                if (thirdWord != "is" && thirdWord != "")
                {
                    rulesList.Add(firstWord + " " + secondWord + " " + thirdWord);
                }
            }
        }

        private Point getNextPosition(Point position, Components.DirectionEnum direction)
        {
            switch (direction)
            {
                case Components.DirectionEnum.Up:
                    return new Point(position.X, position.Y - 1);
                case Components.DirectionEnum.Down:
                    return new Point(position.X, position.Y + 1);
                case Components.DirectionEnum.Left:
                    return new Point(position.X - 1, position.Y);
                case Components.DirectionEnum.Right:
                    return new Point(position.X + 1, position.Y);
                default:
                    return new Point(position.X, position.Y);
            }
        }

        private string getWordAtPosition(int x, int y)
        {
            foreach (var entity in m_entities.Values)
            {
                var position = entity.GetComponent<Components.Position>();
                if (position.x == x && position.y == y)
                {
                    var text = entity.GetComponent<Components.Text>();
                    return text.word;
                }
            }

            return "";
        }
    }
}
