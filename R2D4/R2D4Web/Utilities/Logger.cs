using System;

namespace R2D4Web.Utilities
{
    public static class Logger
    {
        public static void Display(string message)
        {
            Console.WriteLine(message);
        }
        public static void Log(string message)
        {
            //TODO: For real logging
            Console.WriteLine(message);
        }
    }
}