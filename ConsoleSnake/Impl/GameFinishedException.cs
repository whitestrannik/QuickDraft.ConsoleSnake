using System;
using System.Runtime.Serialization;

namespace ConsoleSnake.Impl
{
    [Serializable]
    internal class GameFinishedException : Exception
    {
        internal GameFinishedException()
        {
        }

        internal GameFinishedException(string message) : base(message)
        {
        }

        internal GameFinishedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected GameFinishedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}