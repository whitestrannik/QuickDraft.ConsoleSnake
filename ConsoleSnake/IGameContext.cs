using System;

namespace ConsoleSnake
{
    internal interface IGameContext : IGameParameters
    {
        void AddScore(int value);

        void AddDuration(TimeSpan value);

        void IncrementSegmentsCount();
    }
}
