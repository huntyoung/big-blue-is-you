using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Systems
{
    class SetRules : System
    {
        public static List<string> previousRulesList;
        public static List<string> rulesList;

        private string[] nouns = { "baba", "flag", "lava", "rock", "wall", "water" };
        private string[] definitions = { "you", "win", "stop", "push", "sink", "kill" };
        private string[] verbs = { "is" };

        public SetRules()
            : base(
                  typeof(Components.Text)
                  )
        {
            previousRulesList = new List<string>();
            rulesList = new List<string>();
        }

        public override void Update(GameTime gameTime)
        {
            // reset rules list
            rulesList.Clear();
            foreach (var entity in m_entities.Values)
            {
                var text = entity.GetComponent<Components.Text>();
                var position = entity.GetComponent<Components.Position>();

                checkForRule(text, position, Components.DirectionEnum.Right);
                checkForRule(text, position, Components.DirectionEnum.Down);
            }
            Console.WriteLine("HI");
        }

        private void checkForRule(Components.Text text, Components.Position position, Components.DirectionEnum direction)
        {
            Components.Text firstText = text;
            if (!nouns.Contains(firstText.word)) return;

            Point secondPosition = getNextPosition(new Point(position.x, position.y), direction);
            Components.Text secondText = getTextAtPosition(secondPosition.X, secondPosition.Y);
            if (secondText != null && verbs.Contains(secondText.word))
            {
                Point thirdPosition = getNextPosition(secondPosition, direction);
                Components.Text thirdText = getTextAtPosition(thirdPosition.X, thirdPosition.Y);
                if (thirdText != null && (nouns.Contains(thirdText.word) || definitions.Contains(thirdText.word)))
                {
                    string rule = firstText.word + " " + secondText.word + " " + thirdText.word;
                    if (!rulesList.Contains(rule))
                    {
                        // rules are added if not already contained in rules list
                        rulesList.Add(rule);
                    }
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

        private Components.Text getTextAtPosition(int x, int y)
        {
            foreach (var entity in m_entities.Values)
            {
                var position = entity.GetComponent<Components.Position>();
                if (position.x == x && position.y == y)
                {
                    return entity.GetComponent<Components.Text>();
                }
            }

            return null;
        }
    }
}
