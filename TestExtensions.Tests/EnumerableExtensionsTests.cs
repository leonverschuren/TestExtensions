using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestExtensions.Tests
{
    [TestClass]
    public class EnumerableExtensionsTests
    {
        [TestMethod]
        public void AnyBestMatch_SinglePropertyComparedWithConstant_ReturnsComparedPropertyNameWithActualAndExpectedValue()
        {
            // Arrange
            var data = new List<Data>
            {
                new() { Title = "OtherTitle", Amount = 1 }
            };

            // Act
            Result result = data.AnyBestMatch(d => d.Title == "MyTitle");

            // Assert
            Assert.IsFalse(result.IsEqual);
            Assert.AreEqual("Expected 'Title' to be 'MyTitle', actual 'OtherTitle'", result.Message);
        }

        [TestMethod]
        public void AnyBestMatch_MultiplePropertiesComparedWithConstant_ReturnsComparedPropertyNameWithActualAndExpectedValue()
        {
            // Arrange
            var data = new List<Data>
            {
                new() { Title = "MyTitle", Amount = 2 }
            };

            // Act
            Result result = data.AnyBestMatch(d => d.Title == "MyTitle" && d.Amount == 1);

            // Assert
            Assert.IsFalse(result.IsEqual);
            Assert.AreEqual("Expected 'Amount' to be '1', actual '2'", result.Message);
        }
    }
}
