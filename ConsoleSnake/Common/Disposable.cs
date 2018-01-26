using System;

namespace ConsoleSnake.Common
{
    internal sealed class Disposable : IDisposable
    {
        private readonly Action _action;

        internal Disposable(Action action)
        {
            _action = action;
        }

        public void Dispose()
        {
            _action();
        }
    }
}
