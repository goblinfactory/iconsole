﻿using System;
using static System.ConsoleColor;

namespace Konsole
{
    public class Colors
    {
        public ConsoleColor Foreground { get; } = White;
        public ConsoleColor Background { get; } = Black;

        public Colors()
        {

        }

        public Colors(ConsoleColor foreground, ConsoleColor background)
        {
            Foreground = foreground;
            Background = background;
        }

        public static Colors WhiteOnBlack
        {
            get
            {
                return new Colors(Gray, Black);
            }
        }

        public static Colors BlackOnWhite
        {
            get
            {
                return new Colors(Black, Gray);
            }
        }
    }
}
