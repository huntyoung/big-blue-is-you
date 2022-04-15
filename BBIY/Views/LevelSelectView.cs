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

        List<string[]> levels;

        private enum LevelState
        {
            Level1,
            Level2,
            Level3,
            Level4,
            Level5
        }

        private LevelState m_currentSelection;
        private bool m_waitForKeyRelease;

        public override void initializeSession()
        {
            m_currentSelection = LevelState.Level1;
            m_waitForKeyRelease = true;
        }

        public override void loadContent(ContentManager contentManager)
        {
            m_fontMenu = contentManager.Load<SpriteFont>("Fonts/menu");
            m_fontMenuSelect = contentManager.Load<SpriteFont>("Fonts/menu-select");

            var enviroment = System.Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(enviroment).Parent.Parent.FullName; // returns path to BBIY/BBIY folder
            string levelsPath = Path.Combine(projectDirectory, @"levels-all.bbiy");

            string[] allLevels = System.IO.File.ReadAllLines(levelsPath);
            this.levels = separateLevels(allLevels);
        }
        public override GameStateEnum processInput(GameTime gameTime)
        {
            // This is the technique I'm using to ensure one keypress makes one menu navigation move
            if (!m_waitForKeyRelease)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Escape)) return GameStateEnum.MainMenu;

                // Arrow keys to navigate the menu
                if (Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    if (m_currentSelection == LevelState.Level5) m_currentSelection = LevelState.Level1;
                    else m_currentSelection++;

                    m_waitForKeyRelease = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    if (m_currentSelection == LevelState.Level1) m_currentSelection = LevelState.Level5;
                    else m_currentSelection--;

                    m_waitForKeyRelease = true;
                }

                // If enter is pressed, return the appropriate new state
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    switch (m_currentSelection)
                    {
                        case LevelState.Level1:
                            GameModel.m_currentLevel = levels[0];
                            break;
                        case LevelState.Level2:
                            GameModel.m_currentLevel = levels[1];
                            break;
                        case LevelState.Level3:
                            GameModel.m_currentLevel = levels[2];
                            break;
                        case LevelState.Level4:
                            GameModel.m_currentLevel = levels[3];
                            break;
                        case LevelState.Level5:
                            GameModel.m_currentLevel = levels[4];
                            break;
                    }
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
            float bottom = drawLevelMenuItem(
                m_currentSelection == LevelState.Level1 ? m_fontMenuSelect : m_fontMenu,
                "Level 1",
                200,
                m_currentSelection == LevelState.Level1 ? Color.Yellow : Color.Blue);
            bottom = drawLevelMenuItem(m_currentSelection == LevelState.Level2 ? m_fontMenuSelect : m_fontMenu, "Level 2", bottom, m_currentSelection == LevelState.Level2 ? Color.Yellow : Color.Blue);
            bottom = drawLevelMenuItem(m_currentSelection == LevelState.Level3 ? m_fontMenuSelect : m_fontMenu, "Level 3", bottom, m_currentSelection == LevelState.Level3 ? Color.Yellow : Color.Blue);
            bottom = drawLevelMenuItem(m_currentSelection == LevelState.Level4 ? m_fontMenuSelect : m_fontMenu, "Level 4", bottom, m_currentSelection == LevelState.Level4 ? Color.Yellow : Color.Blue);
            drawLevelMenuItem(m_currentSelection == LevelState.Level5 ? m_fontMenuSelect : m_fontMenu, "Level 5", bottom, m_currentSelection == LevelState.Level5 ? Color.Yellow : Color.Blue);

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
