using System.Collections.Generic;
using Veggerby.Identity.Azure.Entity;

namespace Veggerby.Identity.Azure
{
    public class UserQuerySegment<T> where T : UserEntity
    {
        public string ContinuationToken { get; set; }
        public IEnumerable<T> Users { get; set; }
    }
}