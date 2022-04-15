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
        public static string[] m_currentLevel { get; set; }
        public static int m_gridSize { get; set; }

        private const int GRID_SIZE = 20;
        private const int OBSTACLE_COUNT = 15;
        private readonly int WINDOW_WIDTH;
        private readonly int WINDOW_HEIGHT;

        private List<Entity> m_removeThese = new List<Entity>();
        private List<Entity> m_addThese = new List<Entity>();

        private Systems.Renderer m_sysRenderer;
        private Systems.KeyboardInput m_sysKeyboardInput;
        private Systems.Animate m_sysAnimate;
        private Systems.Collision m_sysCollision;
        private Systems.Movement m_sysMovement;

        private Texture2D m_backgroundSquare;
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
        }

        public void Initialize(ContentManager content, SpriteBatch spriteBatch)
        {
            loadContent(content);

            m_sysRenderer = new Systems.Renderer(spriteBatch, m_backgroundSquare, WINDOW_WIDTH, WINDOW_HEIGHT, GRID_SIZE);
            m_sysKeyboardInput = new Systems.KeyboardInput();
            m_sysAnimate = new Systems.Animate();
            m_sysCollision = new Systems.Collision();
            m_sysMovement = new Systems.Movement();

            initializeLevel();
            //initializeBorder(texSquare);
            //initializeObstacles(texSquare);
            //initializeSnake(texSquare);
            //AddEntity(createFood(texSquare));
        }

        public void Update(GameTime gameTime)
        {
            m_sysKeyboardInput.Update(gameTime);
            m_sysCollision.Update(gameTime);
            m_sysMovement.Update(gameTime);
            m_sysAnimate.Update(gameTime);

            foreach (var entity in m_removeThese)
            {
                RemoveEntity(entity);
            }
            m_removeThese.Clear();

            foreach (var entity in m_addThese)
            {
                AddEntity(entity);
            }
            m_addThese.Clear();
        }

        public void Draw(GameTime gameTime)
        {
            m_sysRenderer.Update(gameTime);
        }

        private void AddEntity(Entity entity)
        {
            m_sysKeyboardInput.Add(entity);
            m_sysRenderer.Add(entity);
            m_sysAnimate.Add(entity);
            m_sysCollision.Add(entity);
            m_sysMovement.Add(entity);
        }

        private void RemoveEntity(Entity entity)
        {
            m_sysKeyboardInput.Remove(entity.Id);
            m_sysRenderer.Remove(entity.Id);
            m_sysAnimate.Remove(entity.Id);
            m_sysCollision.Remove(entity.Id);
            m_sysMovement.Remove(entity.Id);
        }

        private void loadContent(ContentManager content)
        {
            m_backgroundSquare = content.Load<Texture2D>("Images/square");

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
            switch (letter)
            {
                case 'w':
                    m_addThese.Add(Wall.create(m_wall, x, y));
                    break;
                case 'r':
                    m_addThese.Add(Rock.create(m_rock, x, y));
                    break;
                case 'f':
                    m_addThese.Add(Flag.create(m_flag, x, y));
                    break;
                case 'b':
                    m_addThese.Add(BigBlue.create(m_bigBlue, x, y));
                    break;
                case 'l':
                    m_addThese.Add(Floor.create(m_floor, x, y));
                    break;
                case 'g':
                    m_addThese.Add(Grass.create(m_grass, x, y));
                    break;
                case 'a':
                    m_addThese.Add(Water.create(m_water, x, y));
                    break;
                case 'v':
                    m_addThese.Add(Lava.create(m_lava, x, y));
                    break;
                case 'h':
                    m_addThese.Add(Hedge.create(m_hedge, x, y));
                    break;
                case 'W':
                    m_addThese.Add(Text.create(m_wordWall, x, y, new Color(41, 49, 65)));
                    break;
                case 'R':
                    m_addThese.Add(Text.create(m_wordRock, x, y, new Color(144, 103, 62)));
                    break;
                case 'F':
                    m_addThese.Add(Text.create(m_wordFlag, x, y, new Color(237, 226, 133)));
                    break;
                case 'B':
                    m_addThese.Add(Text.create(m_wordBaba, x, y, new Color(217, 57, 106)));
                    break;
                case 'I':
                    m_addThese.Add(Text.create(m_wordIs, x, y, Color.White));
                    break;
                case 'S':
                    m_addThese.Add(Text.create(m_wordStop, x, y, new Color(75, 92, 28)));
                    break;
                case 'P':
                    m_addThese.Add(Text.create(m_wordPush, x, y, new Color(144, 103, 62)));
                    break;
                case 'V':
                    m_addThese.Add(Text.create(m_wordLava, x, y, new Color(130, 38, 28)));
                    break;
                case 'A':
                    m_addThese.Add(Text.create(m_wordWater, x, y, new Color(95, 157, 209)));
                    break;
                case 'Y':
                    m_addThese.Add(Text.create(m_wordYou, x, y, new Color(217, 57, 106)));
                    break;
                case 'X':
                    m_addThese.Add(Text.create(m_wordWin, x, y, new Color(237, 226, 133)));
                    break;
                case 'N':
                    m_addThese.Add(Text.create(m_wordSink, x, y, new Color(95, 157, 209)));
                    break;
                case 'K':
                    m_addThese.Add(Text.create(m_wordKill, x, y, new Color(130, 38, 28)));
                    break;
            }
        }

            //private void initializeBorder(Texture2D square)
            //{
            //    for (int position = 0; position < GRID_SIZE; position++)
            //    {
            //        var left = Hedge.create(square, 0, position);
            //        AddEntity(left);

            //        var right = Hedge.create(square, GRID_SIZE - 1, position);
            //        AddEntity(right);

            //        var top = Hedge.create(square, position, 0);
            //        AddEntity(top);

            //        var bottom = Hedge.create(square, position, GRID_SIZE - 1);
            //        AddEntity(bottom);
            //    }
            //}

            //private void initializeObstacles(Texture2D square)
            //{
            //    MyRandom rnd = new MyRandom();
            //    int remaining = OBSTACLE_COUNT;

            //    while (remaining > 0)
            //    {
            //        int x = (int)rnd.nextRange(1, GRID_SIZE - 1);
            //        int y = (int)rnd.nextRange(1, GRID_SIZE - 1);
            //        var proposed = Floor.create(square, x, y);
            //        if (!m_sysCollision.collidesWithAny(proposed))
            //        {
            //            AddEntity(proposed);
            //            remaining--;
            //        }
            //    }
            //}

            //private void initializeSnake(Texture2D square)
            //{
            //    MyRandom rnd = new MyRandom();
            //    bool done = false;

            //    while (!done)
            //    {
            //        int x = (int)rnd.nextRange(1, GRID_SIZE - 1);
            //        int y = (int)rnd.nextRange(1, GRID_SIZE - 1);
            //        var proposed = BigBlue.create(square, x, y);
            //        if (!m_sysCollision.collidesWithAny(proposed))
            //        {
            //            AddEntity(proposed);
            //            done = true;
            //        }
            //    }
            //}

            //private Entity createFood(Texture2D square)
            //{
            //    MyRandom rnd = new MyRandom();
            //    bool done = false;

            //    while (!done)
            //    {
            //        int x = (int)rnd.nextRange(1, GRID_SIZE - 1);
            //        int y = (int)rnd.nextRange(1, GRID_SIZE - 1);
            //        var proposed = Flag.create(square, x, y);
            //        if (!m_sysCollision.collidesWithAny(proposed))
            //        {
            //            return proposed;
            //        }
            //    }

            //    return null;
        //}
    }
}
