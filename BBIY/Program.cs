using System;

namespace BBIY
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new BBIYGame())
                game.Run();
        }
    }
}
