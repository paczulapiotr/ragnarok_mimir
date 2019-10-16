using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mimir.Core
{
    public abstract class TimestampRecord
    {
        [Timestamp]
        public byte[] Timestamp { get; private set; }
        
        [NotMapped]
        public string TimestampString { get => String.Join(".", Timestamp); }
        public bool CompareTimestamp(string timestampString) => timestampString == TimestampString;
    }
}
