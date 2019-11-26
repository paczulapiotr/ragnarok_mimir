using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mimir.Core
{
    public abstract class TimestampRecord
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Timestamp { get; private set; }
        
        public bool CompareTimestamp(DateTime timestamp) => timestamp == Timestamp;
    }
}
