using System;
using System.Collections.Generic;
using System.Text;

namespace Konsole
{
    public interface ISetColors 
    {
        ConsoleColor ForegroundColor { get; set; }
        ConsoleColor BackgroundColor { get; set; }
    }
}
