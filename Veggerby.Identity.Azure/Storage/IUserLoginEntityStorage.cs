using Veggerby.Identity.Azure.Entity;
using Veggerby.Storage.Azure.Table;

namespace Veggerby.Identity.Azure
{
    public interface IUserLoginEntityStorage : ITableStorage<UserLoginEntity>
    {
    }
}