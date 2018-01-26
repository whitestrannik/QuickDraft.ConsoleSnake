using System;

namespace ConsoleSnake.Common
{
    internal struct Point : IEquatable<Point>
    {
        internal Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        internal int X { get; }

        internal int Y { get; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj is Point)
            {
                return Equals((Point)obj);
            }

            return false;
        }

        public bool Equals(Point other)
        {
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return X * 100000 + Y;
        }
    }
}
