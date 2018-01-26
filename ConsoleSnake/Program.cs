using ConsoleSnake.Impl;

namespace ConsoleSnake
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputMgr = new InputOutputMgr();

            try
            {
                var gameInfo = new GameContext(20, 20, 10, 3);
                
                using (var renderMgr = new RenderMgr(gameInfo, inputMgr))
                using (var game = new GameMgr(gameInfo, inputMgr, renderMgr))
                {
                    game.Play();
                }
            }
            catch (System.Exception ex)
            {
                inputMgr.LogAndExit(ex);
            }

            inputMgr.WaitForClick();
        }
    }
}
