using System.Collections.Generic;
using System.Threading.Tasks;
using Veggerby.Identity.Azure.Entity;
using Veggerby.Storage.Azure.Table;

namespace Veggerby.Identity.Azure.Storage
{
    public interface IUserClaimEntityStorage : ITableStorage<UserClaimEntity>
    {
        Task<IEnumerable<UserClaimEntity>> ListClaimsByClaimValue(string claimType, string claimValue);
    }
}