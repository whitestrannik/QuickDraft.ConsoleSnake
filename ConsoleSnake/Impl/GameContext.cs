using System;

namespace ConsoleSnake
{
    public class GameContext : IGameContext
    {
        public GameContext(int boardWidth, int boardHeight, int snakeSpeed, int initialSnakeSegmentCount)
        {
            BoardWidth = boardWidth;
            BoardHeight = boardHeight;
            SnakeSpeed = snakeSpeed;
            InitialSnakeSegmentCount = initialSnakeSegmentCount;
            OverallSnakeSegmentsCount = initialSnakeSegmentCount;
        }

        public int BoardWidth { get; private set; }

        public int BoardHeight { get; private set; }

        public int SnakeSpeed { get; private set; }

        public int InitialSnakeSegmentCount { get; private set; }

        public int Score { get; private set; }

        public TimeSpan Duration { get; private set; }

        public int OverallSnakeSegmentsCount { get; private set; }

        public void AddDuration(TimeSpan value)
        {
            Duration += value;
        }

        public void AddScore(int value)
        {
            Score += value;
        }

        public void IncrementSegmentsCount()
        {
            OverallSnakeSegmentsCount++;
        }
    }
}
