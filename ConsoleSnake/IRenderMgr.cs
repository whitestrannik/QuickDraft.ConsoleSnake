using ConsoleSnake.Common;
using System;

namespace ConsoleSnake
{
    internal interface IRenderMgr : IDisposable
    {
        void SetRenderData(FieldTypes[,] dataForRendering);

        void ShowText(string text);
    }
}
