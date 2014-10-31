using System;

namespace Veggerby.Storage.Azure.Queue
{
    public class QueueInformationEntity
    {
        public string Name { get; set; }

        public int? ApproximateMessageCount { get; set; }

        /// <summary>
        /// Gets the average time of items on the queue (looking only at the first 100 messages)
        /// </summary>
        public TimeSpan? AverageTimeOnQueue { get; set; }
        public TimeSpan? MaxTimeOnQueue { get; set; }

        public double? AverageDequeueCount { get; set; }
        public int? MaxDequeueCount { get; set; }
    }
}
