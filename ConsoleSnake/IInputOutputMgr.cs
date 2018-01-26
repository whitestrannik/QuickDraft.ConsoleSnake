using System;

namespace ConsoleSnake
{
    public interface IInputOutputMgr
    {
        void WaitForClick();

        IDisposable SubscribeToInput(Action<ConsoleKeyInfo> observer);

        void DisplayOutput(string output);

        void LogAndExit(Exception ex);
    }
}
