using System;

namespace MizJam1
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new MizJam1Game())
                game.Run();
        }
    }
}
