using Microsoft.WindowsAzure.Storage.Queue;

namespace Veggerby.Storage.Azure.Queue
{
    public class QueueItem<T> where T : new()
    {
        public CloudQueueMessage Message { get; set; }
        public T Item { get; set; }
    }
}
