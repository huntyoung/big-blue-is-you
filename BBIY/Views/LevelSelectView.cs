using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace BBIY
{
    class LevelSelectView : GameStateView
    {
        private SpriteFont m_fontMenu;
        private SpriteFont m_fontMenuSelect;

        private List<string[]> m_levels;
        private int m_numberOfLevels;
        private int m_levelSelected;

        private bool m_waitForKeyRelease;

        public override void initializeSession()
        {
            m_levelSelected = 1;
            m_waitForKeyRelease = true;
        }

        public override void loadContent(ContentManager contentManager)
        {
            m_fontMenu = contentManager.Load<SpriteFont>("Fonts/menu");
            m_fontMenuSelect = contentManager.Load<SpriteFont>("Fonts/menu-select");

            var enviroment = System.Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(enviroment).Parent.Parent.FullName; // returns path to BBIY/BBIY folder
            string levelsPath = Path.Combine(projectDirectory, @"levels-all.bbiy");

            string[] allLevels = File.ReadAllLines(levelsPath);
            m_levels = separateLevels(allLevels);
            m_numberOfLevels = m_levels.Count;
        }
        public override GameStateEnum processInput(GameTime gameTime)
        {
            // This is the technique I'm using to ensure one keypress makes one menu navigation move
            if (!m_waitForKeyRelease)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Escape)) return GameStateEnum.MainMenu;

                // Arrow keys/WASD to navigate the menu
                if (Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    if (m_levelSelected == m_numberOfLevels) m_levelSelected = 1;
                    else m_levelSelected++;

                    m_waitForKeyRelease = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    if (m_levelSelected == 1) m_levelSelected = m_numberOfLevels;
                    else m_levelSelected--;

                    m_waitForKeyRelease = true;
                }

                // If enter is pressed, return the appropriate new state
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    GameModel.m_currentLevel = m_levels[m_levelSelected - 1];

                    return GameStateEnum.GamePlay;
                }
            }
            else if (Keyboard.GetState().GetPressedKeys().Length == 0)
            {
                m_waitForKeyRelease = false;
            }

            return GameStateEnum.LevelSelect;
        }
        public override void update(GameTime gameTime)
        {
        }
        public override void render(GameTime gameTime)
        {
            m_spriteBatch.Begin();

            // I split the first one's parameters on separate lines to help you see them better
            float bottom = 100;
            for (int i = 1; i <= m_numberOfLevels; i++)
            {
                bottom = drawLevelMenuItem(
                    m_levelSelected == i ? m_fontMenuSelect : m_fontMenu,
                    m_levels[i - 1][0],
                    bottom,
                    m_levelSelected == i ? Color.Yellow : Color.Blue
                );
            }

            m_spriteBatch.End();
        }

        private float drawLevelMenuItem(SpriteFont font, string text, float y, Color color)
        {
            Vector2 stringSize = font.MeasureString(text);
            m_spriteBatch.DrawString(
                font,
                text,
                new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize.X / 2, y),
                color);

            return y + stringSize.Y;
        }

        private List<string[]> separateLevels(string[] allLevels)
        {
            List<string[]> levels = new List<string[]>();

            int startingLine = 0;
            int endingLine;
            for (int i = 1; i < allLevels.Length; i++)
            {
                if (allLevels[i].StartsWith("Level") || i == allLevels.Length - 1) 
                {
                    if (i == allLevels.Length - 1) endingLine = i + 1;
                    else endingLine = i;

                    levels.Add(allLevels[startingLine..endingLine]);

                    startingLine = i;
                }
            }

            return levels;
        }
    }
}
