using System.Collections.Generic;
using Veggerby.Identity.Azure.Entity;

namespace Veggerby.Identity.Azure
{
    public class UserQuerySegment
    {
        public string ContinuationToken { get; set; }
        public IEnumerable<UserEntity> Users { get; set; }
    }
}