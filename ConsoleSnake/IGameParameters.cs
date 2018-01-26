using System;

namespace ConsoleSnake
{
    internal interface IGameParameters
    {
        int BoardWidth { get; }

        int BoardHeight { get; }

        int SnakeSpeed { get; }

        int InitialSnakeSegmentCount { get; }

        int Score { get; }

        TimeSpan Duration { get; }

        int OverallSnakeSegmentsCount { get; }
    }
}
