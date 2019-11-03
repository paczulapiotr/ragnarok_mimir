using System;

namespace Mimir.Core.CommonExceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message = "You are not authorized to perform this action") : base(message) { }
        public UnauthorizedException(string message, Exception inner) : base(message, inner) { }
    }
}
