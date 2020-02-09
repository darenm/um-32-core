using System;
using System.Collections.Generic;
using System.Text;

namespace Um32Core
{

    public static class Logger
    {
        public static void WriteInfo(string message)
        {
#if UMDEBUG
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
#endif
        }

        public static void WriteError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void Write(string message)
        {
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
