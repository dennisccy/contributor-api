using System;
using System.Collections.Generic;
using System.Linq;
using contributor_api.Model;
using contributor_api.Model.Enum;
using contributor_api.Service.Interface;
using Microsoft.Extensions.Logging;

namespace contributor_api.Service
{
    /// <summary>
    /// Simple authorization with hard-coded users and permissions, and without session expiry management, etc
    /// Assume no user name duplication
    /// </summary>
    public class LocalAuthorizationService : IAuthorization
    {
        private readonly ILogger<LocalAuthorizationService> _logger;
        private List<User> _users;
        private Dictionary<string, string> _userTokens;

        public LocalAuthorizationService(ILogger<LocalAuthorizationService> logger)
        {
            _logger = logger;

            _users = new List<User>();
            _users.Add(new User("dennis.chan", new List<UserPermissionEnum> { UserPermissionEnum.GetPrice, UserPermissionEnum.ContributePrice }));
            _users.Add(new User("peter.smith", new List<UserPermissionEnum> { UserPermissionEnum.GetPrice }));
            _users.Add(new User("gwen.harthway", new List<UserPermissionEnum> { UserPermissionEnum.ContributePrice }));
            _users.Add(new User("jean.petit", new List<UserPermissionEnum>()));

            _userTokens = new Dictionary<string, string>();
        }

        public string Authorize(string userName)
        {
            string token = null;

            if (_users.Any(x => x.UserName == userName))
            {
                if (!_userTokens.Any(x => x.Key == userName))
                {
                    token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                    _userTokens.Add(userName, token);
                    _logger.LogInformation($"Authorize user: {userName}");

                }
                else
                {
                    token = _userTokens[userName];
                }
            }

            return token;
        }

        public bool IsValidToken(string token)
        {
            _logger.LogDebug($"Validate token: {token}");
            if (_userTokens.Any(x => x.Value == token))
            {
                return true;
            }

            return false;
        }

        public List<UserPermissionEnum> GetUserPermissions(string userName)
        {
            var user = _users.SingleOrDefault(x => x.UserName == userName);
            if (user != null)
            {
                return user.UserPermissions;
            }

            return null;
        }

        public string GetUserName(string token)
        {
            var userToken = _userTokens.SingleOrDefault(x => x.Value == token);

            if (!userToken.Equals(default(Dictionary<string, string>)))
            {
                return userToken.Key;
            }

            return null;
        }
    }
}
