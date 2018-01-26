using ConsoleSnake.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleSnake.Impl
{
    internal sealed class SnakeObject
    {
        private SnakeSegment _head;
        private SnakeSegment _tail;

        internal SnakeObject(IEnumerable<Point> points)
        {
            ConstructSnake(points);
        }

        public IReadOnlyCollection<Point> Move(Direction direction)
        {
            if (direction == Direction.Up)
            {
                return MoveSnake(-1, 0, true);
            }
            else if (direction == Direction.Down)
            {
                return MoveSnake(1, 0, true);
            }
            else if (direction == Direction.Right)
            {
                return MoveSnake(0, 1, true);
            }
            else if (direction == Direction.Left)
            {
                return MoveSnake(0, -1, true);
            }

            throw new ArgumentException("Unknown direction value.");
        }

        public IReadOnlyCollection<Point> AddNewHead(Direction direction)
        {
            if (direction == Direction.Up)
            {
                return MoveSnake(-1, 0, false);
            }
            else if (direction == Direction.Down)
            {
                return MoveSnake(1, 0, false);
            }
            else if (direction == Direction.Right)
            {
                return MoveSnake(0, 1, false);
            }
            else if (direction == Direction.Left)
            {
                return MoveSnake(0, -1, false);
            }

            throw new ArgumentException("Unknown direction value.");
        }

        private IReadOnlyCollection<Point> GetSnakePoints()
        {
            var list = new List<Point>();

            SnakeSegment segment = _head;
            while (segment != null)
            {
                list.Add(segment.Position);
                segment = segment.Prev;
            }

            return list;
        }

        private IReadOnlyCollection<Point> MoveSnake(int deltaX, int deltaY, bool removeTail)
        {
            var secondSegment = _head.Prev;

            if (secondSegment.Position.X == _head.Position.X + deltaX && secondSegment.Position.Y == _head.Position.Y + deltaY)
            {
                return GetSnakePoints();
            }

            if (removeTail)
            {
                var preTail = _tail.Next;
                preTail.Prev = null;
                _tail = preTail;
            }

            var newHead = new SnakeSegment(new Point(_head.Position.X + deltaX, _head.Position.Y + deltaY), _head, null);
            _head.Next = newHead;
            _head = newHead;

            return GetSnakePoints();
        }

        private void ConstructSnake(IEnumerable<Point> points)
        {
            _head = new SnakeSegment(points.First(), null, null);

            SnakeSegment tail = _head;
            foreach (var point in points.Skip(1))
            {
                var segment = new SnakeSegment(point, null, tail);
                tail.Prev = segment;
                tail = segment;
            }

            _tail = tail;
        }

        private sealed class SnakeSegment
        {
            public SnakeSegment(Point position, SnakeSegment prev, SnakeSegment next)
            {
                Position = position;
                Prev = prev;
                Next = next;
            }

            public Point Position { get; }

            public SnakeSegment Prev { get; set; }

            public SnakeSegment Next { get; set; }
        }
    }
}
