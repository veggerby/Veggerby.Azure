using Microsoft.WindowsAzure.Storage.Table;
using Veggerby.Identity.Azure.Entity;
using Veggerby.Storage.Azure.Table.Query;

namespace Veggerby.Identity.Azure.Storage
{
    public class ListUserClaimEntityByClaimValueStorageQuery : StorageQuery<UserClaimEntity>
    {
        public ListUserClaimEntityByClaimValueStorageQuery(string claimType, string claimValue)
        {
            Query = Query.Where(TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("ClaimType", QueryComparisons.Equal, claimType),
                TableOperators.And,
                TableQuery.GenerateFilterCondition("ClaimValue", QueryComparisons.Equal, claimValue)
                ));
        }
    }
}