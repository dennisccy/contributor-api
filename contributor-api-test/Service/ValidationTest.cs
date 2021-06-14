using contributor_api.Model;
using contributor_api.Service;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace contributor_api_test.Service
{
    [TestFixture]
    public class ValidationTest
    {
        private Mock<ILogger<ValidationService>> _mockLogger;
        private ValidationService _service;

        [SetUp]
        public void SetUp()
        {
            _mockLogger = new Mock<ILogger<ValidationService>>();
            _service = new ValidationService(_mockLogger.Object);
        }

        [Test]
        public void Validate_Valid()
        {
            var price = new Price("EUR", "USE", 1.2, 1.22, "test");
            var result = _service.ValidateForex(price, out string error);

            Assert.IsTrue(result);
            Assert.IsNull(error);
        }

        [Test]
        public void Validate_Invalid_Bid_Equal_Ask()
        {
            var price = new Price("EUR", "USE", 1.2, 1.2, "test");
            var result = _service.ValidateForex(price, out string error);

            Assert.IsFalse(result);
            Assert.IsNotNull(error);
        }

        [Test]
        public void Validate_Invalid_Bid_Larger_Than_Ask()
        {
            var price = new Price("EUR", "USE", 1.21, 1.2, "test");
            var result = _service.ValidateForex(price, out string error);

            Assert.IsFalse(result);
            Assert.IsNotNull(error);
        }

        [Test]
        public void Validate_Invalid_Spread_Too_Large()
        {
            var price = new Price("EUR", "USE", 1, 1.2, "test");
            var result = _service.ValidateForex(price, out string error);

            Assert.IsFalse(result);
            Assert.IsNotNull(error);
        }
    }
}
