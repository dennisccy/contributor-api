using System;
using System.Collections.Generic;
using contributor_api.Model.Enum;

namespace contributor_api.Model
{
    public class User
    {
        public string UserName { get; set; }
        public List<UserPermissionEnum> UserPermissions { get; set; }

        public User(string userName)
        {
            UserName = userName;
            UserPermissions = new List<UserPermissionEnum>();
        }

        public User(string userName, List<UserPermissionEnum> userPermissions)
        {
            UserName = userName;
            UserPermissions = userPermissions;
        }
    }
}
