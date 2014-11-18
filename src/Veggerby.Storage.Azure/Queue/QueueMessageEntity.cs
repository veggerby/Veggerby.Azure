using System;

namespace Veggerby.Storage.Azure.Queue
{
    public class QueueMessageEntity
    {
        public string Id { get; set; }
        public DateTimeOffset? InsertionTime { get; set; }
        public int DequeueCount { get; set; }
        public DateTimeOffset? ExpirationTime { get; set; }
        public DateTimeOffset? NextVisibleTime { get; set; }
        public string Message { get; set; }
    }
}