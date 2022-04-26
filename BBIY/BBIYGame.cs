﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace BBIY
{
    public class BBIYGame : Game
    {
        private GraphicsDeviceManager m_graphics;
        private IGameState m_currentState;
        private GameStateEnum m_nextStateEnum = GameStateEnum.MainMenu;
        private Dictionary<GameStateEnum, IGameState> m_states;

        private KeyboardControlPersistance m_keyboardControlPersistance;

        public BBIYGame()
        {
            m_graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            m_keyboardControlPersistance = new KeyboardControlPersistance();

            // Set window size preferences
            m_graphics.IsFullScreen = false;
            m_graphics.PreferredBackBufferWidth = 800;
            m_graphics.PreferredBackBufferHeight = 600;

            m_graphics.ApplyChanges();

            // Create all the game states here
            m_states = new Dictionary<GameStateEnum, IGameState>();
            m_states.Add(GameStateEnum.MainMenu, new MainMenuView(m_keyboardControlPersistance));
            m_states.Add(GameStateEnum.LevelSelect, new LevelSelectView());
            m_states.Add(GameStateEnum.GamePlay, new GamePlayView());
            m_states.Add(GameStateEnum.Settings, new SettingsView(m_keyboardControlPersistance));
            m_states.Add(GameStateEnum.Credits, new CreditsView());

            // We are starting with the main menu
            m_currentState = m_states[GameStateEnum.MainMenu];
            m_currentState.initializeSession();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Give all game states a chance to load their content
            foreach (var item in m_states)
            {
                item.Value.initialize(this.GraphicsDevice, m_graphics);
                item.Value.loadContent(this.Content);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            m_nextStateEnum = m_currentState.processInput(gameTime);

            // Special case for exiting the game
            if (m_nextStateEnum == GameStateEnum.Exit)
            {
                Exit();
            }

            m_currentState.update(gameTime);


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            m_currentState.render(gameTime);

            if (m_currentState != m_states[m_nextStateEnum])
            {
                m_currentState = m_states[m_nextStateEnum];
                m_currentState.initializeSession();
            }

            base.Draw(gameTime);
        }
    }
}
