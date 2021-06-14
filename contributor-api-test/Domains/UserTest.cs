using System.Collections.Generic;
using System.Linq;
using contributor_api.Model;
using contributor_api.Model.Enum;
using NUnit.Framework;

namespace contributor_api_test.Domains
{
    [TestFixture]
    public class UserTest
    {
        [Test]
        public void CreateNewUser()
        {
            var user = new User("test");
            Assert.AreEqual(user.UserName, "test");
            Assert.False(user.UserPermissions.Any());
        }

        [Test]
        public void CreateNewUserWithPermission()
        {
            var user = new User("test", new List<UserPermissionEnum> { UserPermissionEnum.ContributePrice, UserPermissionEnum.GetPrice });
            Assert.AreEqual(user.UserName, "test");
            Assert.True(user.UserPermissions.Contains(UserPermissionEnum.GetPrice));
            Assert.True(user.UserPermissions.Contains(UserPermissionEnum.ContributePrice));
        }
    }
}
