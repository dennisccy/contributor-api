using System.Collections.Generic;
using contributor_api.Model.Enum;

namespace contributor_api.Service.Interface
{
    public interface IAuthorization
    {
        /// <summary>
        /// Can plug e.g. SGConnect behind
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>Token</returns>
        string Authorize(string userName);

        /// <summary>
        /// Check if the token is valid or not
        /// </summary>
        /// <param name="token"></param>
        /// <returns>Is valid token or not</returns>
        bool IsValidToken(string token);

        /// <summary>
        /// Get the list of user permission e.g. Read, Write
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>List of user permisssions</returns>
        List<UserPermissionEnum> GetUserPermissions(string userName);

        /// <summary>
        /// Get username by token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        string GetUserName(string token);
    }
}
