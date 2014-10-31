using Microsoft.WindowsAzure.Storage.Table;

namespace Veggerby.Identity.Azure.Entity
{
    public class UserClaimEntity : TableEntity
    {
        [IgnoreProperty]
        public string UserId
        {
            get { return PartitionKey; }
            set { PartitionKey = value; }
        }

        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }
    }
}
