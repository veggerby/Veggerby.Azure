using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace Veggerby.Storage.Azure.Queue
{
    public class QueueStorageSettings
    {
        public static QueueStorageSettings Default = new QueueStorageSettings
        {
            VisibilityTimeout = TimeSpan.FromMinutes(1), 
            MaxDequeueCount = 5
        };

        public TimeSpan VisibilityTimeout { get; set; }
        public int MaxDequeueCount { get; set; }
    }

    public class QueueStorage<T> : IQueueStorage, IQueueStorage<T> where T : new()
    {
        private readonly IPoisonMessageQueueStorage _poisonMessageQueue;
        private readonly QueueStorageSettings _settings;
        private readonly CloudQueue _queue;

        private static JsonSerializerSettings _Settings = new JsonSerializerSettings
        {
            Formatting = Formatting.None,
            TypeNameHandling = TypeNameHandling.All,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        public QueueStorage(string queueName = null, string connectionString = null, StorageInitializeManager storageInitializeManager = null, IPoisonMessageQueueStorage poisonMessageQueue = null, QueueStorageSettings settings = null)
        {
            _poisonMessageQueue = poisonMessageQueue;
            _settings = settings;

            var storageAccount = !string.IsNullOrEmpty(connectionString)
                ? CloudStorageAccount.Parse(connectionString)
                : CloudStorageAccount.DevelopmentStorageAccount;
            var queueClient = storageAccount.CreateCloudQueueClient();

            queueName = queueName ?? (typeof(T).Name.ToLowerInvariant());

            _queue = queueClient.GetQueueReference(queueName);

            if (storageInitializeManager == null || storageInitializeManager.HasBeenInitialized(queueName))
            {
                return;
            }

            _queue.CreateIfNotExists();
            storageInitializeManager.SetAsInitialized(queueName);
        }

        public async Task InitializeAsync()
        {
            await Queue.CreateIfNotExistsAsync();
        }

        protected CloudQueue Queue
        {
            get { return _queue; }
        }

        public string Name
        {
            get { return Queue.Name; }
        }

        public async Task<bool> AddAsync(T obj)
        {
            var message = new CloudQueueMessage(JsonConvert.SerializeObject(obj, _Settings));
            await Queue.AddMessageAsync(message);
            return true;
        }

        public async Task<QueueItem<T>> GetAsync()
        {
            while (true)
            {
                var message = await Queue.GetMessageAsync(
                    (_settings ?? QueueStorageSettings.Default).VisibilityTimeout, 
                    null,
                    null);

                if (message == null)
                {
                    return null;
                }

                if (message.DequeueCount >= (_settings ?? QueueStorageSettings.Default).MaxDequeueCount)
                {
                    var poisonMessage = new PoisonMessage
                    {
                        OriginalInsertionTimeUtc = message.InsertionTime, 
                        SourceQueue = Queue.Name, 
                        SourceMessage = message.AsString
                    };

                    await _poisonMessageQueue.AddAsync(poisonMessage);
                    await Queue.DeleteMessageAsync(message);

                    continue;
                }

                try
                {
                    var item = JsonConvert.DeserializeObject<T>(message.AsString, _Settings);
                    return new QueueItem<T>
                    {
                        Message = message, Item = item
                    };
                }
                catch (Exception e)
                {
                    throw new GetQueueItemException("Failed to get message from queue", e);
                }
            }
        }

        public async Task<T> DeleteAsync(QueueItem<T> queueItem)
        {
            await Queue.DeleteMessageAsync(queueItem.Message);
            return queueItem.Item;
        }

        public async Task<QueueInformationEntity> GetInformationAsync()
        {
            await Queue.FetchAttributesAsync();

            var result = new QueueInformationEntity
            {
                Name = Queue.Name,
                ApproximateMessageCount = Queue.ApproximateMessageCount
            };

            var messages = (await Queue.PeekMessagesAsync(32)).ToList();

            result.AverageTimeOnQueue = messages.Any()
                ? TimeSpan.FromTicks((long)Math.Round(messages.Where(x => x.InsertionTime != null).Average(x => DateTimeOffset.UtcNow.Ticks - x.InsertionTime.Value.Ticks)))
                : (TimeSpan?)null;

            result.MaxTimeOnQueue = messages.Any()
                ? TimeSpan.FromTicks((long)Math.Round(messages.Where(x => x.InsertionTime != null).Max(x => 1.0D * DateTimeOffset.UtcNow.Ticks - x.InsertionTime.Value.Ticks)))
                : (TimeSpan?)null;

            result.MaxDequeueCount = messages.Any()
                ? messages.Max(x => x.DequeueCount)
                : (int?)null;

            result.AverageDequeueCount = messages.Any()
                ? messages.Average(x => x.DequeueCount)
                : (double?)null;

            return result;
        }

        public async Task<IEnumerable<QueueMessageEntity>> PeekMessagesAsync(int count)
        {
            var messages = (await Queue.PeekMessagesAsync(count)).ToList();
            return messages.Select(x => new QueueMessageEntity
            {
                Id = x.Id,
                InsertionTime = x.InsertionTime,
                DequeueCount = x.DequeueCount,
                ExpirationTime = x.ExpirationTime,
                NextVisibleTime = x.NextVisibleTime,
                Message = x.AsString,
            }).ToList();
        }
    }
}
