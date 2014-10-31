using Veggerby.Identity.Azure.Entity;
using Veggerby.Storage.Azure;
using Veggerby.Storage.Azure.Table;

namespace Veggerby.Identity.Azure.Storage
{
    public class UserRoleEntityStorage : TableStorage<UserRoleEntity>, IUserRoleEntityStorage
    {
        public UserRoleEntityStorage(string connectionString, StorageInitializeManager storageInitializeManager)
            : base(connectionString: connectionString, storageInitializeManager: storageInitializeManager)
        {
        }
    }
}