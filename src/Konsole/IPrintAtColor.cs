using System;

namespace Konsole
{
    public interface IPrintAtColor
    {
        void PrintAtColor(ConsoleColor foreground, int x, int y, string text, ConsoleColor? background);
    }
}
