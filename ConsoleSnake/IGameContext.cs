using System;

namespace ConsoleSnake
{
    public interface IGameParameters
    {
        int BoardWidth { get; }

        int BoardHeight { get; }

        int SnakeSpeed { get; }

        int InitialSnakeSegmentCount { get; }

        int Score { get; }

        TimeSpan Duration { get; }

        int OverallSnakeSegmentsCount { get; }
    }

    public interface IGameContext : IGameParameters
    {
        void AddScore(int value);

        void AddDuration(TimeSpan value);

        void IncrementSegmentsCount();
    }
}
