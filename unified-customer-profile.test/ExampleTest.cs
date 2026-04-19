using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace unified_customer_profile.test
{
    [TestClass]
    public sealed class ExampleServiceTests
    {
        private readonly ExampleService _exampleService;

        public ExampleServiceTests()
        {
            _exampleService = new ExampleService();
        }

        [TestMethod]
        public void ExampleServiceTests_IsFour()
        {
            // Arrange
            int a = 2;
            int b = 2;

            // Act
            int result = _exampleService.PlusOperator(a, b);

            // Assert
            Assert.AreEqual(4, result);
        }
    }
}
