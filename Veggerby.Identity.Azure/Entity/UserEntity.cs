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

        public string EmailAddress { get; set; }
        public string DisplayName { get; set; }
        public DateTime LastLogin { get; set; }
    }
}
