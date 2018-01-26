using ConsoleSnake.Common;
using System;

namespace ConsoleSnake
{
    public interface IRenderMgr : IDisposable
    {
        void SetRenderData(FieldTypes[,] dataForRendering);

        void ShowText(string text);
    }
}
