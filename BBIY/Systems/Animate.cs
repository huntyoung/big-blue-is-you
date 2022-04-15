using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Systems
{
    class Animate : System
    {
        private float m_elapsedTime;
        private const float ANIMATION_INTERVAL = 0.3f;

        public Animate() : base(typeof(Components.Animated))
        {
        }

        public override void Update(GameTime gameTime)
        {
            m_elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (m_elapsedTime >= ANIMATION_INTERVAL)
            {
                foreach (var entity in m_entities.Values)
                {
                    var animated = entity.GetComponent<Components.Animated>();

                    if (animated.animationStage < animated.numAnimationSegments - 1)
                    {
                        animated.sourceRectangle.X += animated.xPixelShift;
                        animated.animationStage++;
                    } else
                    {
                        animated.sourceRectangle.X = 0;
                        animated.animationStage = 0;
                    }
                }

                float carryOverTime = m_elapsedTime - ANIMATION_INTERVAL;
                m_elapsedTime = carryOverTime;
            }
        }
    }
}
