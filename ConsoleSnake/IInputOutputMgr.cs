using System;

namespace ConsoleSnake
{
    internal interface IInputOutputMgr
    {
        void WaitForClick();

        IDisposable SubscribeToInput(Action<ConsoleKeyInfo> observer);

        void DisplayOutput(string output);

        void LogAndStopOutput(Exception ex);
    }
}
