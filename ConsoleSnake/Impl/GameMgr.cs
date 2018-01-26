using ConsoleSnake.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleSnake.Impl
{
    internal sealed class GameMgr : IGameMgr
    {
        private readonly IGameContext _gameContext;
        private readonly IInputOutputMgr _inputMgr;
        private readonly IRenderMgr _renderMgr;
        private readonly Random _rnd;

        private List<Point> _prebuildWalls;
        private List<Point> _snakeNextHeads;
        private List<Point> _initialSnakeState;
        private volatile Direction _currentDirection;
        private SnakeObject _snake;

        internal GameMgr(IGameContext gameContext, IInputOutputMgr inputMgr, IRenderMgr renderMgr)
        {
            _gameContext = gameContext;
            _inputMgr = inputMgr;
            _renderMgr = renderMgr;
            _currentDirection = Direction.Right;
            _rnd = new Random();
        }

        public void Play()
        {
            ShowWelcomeScreen();

            PlayGame();

            ShowFinalScreen();
        }

        private void InitGame()
        {
            _initialSnakeState = new List<Point> { new Point(0, 2), new Point(0, 1), new Point(0, 0) };
            _prebuildWalls = GeneratePrebuildsWalls(20);
            _snakeNextHeads = new List<Point> { GenerateNewHead(), GenerateNewHead(), GenerateNewHead() };
            _snake = new SnakeObject(_initialSnakeState);
            Render(_initialSnakeState);
        }

        private void PlayGame()
        {
            InitGame();

            using (_inputMgr.SubscribeToInput(OnKeyPressed))
            {
                var time = DateTime.Now;
                while (true)
                {
                    Task.Delay((10 - _gameContext.SnakeSpeed) * 100).Wait();

                    try
                    {
                        var currentTime = DateTime.Now;
                        _gameContext.AddDuration(currentTime - time);
                        time = currentTime;

                        RedrawBoard();
                    }
                    catch (GameFinishedException)
                    {
                        return;
                    }
                }
            }
        }

        private void ShowFinalScreen()
        {
            _renderMgr.ShowText("Game is over!");
        }

        private void ShowWelcomeScreen()
        {
            _renderMgr.ShowText("Press any key ...");
            _inputMgr.WaitForClick();
        }

        private void OnKeyPressed(ConsoleKeyInfo key)
        {
            var direction = ConvertToDirection(key);

            if (direction.HasValue)
            {
                _currentDirection = direction.Value;
            }
        }

        private void RedrawBoard()
        {
            var newSnakePositions = _snake.Move(_currentDirection);
            var head = newSnakePositions.First();

            CheckForCollision(head);

            if (_snakeNextHeads.Contains(head))
            {
                _snakeNextHeads.Remove(head);
                _snakeNextHeads.Add(GenerateNewHead());
                newSnakePositions = _snake.AddNewHead(_currentDirection);
                _gameContext.IncrementSegmentsCount();
                _gameContext.AddScore(10);
            }

            Render(newSnakePositions);
        }

        private void CheckForCollision(Point head)
        {
            if (head.X < 0 || head.X > _gameContext.BoardHeight || head.Y < 0 || head.Y >= _gameContext.BoardWidth || _prebuildWalls.Contains(head))
            {
                throw new GameFinishedException(); 
            }
        }

        private void Render(IReadOnlyCollection<Point> snakePositions)
        {
            var board = new FieldTypes[_gameContext.BoardHeight, _gameContext.BoardWidth];

            foreach (var position in snakePositions)
            {
                board[position.X, position.Y] = FieldTypes.Snake;
            }

            foreach (var wall in _prebuildWalls)
            {
                board[wall.X, wall.Y] = FieldTypes.Wall;
            }

            foreach (var segment in _snakeNextHeads)
            {
                board[segment.X, segment.Y] = FieldTypes.NextHead;
            }

            board[_gameContext.BoardHeight-1, _gameContext.BoardWidth-1] = FieldTypes.NextHead;

            _renderMgr.SetRenderData(board);
        }

        private Direction? ConvertToDirection(ConsoleKeyInfo key)
        {
            Direction? result;

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    result = Direction.Up;
                    break;
                case ConsoleKey.DownArrow:
                    result = Direction.Down;
                    break;
                case ConsoleKey.LeftArrow:
                    result = Direction.Left;
                    break;
                case ConsoleKey.RightArrow:
                    result = Direction.Right;
                    break;
                default:
                    result = null;
                    break;
            }

            return result;
        }

        private List<Point> GeneratePrebuildsWalls(int wallsToGenerate)
        {
            List<Point> list = new List<Point>();

            while (list.Count < wallsToGenerate)
            {
                var point = GeneratePoint(_gameContext.BoardHeight - 1, _gameContext.BoardWidth - 1);

                if (!_initialSnakeState.Contains(point))
                {
                    list.Add(point);
                }
            }

            return list;
        }

        private Point GenerateNewHead()
        {
            while (true)
            {
                var point = GeneratePoint(_gameContext.BoardHeight - 1, _gameContext.BoardWidth - 1);

                if (!_prebuildWalls.Contains(point))
                {
                    return point;
                }
            }
        }

        private Point GeneratePoint(int maxRow, int maxColumn)
        {
            return new Point(_rnd.Next(maxRow), _rnd.Next(maxColumn));
        }
    }
}
