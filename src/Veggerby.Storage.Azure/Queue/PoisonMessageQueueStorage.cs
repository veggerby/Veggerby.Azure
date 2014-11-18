namespace Veggerby.Storage.Azure.Queue
{
    public class PoisonMessageQueueStorage : QueueStorage<PoisonMessage>, IPoisonMessageQueueStorage
    {
        public PoisonMessageQueueStorage(string queueName = "poisonmessages", string connectionString = null, StorageInitializeManager storageInitializeManager = null)
            : base(queueName, connectionString, storageInitializeManager, null)
        {
        }
    }
}