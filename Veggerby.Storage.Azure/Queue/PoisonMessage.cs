using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veggerby.Storage.Azure.Queue
{
    public class PoisonMessage
    {
        public DateTimeOffset? OriginalInsertionTimeUtc { get; set; }
        public string SourceQueue { get; set; }
        public string SourceMessage { get; set; }
    }
}
