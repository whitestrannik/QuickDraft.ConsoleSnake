using System;

namespace ConsoleSnake
{
    public interface IGameMgr : IDisposable
    {
        void Play();
    }
}
