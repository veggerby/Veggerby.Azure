using Veggerby.Identity.Azure.Entity;
using Veggerby.Storage.Azure;
using Veggerby.Storage.Azure.Table;

namespace Veggerby.Identity.Azure.Storage
{
    public class UserTokenEntityStorage : TableStorage<UserTokenEntity>, IUserTokenEntityStorage
    {
        public UserTokenEntityStorage(string connectionString, StorageInitializeManager storageInitializeManager)
            : base(connectionString: connectionString, storageInitializeManager: storageInitializeManager)
        {
        }
    }
}