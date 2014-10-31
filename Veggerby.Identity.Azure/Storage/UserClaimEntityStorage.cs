using System.Collections.Generic;
using System.Threading.Tasks;
using Veggerby.Identity.Azure.Entity;
using Veggerby.Storage.Azure;
using Veggerby.Storage.Azure.Table;

namespace Veggerby.Identity.Azure.Storage
{
    public class UserClaimEntityStorage : TableStorage<UserClaimEntity>, IUserClaimEntityStorage
    {
        public UserClaimEntityStorage(string connectionString, StorageInitializeManager storageInitializeManager)
            : base(connectionString: connectionString, storageInitializeManager: storageInitializeManager)
        {
        }

        public Task<IEnumerable<UserClaimEntity>> ListClaimsByClaimValue(string claimType, string claimValue)
        {
            return new ListUserClaimEntityByClaimValueStorageQuery(claimType, claimValue).ExecuteOnAsync(Table);
        }
    }
}