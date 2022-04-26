using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace BBIY
{
    public class KeyboardControls
    {
        public Keys moveUp { get; set; }
        public Keys moveDown { get; set; }
        public Keys moveLeft { get; set; }
        public Keys moveRight { get; set; }
        public Keys reset { get; set; }

        public KeyboardControls() { }

        public KeyboardControls(Keys up, Keys down, Keys left, Keys right, Keys reset)
        {
            this.moveUp = up;
            this.moveDown = down;
            this.moveLeft = left;
            this.moveRight = right;
            this.reset = reset;
        }
    }
}
