using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace BBIY
{

    public class SettingsView : GameStateView
    {
        private SpriteFont m_header;
        private SpriteFont m_subFont;
        private const string MESSAGE = "Change Keyboard Controls";

        private int WINDOW_WIDTH;
        private int WINDOW_HEIGHT;

        private CurrentlySelectedEnum m_currentSelection;
        private bool m_waitForKeyRelease;

        private enum CurrentlySelectedEnum
        {
            Up,
            Down,
            Left,
            Right,
            Reset
        }

        public override void initializeSession()
        {
            this.WINDOW_WIDTH = m_graphics.PreferredBackBufferWidth;
            this.WINDOW_HEIGHT = m_graphics.PreferredBackBufferHeight;
            m_currentSelection = CurrentlySelectedEnum.Up;

            m_waitForKeyRelease = true;
        }

        public override void loadContent(ContentManager contentManager)
        {
            m_header = contentManager.Load<SpriteFont>("Fonts/menu");
            m_subFont = contentManager.Load<SpriteFont>("Fonts/sub-menu");
        }

        public override GameStateEnum processInput(GameTime gameTime)
        {
            if (!m_waitForKeyRelease)
            {
                if (Keyboard.GetState().GetPressedKeys().Length > 0)
                {
                    Keys pressedKey = Keyboard.GetState().GetPressedKeys()[0];
                    if (pressedKey == Keys.Escape) return GameStateEnum.MainMenu;

                    if (pressedKey == KeyboardControls.moveUp) KeyboardControls.moveUp = Keys.None;
                    if (pressedKey == KeyboardControls.moveDown) KeyboardControls.moveDown = Keys.None;
                    if (pressedKey == KeyboardControls.moveLeft) KeyboardControls.moveLeft = Keys.None;
                    if (pressedKey == KeyboardControls.moveRight) KeyboardControls.moveRight = Keys.None;
                    if (pressedKey == KeyboardControls.reset) KeyboardControls.reset = Keys.None;

                    switch (m_currentSelection)
                    {
                        case CurrentlySelectedEnum.Up:
                            KeyboardControls.moveUp = pressedKey;
                            break;
                        case CurrentlySelectedEnum.Down:
                            KeyboardControls.moveDown = pressedKey;
                            break;
                        case CurrentlySelectedEnum.Left:
                            KeyboardControls.moveLeft = pressedKey;
                            break;
                        case CurrentlySelectedEnum.Right:
                            KeyboardControls.moveRight = pressedKey;
                            break;
                        case CurrentlySelectedEnum.Reset:
                            KeyboardControls.reset = pressedKey;
                            break;
                    }

                    if (m_currentSelection == CurrentlySelectedEnum.Reset) m_currentSelection = CurrentlySelectedEnum.Up;
                    else m_currentSelection++;

                    m_waitForKeyRelease = true;
                }
            }
            else if (Keyboard.GetState().GetPressedKeys().Length == 0)
            {
                m_waitForKeyRelease = false;
            }


            return GameStateEnum.Settings;
        }

        public override void update(GameTime gameTime)
        {
        }

        public override void render(GameTime gameTime)
        {
            m_spriteBatch.Begin();

            Vector2 headerSize = m_header.MeasureString(MESSAGE);
            m_spriteBatch.DrawString(m_header, MESSAGE,
                new Vector2(WINDOW_WIDTH / 2 - headerSize.X / 2, WINDOW_HEIGHT / 4 - headerSize.Y), Color.Black);

            m_spriteBatch.DrawString(m_subFont, "Up: " + KeyboardControls.moveUp.ToString(),
                new Vector2(WINDOW_WIDTH / 2 - headerSize.X / 3, WINDOW_HEIGHT / 3), 
                m_currentSelection == CurrentlySelectedEnum.Up ? Color.Red : Color.Black);
            m_spriteBatch.DrawString(m_subFont, "Down: " + KeyboardControls.moveDown.ToString(),
                new Vector2(WINDOW_WIDTH / 2 - headerSize.X / 3, (int)(WINDOW_HEIGHT / 2.3)),
                m_currentSelection == CurrentlySelectedEnum.Down ? Color.Red : Color.Black);
            m_spriteBatch.DrawString(m_subFont, "Left: " + KeyboardControls.moveLeft.ToString(),
                new Vector2(WINDOW_WIDTH / 2 + headerSize.X / 6, WINDOW_HEIGHT / 3),
                m_currentSelection == CurrentlySelectedEnum.Left ? Color.Red : Color.Black);
            m_spriteBatch.DrawString(m_subFont, "Right: " + KeyboardControls.moveRight.ToString(),
                new Vector2(WINDOW_WIDTH / 2 + headerSize.X / 6, (int)(WINDOW_HEIGHT / 2.3)),
                m_currentSelection == CurrentlySelectedEnum.Right ? Color.Red : Color.Black);
            m_spriteBatch.DrawString(m_subFont, "Reset: " + KeyboardControls.reset.ToString(),
                new Vector2(WINDOW_WIDTH / 2 - m_subFont.MeasureString("Reset: " + KeyboardControls.reset.ToString()).X / 2, 
                (int)(WINDOW_HEIGHT / 1.8)),
                m_currentSelection == CurrentlySelectedEnum.Reset ? Color.Red : Color.Black);

            m_spriteBatch.End();
        }

    }
}
