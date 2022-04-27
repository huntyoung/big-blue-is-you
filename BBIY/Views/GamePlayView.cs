using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;


namespace BBIY
{
    public class GamePlayView : GameStateView
    {
        ContentManager m_content;
        private GameModel m_gameModel;

        private Keys m_resetKey;
        private bool m_resetAvailable;

        public override void initializeSession()
        {
            m_gameModel = new GameModel(m_graphics.PreferredBackBufferWidth, m_graphics.PreferredBackBufferHeight);
            m_gameModel.Initialize(m_content, m_spriteBatch);
        }

        public override void loadContent(ContentManager content)
        {
            m_content = content;
        }

        public override GameStateEnum processInput(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                SoundEffects.stopBackgroundMusic();
                return GameStateEnum.MainMenu;
            }

            if (KeyboardControlPersistance.m_loadedControls == null) m_resetKey = Keys.R;
            else m_resetKey = KeyboardControlPersistance.m_loadedControls.reset;

            if (Keyboard.GetState().IsKeyDown(m_resetKey) && m_resetAvailable)
            {
                initializeSession();
                m_resetAvailable = false;
            }
            if (Keyboard.GetState().IsKeyUp(m_resetKey)) m_resetAvailable = true;

            return GameStateEnum.GamePlay;
        }

        public override void render(GameTime gameTime)
        {
            m_gameModel.Draw(gameTime);
        }

        public override void update(GameTime gameTime)
        {
            m_gameModel.Update(gameTime);
        }
    }
}
