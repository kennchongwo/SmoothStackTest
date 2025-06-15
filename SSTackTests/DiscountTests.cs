using NUnit.Framework;

namespace SSTackTests
{
    public class DiscountTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Testing1()
        {
            Assert.Pass();
        }

        [Test]
        [TestCase("1b96b5fc-159d-4b61-aa69-8898d04d6b25",1)]
        public void GetCustomerDiscountTest()
        {
            //Arrange & Act
            var s = new SStack.Controllers.OrderServiceController().GetDiscountPercentage("1b96b5fc-159d-4b61-aa69-8898d04d6b25", 1);

            //Assert
            Assert.AreEqual(15, s);
        }
    }
}