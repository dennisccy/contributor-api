using System;
using System.Linq;
using contributor_api.Model.Enum;
using contributor_api.Service;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace contributor_api_test.Service
{
    [TestFixture]
    public class LocalAuthorizationServiceTest
    {
        private Mock<ILogger<LocalAuthorizationService>> _mockLogger;
        private LocalAuthorizationService _service;

        [SetUp]
        public void SetUp()
        {
            _mockLogger = new Mock<ILogger<LocalAuthorizationService>>();
            _service = new LocalAuthorizationService(_mockLogger.Object);
        }

        [Test]
        public void AuthorizeUser_Exist()
        {
            Assert.IsFalse(string.IsNullOrEmpty(_service.Authorize("dennis.chan")));
        }

        [Test]
        public void AuthorizeUser_NonExist()
        {
            Assert.IsTrue(string.IsNullOrEmpty(_service.Authorize("dennis.chann")));
        }

        [Test]
        public void IsValidToken()
        {
            var token = _service.Authorize("dennis.chan");
            Assert.IsTrue(_service.IsValidToken(token));
            Assert.IsFalse(_service.IsValidToken(token + "fake"));
            Assert.IsFalse(_service.IsValidToken(null));
        }

        [Test]
        public void GetPermission()
        {
            var permissions = _service.GetUserPermissions("dennis.chan");
            Assert.IsTrue(permissions.Contains(UserPermissionEnum.GetPrice));
            Assert.IsTrue(permissions.Contains(UserPermissionEnum.ContributePrice));

            permissions = _service.GetUserPermissions("peter.smith");
            Assert.IsTrue(permissions.Contains(UserPermissionEnum.GetPrice));
            Assert.IsFalse(permissions.Contains(UserPermissionEnum.ContributePrice));

            permissions = _service.GetUserPermissions("fake");
            Assert.IsNull(permissions);
        }

        [Test]
        public void GetUserName()
        {
            var token = _service.Authorize("dennis.chan");
            Assert.AreEqual("dennis.chan", _service.GetUserName(token));
            Assert.IsNull(_service.GetUserName(token + "fake"));
            Assert.IsNull(_service.GetUserName(null));
        }
    }
}
