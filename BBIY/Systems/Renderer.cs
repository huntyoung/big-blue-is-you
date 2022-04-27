using Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Systems
{

    class Renderer : System
    {
        private readonly int CELL_SIZE;
        private readonly int OFFSET_X;
        private readonly int OFFSET_Y;
        private readonly SpriteBatch m_spriteBatch;
        private readonly Texture2D m_texBackground;

        public Renderer(SpriteBatch spriteBatch, Texture2D texBackGround, int width, int height, int gridWidth, int gridHeight) :
            base(typeof(Components.Appearance), typeof(Components.Position))
        {
            CELL_SIZE = height / gridHeight;
            OFFSET_X = (width - gridWidth * CELL_SIZE) / 2;
            OFFSET_Y = (height - gridHeight * CELL_SIZE) / 2;
            m_spriteBatch = spriteBatch;
            m_texBackground = texBackGround;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entity in m_entities.Values)
            {
                renderEntity(entity);
            }
        }

        private void renderEntity(Entity entity)
        {
            var appearance = entity.GetComponent<Components.Appearance>();
            var position = entity.GetComponent<Components.Position>();
            Rectangle area = new Rectangle();

            area.X = OFFSET_X + position.x * CELL_SIZE;
            area.Y = OFFSET_Y + position.y * CELL_SIZE;
            area.Width = CELL_SIZE;
            area.Height = CELL_SIZE;

            if (entity.ContainsComponent<Components.Animated>())
            {
                var animated = entity.GetComponent<Components.Animated>();
                m_spriteBatch.Draw(appearance.image, area, animated.sourceRectangle, 
                    appearance.fill, 0f, new Vector2(0, 0), SpriteEffects.None, position.layerDepth);
            }
            else
            {
                m_spriteBatch.Draw(appearance.image, area, null, 
                    appearance.fill, 0f, new Vector2(0,0), SpriteEffects.None, position.layerDepth);
            }
        }
    }
}
