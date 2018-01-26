using ConsoleSnake.Common;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleSnake.Impl
{
    public class RenderMgr : IRenderMgr
    {
        private readonly IGameParameters _gameParameters;
        private readonly IInputOutputMgr _inputOutputMgr;
        private Task _task;
        private readonly CancellationTokenSource _cts;
        private long _counter;
        private volatile Action _currentRenderAction;

        public RenderMgr(IGameParameters gameResults, IInputOutputMgr inputOutputMgr)
        {
            _gameParameters = gameResults;
            _inputOutputMgr = inputOutputMgr;
            _cts = new CancellationTokenSource();

            StartRenderCycle();
        }

        public void SetRenderData(FieldTypes[,] dataForRendering)
        {
            _currentRenderAction = () => Render((i, j) => GetCharFromFieldType(dataForRendering[i, j]));
        }

        public void ShowText(string text)
        {
            _currentRenderAction = () => Render((i, j) => RenderScreen(i, j, text));
        }

        public void Dispose()
        {
            _cts.Cancel();
            _task.Wait();
            _cts.Dispose();
        }

        private char RenderScreen(int i, int j, string text)
        {
            if (i == _gameParameters.BoardHeight / 2 && j < text.Length)
            {
                return text[j];
            }
            else return ' ';
        }

        private void StartRenderCycle()
        {
            _task = StartRenderCycleInternal();
        }

        private async Task StartRenderCycleInternal()
        {
            while (!_cts.IsCancellationRequested)
            {
                await Task.Delay(80).ConfigureAwait(false);

                try
                {
                    _currentRenderAction();
                }
                catch (Exception ex)
                {
                    _inputOutputMgr.LogAndExit(ex);
                }
            }
        }

        private void Render(Func<int, int, char> contentRenderer)
        {
            _counter++;

            var realRowCount = _gameParameters.BoardHeight + 5;
            var realColumnCount = _gameParameters.BoardWidth + 2;
            var capacity = realRowCount * realColumnCount;
            var sb = new StringBuilder(capacity);

            sb.Append("╔".PadRight(7, '═'));
            sb.Append("╦".PadRight(7, '═'));
            sb.Append("╦".PadRight(realColumnCount - 15, '═'));
            sb.Append('╗');
            sb.AppendLine();

            sb.Append('║');
            sb.Append(GetAnimatedLine());
            sb.Append(_gameParameters.Duration.TotalSeconds.ToString("00000"));
            sb.Append('║');
            sb.Append(GetAnimatedLine());
            sb.Append(_gameParameters.OverallSnakeSegmentsCount.ToString("00000"));
            sb.Append('║');
            sb.Append(GetAnimatedLine());
            sb.Append(_gameParameters.Score.ToString("00000").PadRight(realColumnCount - 17, ' '));
            sb.Append('║');
            sb.AppendLine();

            sb.Append("╠".PadRight(7, '═'));
            sb.Append("╩".PadRight(7, '═'));
            sb.Append("╩".PadRight(realColumnCount - 15, '═'));
            sb.Append('╣');
            sb.AppendLine();

            for (int i = 0; i < _gameParameters.BoardHeight; i++)
            {
                sb.Append('║');
                for (int j = 0; j < _gameParameters.BoardWidth; j++)
                {
                    sb.Append(contentRenderer(i, j));
                }
                sb.Append('║');
                sb.AppendLine();
            }

            sb.Append("╚".PadRight(realColumnCount - 1, '═'));
            sb.Append('╝');

            _inputOutputMgr.DisplayOutput(sb.ToString());
        }

        private char GetAnimatedLine()
        {
            var value = _counter % 4;

            if (value == 0)
            {
                return '|';
            }
            else if (value == 1)
            {
                return '/';
            }
            else if (value == 2)
            {
                return '-';
            }
            else
            {
                return '\\';
            }
        }

        private char GetAnimatedCircle()
        {
            var value = _counter % 3;

            if (value == 0)
            {
                return 'o';
            }
            else if (value == 1)
            {
                return 'O';
            }
            else
            {
                return '0';
            }
        }

        private char GetCharFromFieldType(FieldTypes value)
        {
            char result;
            switch (value)
            {
                case FieldTypes.Empty:
                    result = ':';
                    break;
                case FieldTypes.Snake:
                    result = 'o';
                    break;
                case FieldTypes.NextHead:
                    result = GetAnimatedCircle();
                    break;
                case FieldTypes.Wall:
                    result = '▒';
                    break;
                default:
                    throw new ArgumentException("Unknown field type");
            }

            return result;
        }
    }
}
