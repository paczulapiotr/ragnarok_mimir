using System;

namespace Mimir.Core.CommonExceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException(string message = "You are forbidden to perform this action") : base(message) { }
        public ForbiddenException(string message, Exception inner) : base(message, inner) { }
    }
}
