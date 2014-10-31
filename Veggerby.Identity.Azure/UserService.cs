using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Veggerby.Identity.Azure.Entity;
using Veggerby.Identity.Azure.Storage;

namespace Veggerby.Identity.Azure
{
    public class UserService : IUserLoginStore<UserEntity>, IUserClaimStore<UserEntity>, IUserRoleStore<UserEntity>, IUserPasswordStore<UserEntity>, IUserSecurityStampStore<UserEntity>, IUserStore<UserEntity>, IUserTokenProvider<UserEntity, string>, IDisposable
    {
        private readonly IUserEntityStorage _userStorage;
        private readonly IUserLoginEntityStorage _userLoginStorage;
        private readonly IUserClaimEntityStorage _userClaimStorage;
        private readonly IUserRoleEntityStorage _userRoleStorage;
        private readonly IUserTokenEntityStorage _userTokenStorage;

        public UserService(IUserEntityStorage userStorage, IUserLoginEntityStorage userLoginStorage, IUserClaimEntityStorage userClaimStorage, IUserRoleEntityStorage userRoleStorage, IUserTokenEntityStorage userTokenStorage)
        {
            _userStorage = userStorage;
            _userLoginStorage = userLoginStorage;
            _userClaimStorage = userClaimStorage;
            _userRoleStorage = userRoleStorage;
            _userTokenStorage = userTokenStorage;
        }

        public void Dispose()
        {
        }

        public async Task CreateAsync(UserEntity user)
        {
            var result = await _userStorage.InsertAsync(user);

            if (string.IsNullOrEmpty(result))
            {
                throw new Exception(string.Format("Failed to create user account ({0}).", user.UserName));
            }

            user.InternalId = user.RowKey;
        }

        public async Task UpdateAsync(UserEntity user)
        {
            await _userStorage.ReplaceAsync(user);
        }

        public async Task DeleteAsync(UserEntity user)
        {
            await _userStorage.DeleteAsync(user);
        }

        public async Task<UserEntity> GetByInternalId(string internalId)
        {
            var partitionKey = GetPartitionKeyFromEmail(internalId);
            return await _userStorage.GetAsync(partitionKey, internalId);
        }

        public async Task<UserEntity> FindByIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }

            var internalId = userId.ToBase64UserId();
            return await GetByInternalId(internalId);
        }

        public async Task<UserEntity> FindByNameAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return null;
            }

            var internalId = userName.ToBase64UserId();
            return await GetByInternalId(internalId);
        }

        private static string GetPartitionKeyFromEmail(string userName)
        {
            var partitionKey = "user";
            if (!string.IsNullOrEmpty(userName) && userName.Contains("@"))
            {
                partitionKey = userName.Split('@').Last();
            }

            return partitionKey;
        }

        public async Task AddLoginAsync(UserEntity user, UserLoginInfo login)
        {
            await _userLoginStorage.InsertAsync(new UserLoginEntity
            {
                UserId = user.InternalId,
                LoginProvider = login.LoginProvider,
                ProviderKey = login.ProviderKey
            });
        }

        public async Task RemoveLoginAsync(UserEntity user, UserLoginInfo login)
        {
            var userLogin = await _userLoginStorage.GetAsync(user.InternalId, login.LoginProvider);
            await _userLoginStorage.DeleteAsync(userLogin);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(UserEntity user)
        {
            var userLogins = await _userLoginStorage.ListAsync(user.InternalId);
            return userLogins
                .Select(x => new UserLoginInfo(x.LoginProvider, x.ProviderKey))
                .ToList();
        }

        public Task<UserEntity> FindAsync(UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<Claim>> GetClaimsAsync(UserEntity user)
        {
            var claims = await _userClaimStorage.ListAsync(user.InternalId);
            return claims
                .Select(x => new Claim(x.ClaimType, x.ClaimValue))
                .ToList();
        }

        public async Task AddClaimAsync(UserEntity user, Claim claim)
        {
            await _userClaimStorage.InsertAsync(new UserClaimEntity
            {
                RowKey = Guid.NewGuid().ToString("N"),
                UserId = user.InternalId,
                ClaimType = claim.Type,
                ClaimValue = claim.Value
            });
        }

        public async Task RemoveClaimAsync(UserEntity user, Claim claim)
        {
            var claims = await _userClaimStorage.ListAsync(user.InternalId);
            var deleteClaims = claims.Where(x => string.Equals(claim.Type, x.ClaimType) && string.Equals(claim.Value, x.ClaimValue)).ToList();
            foreach (var c in deleteClaims)
            {
                await _userClaimStorage.DeleteAsync(c);
            }
        }

        public async Task AddToRoleAsync(UserEntity user, string roleName)
        {
            await _userRoleStorage.InsertAsync(new UserRoleEntity
            {
                UserId = user.InternalId,
                RoleName = roleName
            });
        }

        public async Task RemoveFromRoleAsync(UserEntity user, string roleName)
        {
            var role = await _userRoleStorage.GetAsync(user.InternalId, roleName);
            await _userRoleStorage.DeleteAsync(role);
        }

        public async Task<IList<string>> GetRolesAsync(UserEntity user)
        {
            var roles = await _userRoleStorage.ListAsync(user.InternalId);
            return roles.Select(x => x.RoleName).ToList();
        }

        public async Task<bool> IsInRoleAsync(UserEntity user, string roleName)
        {
            var role = await _userRoleStorage.GetAsync(user.InternalId, roleName);
            return role != null;
        }

        public Task SetPasswordHashAsync(UserEntity user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(UserEntity user)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(UserEntity user)
        {
            return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
        }

        public Task SetSecurityStampAsync(UserEntity user, string stamp)
        {
            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }

        public Task<string> GetSecurityStampAsync(UserEntity user)
        {
            return Task.FromResult(user.SecurityStamp);
        }

        public async Task<UserQuerySegment> GetUsersSegmentedAsync(string continuationToken, int pageCount, string queryFilter = null)
        {
            var queryResults = await _userStorage.ListAllSegmentedAsync(continuationToken, pageCount, queryFilter).ConfigureAwait(false);

            return new UserQuerySegment
            {
                ContinuationToken = queryResults.ContinuationToken,
                Users = queryResults.Items.ToList()
            };
        }

        public async Task UpdateUserAsync(UserEntity user, string displayName, string givenName, string surName)
        {
            await AddClaimAsync(user, new Claim(ClaimTypes.Name, displayName));
            await AddClaimAsync(user, new Claim(ClaimTypes.GivenName, givenName));
            await AddClaimAsync(user, new Claim(ClaimTypes.Surname, surName));
        }

        public async Task<string> GenerateAsync(string purpose, UserManager<UserEntity, string> manager, UserEntity user)
        {
            var token = new UserTokenEntity
            {
                UserId = user.InternalId,
                Token = Guid.NewGuid().ToString("N"),
                IssueDateTimeUtc = DateTime.UtcNow,
                Purpose = purpose
            };

            await _userTokenStorage.InsertAsync(token);

            return token.Token;
        }

        public async Task<bool> ValidateAsync(string purpose, string token, UserManager<UserEntity, string> manager, UserEntity user)
        {
            var tokenEntity = await _userTokenStorage.GetAsync(user.InternalId, token);
            return
                tokenEntity != null
                    // && tokenEntity.IssueDateTimeUtc.AddDays(1) > DateTime.UtcNow - Disable date check for now (welcome email)
                && string.Equals(purpose, tokenEntity.Purpose);
        }

        public Task NotifyAsync(string token, UserManager<UserEntity, string> manager, UserEntity user)
        {
            return Task.FromResult(0);
        }

        public Task<bool> IsValidProviderForUserAsync(UserManager<UserEntity, string> manager, UserEntity user)
        {
            return Task.FromResult(true);
        }
    }
}