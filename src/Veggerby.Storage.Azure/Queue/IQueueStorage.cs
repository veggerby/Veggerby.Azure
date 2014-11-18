using System.Collections.Generic;
using System.Threading.Tasks;

namespace Veggerby.Storage.Azure.Queue
{
    public interface IQueueStorage<T> : IQueueStorage where T : new()
    {
        Task InitializeAsync();
        Task<bool> AddAsync(T obj);
        Task<QueueItem<T>> GetAsync();
        Task<T> DeleteAsync(QueueItem<T> queueItem);
    }

    public interface IQueueStorage
    {
        string Name { get; }
        Task<QueueInformationEntity> GetInformationAsync();
        Task<IEnumerable<QueueMessageEntity>> PeekMessagesAsync(int count);
    }
}