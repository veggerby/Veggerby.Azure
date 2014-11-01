using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Veggerby.Identity.Azure.Entity
{
    public class UserLoginEntity : TableEntity
    {
        [IgnoreProperty]
        public string UserId
        {
            get { return PartitionKey; }
            set { PartitionKey = value; }
        }

        [IgnoreProperty]
        public string LoginProvider
        {
            get { return RowKey; }
            set { RowKey = value; }
        }

        public virtual string ProviderKey { get; set; }

        public DateTime CreatedDateUtc { get; set; }

        public bool IsEnabled { get; set; }
    }
}
