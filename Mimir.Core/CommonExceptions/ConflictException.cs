﻿using System;

namespace Mimir.Core.CommonExceptions
{

    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message) { }
        public ConflictException(string message, Exception inner) : base(message, inner) { }
    }
}
