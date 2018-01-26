using ConsoleSnake.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleSnake.Impl
{
    public class InputOutputMgr : IInputOutputMgr
    {
        private volatile bool _isExit;

        public InputOutputMgr()
        {
            Console.CursorVisible = false;
            Console.Clear();
        }

        public void WaitForClick()
        {
            Console.ReadKey(true);
        }

        public IDisposable SubscribeToInput(Action<ConsoleKeyInfo> observer)
        {
            var cts = new CancellationTokenSource();
            var task = ListenInternal(cts.Token, observer);

            return new Disposable(() =>
            {
               cts.Cancel();
               task.Wait();
               cts.Dispose();
            });
        }

        private async Task ListenInternal(CancellationToken token, Action<ConsoleKeyInfo> observer)
        {
            while (!token.IsCancellationRequested)
            {
                await Task.Delay(50).ConfigureAwait(false);

                if (Console.KeyAvailable)
                {
                    observer(Console.ReadKey());
                }
            }
        }

        public void LogAndExit(Exception ex)
        {
            _isExit = true;

            Console.Clear();
            Console.WriteLine(ex.Message);
            Console.WriteLine("Press any key...");
        }

        public void DisplayOutput(string output)
        {
            if (_isExit)
            {
                return;
            }

            Console.SetCursorPosition(0, 0);
            Console.Write(output);
        }
    }
}
