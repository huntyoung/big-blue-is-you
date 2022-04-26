using Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace BBIY
{
    class GameModel
    {
        private SpriteBatch m_spriteBatch;

        public static string[] m_currentLevel { get; set; }
        public static int m_gridSize { get; set; }

        private const int GRID_SIZE = 20;
        private readonly int WINDOW_WIDTH;
        private readonly int WINDOW_HEIGHT;
        private readonly int CELL_SIZE;

        private Systems.Renderer m_sysRenderer;
        private Systems.KeyboardInput m_sysKeyboardInput;
        private Systems.Animate m_sysAnimate;
        private Systems.Collision m_sysCollision;
        private Systems.Movement m_sysMovement;
        private Systems.ParticleSystem m_sysParticleSystem;
        private Systems.SetRules m_sysRules;
        private Systems.AssignComponents m_sysAssignComponents;

        private Texture2D m_square;
        private Texture2D m_bigBlue;
        private Texture2D m_flag;
        private Texture2D m_floor;
        private Texture2D m_grass;
        private Texture2D m_hedge;
        private Texture2D m_lava;
        private Texture2D m_rock;
        private Texture2D m_wall;
        private Texture2D m_water;
        private Texture2D m_wordBaba;
        private Texture2D m_wordFlag;
        private Texture2D m_wordIs;
        private Texture2D m_wordKill;
        private Texture2D m_wordLava;
        private Texture2D m_wordPush;
        private Texture2D m_wordRock;
        private Texture2D m_wordSink;
        private Texture2D m_wordStop;
        private Texture2D m_wordWall;
        private Texture2D m_wordWater;
        private Texture2D m_wordWin;
        private Texture2D m_wordYou;

        public GameModel(int width, int height)
        {
            this.WINDOW_WIDTH = width;
            this.WINDOW_HEIGHT = height;
            this.CELL_SIZE = height / GRID_SIZE;
        }

        public void Initialize(ContentManager content, SpriteBatch spriteBatch)
        {
            loadContent(content);
            m_spriteBatch = spriteBatch;

            m_sysRenderer = new Systems.Renderer(m_spriteBatch, m_square, WINDOW_WIDTH, WINDOW_HEIGHT, GRID_SIZE);
            m_sysKeyboardInput = new Systems.KeyboardInput();
            m_sysAnimate = new Systems.Animate();
            m_sysCollision = new Systems.Collision((entity) => {
                m_sysAssignComponents.addToEntityList(entity);
                AddEntity(entity);
            }, (entity) => {
                m_sysAssignComponents.removeFromEntityList(entity);
                RemoveEntity(entity);
            }, playerDeathParticles);
            m_sysMovement = new Systems.Movement();
            m_sysParticleSystem = new Systems.ParticleSystem(AddEntity, RemoveEntity, WINDOW_WIDTH, WINDOW_HEIGHT, GRID_SIZE);
            m_sysRules = new Systems.SetRules();
            m_sysAssignComponents = new Systems.AssignComponents(AddEntity, RemoveEntity, checkAndAdd);

            initializeLevel();
        }

        public void Update(GameTime gameTime)
        {
            m_sysRules.Update(gameTime);
            m_sysAssignComponents.Update(gameTime);
            m_sysKeyboardInput.Update(gameTime);
            m_sysCollision.Update(gameTime);
            m_sysMovement.Update(gameTime);
            m_sysParticleSystem.Update(gameTime);
            m_sysAnimate.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            m_spriteBatch.Begin(SpriteSortMode.FrontToBack);

            m_sysRenderer.Update(gameTime);
            m_sysParticleSystem.renderParticles(m_spriteBatch);

            m_spriteBatch.End();
        }

        private void AddEntity(Entity entity)
        {
            m_sysKeyboardInput.Add(entity);
            m_sysRenderer.Add(entity);
            m_sysAnimate.Add(entity);
            m_sysCollision.Add(entity);
            m_sysMovement.Add(entity);
            m_sysParticleSystem.Add(entity);
            m_sysRules.Add(entity);
            m_sysAssignComponents.Add(entity);
        }

        private void RemoveEntity(Entity entity)
        {
            m_sysKeyboardInput.Remove(entity.Id);
            m_sysRenderer.Remove(entity.Id);
            m_sysAnimate.Remove(entity.Id);
            m_sysCollision.Remove(entity.Id);
            m_sysMovement.Remove(entity.Id);
            m_sysParticleSystem.Remove(entity.Id);
            m_sysRules.Remove(entity.Id);
            m_sysAssignComponents.Remove(entity.Id);
        }

        private void playerDeathParticles(Point position)
        {
            m_sysParticleSystem.playerDeath(m_square, position);
        }

        private void loadContent(ContentManager content)
        {
            m_square = content.Load<Texture2D>("Images/square");

            m_bigBlue = content.Load<Texture2D>("Images/Objects/BigBlue");
            m_flag = content.Load<Texture2D>("Images/Objects/flag");
            m_floor = content.Load<Texture2D>("Images/Objects/floor");
            m_grass = content.Load<Texture2D>("Images/Objects/grass");
            m_hedge = content.Load<Texture2D>("Images/Objects/hedge");
            m_lava = content.Load<Texture2D>("Images/Objects/lava");
            m_rock = content.Load<Texture2D>("Images/Objects/rock");
            m_wall = content.Load<Texture2D>("Images/Objects/wall");
            m_water = content.Load<Texture2D>("Images/Objects/water");

            m_wordBaba = content.Load<Texture2D>("Images/Words/word-baba");
            m_wordFlag = content.Load<Texture2D>("Images/Words/word-flag");
            m_wordIs = content.Load<Texture2D>("Images/Words/word-is");
            m_wordKill = content.Load<Texture2D>("Images/Words/word-kill");
            m_wordLava = content.Load<Texture2D>("Images/Words/word-lava");
            m_wordPush = content.Load<Texture2D>("Images/Words/word-push");
            m_wordRock = content.Load<Texture2D>("Images/Words/word-rock");
            m_wordSink = content.Load<Texture2D>("Images/Words/word-sink");
            m_wordStop = content.Load<Texture2D>("Images/Words/word-stop");
            m_wordWall = content.Load<Texture2D>("Images/Words/word-wall");
            m_wordWater = content.Load<Texture2D>("Images/Words/word-water");
            m_wordWin = content.Load<Texture2D>("Images/Words/word-win");
            m_wordYou = content.Load<Texture2D>("Images/Words/word-you");
        }

        private void initializeLevel()
        {
            // the game area starts on row 2 in the text
            string[] gameLayer1 = m_currentLevel[2..(2 + (GRID_SIZE + 1))];
            string[] gameLayer2 = m_currentLevel[(2 + GRID_SIZE)..m_currentLevel.Length];

            for (int row = 0; row < gameLayer1.Length; row++)
            {
                for (int col = 0; col < gameLayer1[row].Length; col++)
                {
                    checkAndAdd(gameLayer1[row][col], col, row);
                }
            }

            for (int row = 0; row < gameLayer2.Length; row++)
            {
                for (int col = 0; col < gameLayer2[row].Length; col++)
                {
                    checkAndAdd(gameLayer2[row][col], col, row);
                }
            }
        }

        private void checkAndAdd(char letter, int x, int y)
        {
            Entity entity;
            switch (letter)
            {
                case 'w':
                    entity = Wall.create(m_wall, x, y);
                    break;
                case 'r':
                    entity = Rock.create(m_rock, x, y);
                    break;
                case 'f':
                    entity = Flag.create(m_flag, x, y);
                    break;
                case 'b':
                    entity = BigBlue.create(m_bigBlue, x, y);
                    break;
                case 'l':
                    entity = Floor.create(m_floor, x, y);
                    break;
                case 'g':
                    entity = Grass.create(m_grass, x, y);
                    break;
                case 'a':
                    entity = Water.create(m_water, x, y);
                    break;
                case 'v':
                    entity = Lava.create(m_lava, x, y);
                    break;
                case 'h':
                    entity = Hedge.create(m_hedge, x, y);
                    break;
                case 'W':
                    entity = Text.create(m_wordWall, "wall", x, y, new Color(41, 49, 65));
                    break;
                case 'R':
                    entity = Text.create(m_wordRock, "rock", x, y, new Color(144, 103, 62));
                    break;
                case 'F':
                    entity = Text.create(m_wordFlag, "flag", x, y, new Color(237, 226, 133));
                    break;
                case 'B':
                    entity = Text.create(m_wordBaba, "baba", x, y, new Color(217, 57, 106));
                    break;
                case 'I':
                    entity = Text.create(m_wordIs, "is", x, y, Color.White);
                    break;
                case 'S':
                    entity = Text.create(m_wordStop, "stop", x, y, new Color(75, 92, 28));
                    break;
                case 'P':
                    entity = Text.create(m_wordPush, "push", x, y, new Color(144, 103, 62));
                    break;
                case 'V':
                    entity = Text.create(m_wordLava, "lava", x, y, new Color(130, 38, 28));
                    break;
                case 'A':
                    entity = Text.create(m_wordWater, "water", x, y, new Color(95, 157, 209));
                    break;
                case 'Y':
                    entity = Text.create(m_wordYou, "you", x, y, new Color(217, 57, 106));
                    break;
                case 'X':
                    entity = Text.create(m_wordWin, "win", x, y, new Color(237, 226, 133));
                    break;
                case 'N':
                    entity = Text.create(m_wordSink, "sink", x, y, new Color(95, 157, 209));
                    break;
                case 'K':
                    entity = Text.create(m_wordKill, "kill", x, y, new Color(130, 38, 28));
                    break;
                default:
                    return;
            }

            // add to entity list to assign additional components in AssignComponents
            m_sysAssignComponents.addToEntityList(entity);
            AddEntity(entity);
        }
    }
}
