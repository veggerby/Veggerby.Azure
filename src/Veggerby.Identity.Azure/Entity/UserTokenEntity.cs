using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Veggerby.Identity.Azure.Entity
{
    public class UserTokenEntity : TableEntity
    {
        [IgnoreProperty]
        public string UserId
        {
            get { return PartitionKey; }
            set { PartitionKey = value; }
        }

        [IgnoreProperty]
        public string Token
        {
            get { return RowKey; }
            set { RowKey = value; }
        }

        public string Purpose { get; set; }

        public DateTime IssueDateUtc { get; set; }

        public int ValidateCount { get; set; }
    }
}