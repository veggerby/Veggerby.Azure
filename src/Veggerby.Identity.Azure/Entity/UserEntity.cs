using System;
using Microsoft.AspNet.Identity;
using Microsoft.WindowsAzure.Storage.Table;

namespace Veggerby.Identity.Azure.Entity
{
    public class UserEntity : TableEntity, IUser
    {
        public string Id
        {
            get { return UserName; }
        }

        public string InternalId { get; set; }

        public string UserName { get; set; }

        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }

        public DateTime CreatedDateUtc { get; set; }
        public DateTime LastAccessDateUtc { get; set; }

        public DateTime? LastAccessFailureDateUtc { get; set; }
        public int? AccessFailedCount { get; set; }

        public DateTime? LockOutEndDateUtc { get; set; }
        public bool IsLockedOut { get; set; }

        public string Email { get; set; }
        public bool IsEmailConfirmed { get; set; }
    }
}
