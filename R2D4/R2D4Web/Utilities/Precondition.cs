using System;

namespace R2D4Web.Utilities
{
    public class Precondition
    {
        public static void ThrowArgumentException(Func<bool> conditions, string name, string message)
        {
            if (conditions())
            {
                throw new ArgumentException(name, message);
            }
        }
    }
}