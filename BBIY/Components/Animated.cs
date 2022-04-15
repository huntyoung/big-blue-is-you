using Microsoft.Xna.Framework;

namespace Components
{
    public class Animated : Component
    {
        public Rectangle sourceRectangle;
        public int xPixelShift;
        public int animationStage = 0;
        public int numAnimationSegments = 3;

        public Animated(Rectangle sourceRectangle, int xPixelShift)
        {
            this.sourceRectangle = sourceRectangle;
            this.xPixelShift = xPixelShift;
        }
    }
}
