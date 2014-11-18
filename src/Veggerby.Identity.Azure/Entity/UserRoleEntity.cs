using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Veggerby.Identity.Azure.Entity
{
    public class UserRoleEntity : TableEntity
    {
        [IgnoreProperty]
        public string UserId
        {
            get { return PartitionKey; }
            set { PartitionKey = value; }
        }

        [IgnoreProperty]
        public string RoleName
        {
            get { return RowKey; }
            set { RowKey = value; }
        }

        public DateTime CreatedDateUtc { get; set; }
    }
}
