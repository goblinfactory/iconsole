using System;

namespace Konsole
{
    public class ConsoleState
    {
        public bool CursorVisible { get; set; }
        public int Top { get; set; }
        public int Left { get; set; }
        public ConsoleColor ForegroundColor { get; set; }
        public ConsoleColor BackgroundColor { get; set; }

        public ConsoleState(ConsoleColor foreground, ConsoleColor background, int x, int y, bool cursorVisible)
        {
            ForegroundColor = foreground;
            BackgroundColor = background;
            Top = y;
            Left = x;
            CursorVisible = cursorVisible;
        }
    }
}