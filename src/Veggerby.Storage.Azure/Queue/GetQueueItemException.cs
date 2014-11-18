using System;

namespace Veggerby.Storage.Azure.Queue
{
    public class GetQueueItemException : ApplicationException
    {
        public GetQueueItemException(string message)
            : base(message)
        {
        }

        public GetQueueItemException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
