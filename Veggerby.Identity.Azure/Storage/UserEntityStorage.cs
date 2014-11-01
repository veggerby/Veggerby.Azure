using Veggerby.Identity.Azure.Entity;
using Veggerby.Storage.Azure;
using Veggerby.Storage.Azure.Table;

namespace Veggerby.Identity.Azure.Storage
{
    public class UserEntityStorage<T> : TableStorage<T>, IUserEntityStorage<T> where T : UserEntity, new()
    {
        public UserEntityStorage(string connectionString, StorageInitializeManager storageInitializeManager)
            : base(connectionString: connectionString, storageInitializeManager: storageInitializeManager)
        {
        }
    }
}