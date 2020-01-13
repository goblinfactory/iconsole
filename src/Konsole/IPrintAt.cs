using System;
using System.Collections.Generic;
using System.Text;

namespace Konsole
{
    public interface IPrintAt
    {
        void PrintAt(int x, int y, string format, params object[] args);
        void PrintAt(int x, int y, string text);
        void PrintAt(int x, int y, char c);
        void PrintAtColor(ConsoleColor foreground, int x, int y, string text, ConsoleColor? background);

        int WindowWidth { get; }
        int WindowHeight { get; }

    }
}
