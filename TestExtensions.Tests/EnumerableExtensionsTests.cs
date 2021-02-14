using System;
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

        [TestMethod]
        public void AnyBestMatch_MultiplePropertiesComparedWithProvidedObject_ReturnsComparedPropertyNameWithActualAndExpectedValue()
        {
            // Arrange
            var expected = new Data { Title = "MyTitle", Amount = 1 };

            var data = new List<Data>
            {
                new() { Title = "MyTitle", Amount = 2 }
            };

            // Act
            Result result = data.AnyBestMatch(d => d.Title == expected.Title && d.Amount == expected.Amount);

            // Assert
            Assert.IsFalse(result.IsEqual);
            Assert.AreEqual("Expected 'Amount' to be '1', actual '2'", result.Message);
        }

        [TestMethod]
        public void AnyBestMatch_MultipleObjectsInCollectionOneMatching_ReturnsIsEqualTrueAndMessageIsEmptyString()
        {
            // Arrange
            var expected = new Data { Title = "MyTitle", Amount = 1 };

            var data = new List<Data>
            {
                new() { Title = "MyTitle", Amount = 1 },
                new() { Title = "MyTitle", Amount = 2 }
            };

            // Act
            Result result = data.AnyBestMatch(d => d.Title == expected.Title && d.Amount == expected.Amount);

            // Assert
            Assert.IsTrue(result.IsEqual);
            Assert.AreEqual(string.Empty, result.Message);
        }

        [TestMethod]
        public void AnyBestMatch_MultipleObjectsInCollectionNoneMatching_ReturnsObjectWithMostMatchingProperties()
        {
            // Arrange
            var expected = new Data { Title = "MyTitle", Amount = 1, Published = DateTime.Today };

            var data = new List<Data>
            {
                new() { Title = "OtherTitle", Amount = 2, Published = DateTime.Today.AddDays(1) }, // 0 matching properties
                new() { Title = "MyTitle", Amount = 2, Published = DateTime.Today.AddDays(1) }, // 1 matching property
                new() { Title = "MyTitle", Amount = 2, Published = DateTime.Today } // 2 matching properties
            };

            // Act
            Result result = data.AnyBestMatch(d => d.Title == expected.Title && d.Amount == expected.Amount && d.Published == expected.Published);

            // Assert
            Assert.IsFalse(result.IsEqual);
            Assert.AreEqual("Expected 'Amount' to be '1', actual '2'", result.Message);
        }
    }
}
