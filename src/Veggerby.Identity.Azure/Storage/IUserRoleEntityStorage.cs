﻿using Veggerby.Identity.Azure.Entity;
using Veggerby.Storage.Azure.Table;

namespace Veggerby.Identity.Azure.Storage
{
    public interface IUserRoleEntityStorage : ITableStorage<UserRoleEntity>
    {
    }
}