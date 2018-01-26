using ConsoleSnake.Impl;
using System;

namespace ConsoleSnake
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var rowCount = 20;
                var columnCount = 40;
                var snakeSpeed = 6;
                var initialSnakeSize = 3;
                var gameContext = new GameContext(columnCount, rowCount, snakeSpeed, initialSnakeSize);

                var inputMgr = new InputOutputMgr();

                using (var renderMgr = new RenderMgr(gameContext, inputMgr))
                {
                    var game = new GameMgr(gameContext, inputMgr, renderMgr);
                    game.Play();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine("Press any key to exit.");
            }

            Console.ReadKey();
        }
    }
}
