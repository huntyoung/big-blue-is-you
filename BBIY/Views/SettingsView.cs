using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace BBIY
{

    public class SettingsView : GameStateView
    {
        private KeyboardControlPersistance m_keyboardControlPersistance;
        private SpriteFont m_header;
        private SpriteFont m_subFont;
        private const string MESSAGE = "Change Keyboard Controls";

        private KeyboardControls m_loadedControls;
        private Keys m_currentUp;
        private Keys m_currentDown;
        private Keys m_currentLeft;
        private Keys m_currentRight;
        private Keys m_currentReset;
        private bool m_getOriginalControls;

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

        public SettingsView(KeyboardControlPersistance keyboardControlPersistance)
        {
            m_keyboardControlPersistance = keyboardControlPersistance;
        }

        public override void initializeSession()
        {
            this.WINDOW_WIDTH = m_graphics.PreferredBackBufferWidth;
            this.WINDOW_HEIGHT = m_graphics.PreferredBackBufferHeight;
            m_currentSelection = CurrentlySelectedEnum.Up;

            m_waitForKeyRelease = true;

            m_loadedControls = KeyboardControlPersistance.m_loadedControls;
            m_getOriginalControls = true;

            m_currentUp = Keys.W;
            m_currentDown = Keys.S;
            m_currentLeft = Keys.A;
            m_currentRight = Keys.D;
            m_currentReset = Keys.R;
        }


        public override void loadContent(ContentManager contentManager)
        {
            m_header = contentManager.Load<SpriteFont>("Fonts/menu");
            m_subFont = contentManager.Load<SpriteFont>("Fonts/sub-menu");
        }

        public override GameStateEnum processInput(GameTime gameTime)
        {
            if (m_loadedControls != null)
            {
                if (m_getOriginalControls)
                {
                    m_currentUp = m_loadedControls.moveUp;
                    m_currentDown = m_loadedControls.moveDown;
                    m_currentLeft = m_loadedControls.moveLeft;
                    m_currentRight = m_loadedControls.moveRight;
                    m_currentReset = m_loadedControls.reset;

                    m_getOriginalControls = false;
                }
            } 
            else
            {
                // keep trying for non-null value
                m_loadedControls = KeyboardControlPersistance.m_loadedControls;
            }

            if (!m_waitForKeyRelease)
            {
                if (Keyboard.GetState().GetPressedKeys().Length > 0)
                {
                    Keys pressedKey = Keyboard.GetState().GetPressedKeys()[0];
                    if (pressedKey == Keys.Escape)
                    {
                        m_keyboardControlPersistance.saveControls(m_currentUp, m_currentDown, m_currentLeft, m_currentRight, m_currentReset);
                        while (m_keyboardControlPersistance.isSaving()) { }
                        return GameStateEnum.MainMenu;
                    }

                    if (pressedKey == m_currentUp) m_currentUp = Keys.None;
                    if (pressedKey == m_currentDown) m_currentDown = Keys.None;
                    if (pressedKey == m_currentLeft) m_currentLeft = Keys.None;
                    if (pressedKey == m_currentRight) m_currentRight = Keys.None;
                    if (pressedKey == m_currentReset) m_currentReset = Keys.None;

                    switch (m_currentSelection)
                    {
                        case CurrentlySelectedEnum.Up:
                            m_currentUp = pressedKey;
                            break;
                        case CurrentlySelectedEnum.Down:
                            m_currentDown = pressedKey;
                            break;
                        case CurrentlySelectedEnum.Left:
                            m_currentLeft = pressedKey;
                            break;
                        case CurrentlySelectedEnum.Right:
                            m_currentRight = pressedKey;
                            break;
                        case CurrentlySelectedEnum.Reset:
                            m_currentReset = pressedKey;
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
                new Vector2(WINDOW_WIDTH / 2 - headerSize.X / 2, WINDOW_HEIGHT / 4 - headerSize.Y), Color.White);

            m_spriteBatch.DrawString(m_subFont, "Up: " + m_currentUp.ToString(),
                new Vector2(WINDOW_WIDTH / 2 - headerSize.X / 3, WINDOW_HEIGHT / 3), 
                m_currentSelection == CurrentlySelectedEnum.Up ? Color.Red : Color.White);
            m_spriteBatch.DrawString(m_subFont, "Down: " + m_currentDown.ToString(),
                new Vector2(WINDOW_WIDTH / 2 - headerSize.X / 3, (int)(WINDOW_HEIGHT / 2.3)),
                m_currentSelection == CurrentlySelectedEnum.Down ? Color.Red : Color.White);
            m_spriteBatch.DrawString(m_subFont, "Left: " + m_currentLeft.ToString(),
                new Vector2(WINDOW_WIDTH / 2 + headerSize.X / 6, WINDOW_HEIGHT / 3),
                m_currentSelection == CurrentlySelectedEnum.Left ? Color.Red : Color.White);
            m_spriteBatch.DrawString(m_subFont, "Right: " + m_currentRight.ToString(),
                new Vector2(WINDOW_WIDTH / 2 + headerSize.X / 6, (int)(WINDOW_HEIGHT / 2.3)),
                m_currentSelection == CurrentlySelectedEnum.Right ? Color.Red : Color.White);
            m_spriteBatch.DrawString(m_subFont, "Reset: " + m_currentReset.ToString(),
                new Vector2(WINDOW_WIDTH / 2 - m_subFont.MeasureString("Reset: " + m_currentReset.ToString()).X / 2, 
                (int)(WINDOW_HEIGHT / 1.8)),
                m_currentSelection == CurrentlySelectedEnum.Reset ? Color.Red : Color.White);

            m_spriteBatch.End();
        }

    }
}
