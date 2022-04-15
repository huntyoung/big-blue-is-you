using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace BBIY
{
    class KeyboardControls
    {
        public static Keys moveUp { get; set; } = Keys.W;
        public static Keys moveDown { get; set; } = Keys.S;
        public static Keys moveLeft { get; set; } = Keys.A;
        public static Keys moveRight { get; set; } = Keys.D;
        public static Keys reset { get; set; } = Keys.R;
    }
}
