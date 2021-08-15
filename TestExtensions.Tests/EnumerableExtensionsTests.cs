using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestExtensions.Tests
{
    [TestClass]
    public class EnumerableExtensionsTests
    {
        [TestMethod]
        public void MatchAny_SinglePropertyComparedWithConstant_ReturnsComparedPropertyNameWithActualAndExpectedValue()
        {
            // Arrange
            var data = new List<Data>
            {
                new() { Title = "OtherTitle", Amount = 1 }
            };

            // Act
            MatchAnyResult matchAnyResult = data.MatchAny(d => d.Title == "MyTitle");

            // Assert
            Assert.IsFalse(matchAnyResult.IsMatch);
            Assert.AreEqual("Expected 'Title' to be 'MyTitle', actual 'OtherTitle'", matchAnyResult.Message);
        }

        [TestMethod]
        public void MatchAny_MultiplePropertiesComparedWithConstant_ReturnsComparedPropertyNameWithActualAndExpectedValue()
        {
            // Arrange
            var data = new List<Data>
            {
                new() { Title = "MyTitle", Amount = 2 }
            };

            // Act
            MatchAnyResult matchAnyResult = data.MatchAny(d => d.Title == "MyTitle" && d.Amount == 1);

            // Assert
            Assert.IsFalse(matchAnyResult.IsMatch);
            Assert.AreEqual("Expected 'Amount' to be '1', actual '2'", matchAnyResult.Message);
        }

        [TestMethod]
        public void MatchAny_MultiplePropertiesComparedWithProvidedObject_ReturnsComparedPropertyNameWithActualAndExpectedValue()
        {
            // Arrange
            var expected = new Data { Title = "MyTitle", Amount = 1 };

            var data = new List<Data>
            {
                new() { Title = "MyTitle", Amount = 2 }
            };

            // Act
            MatchAnyResult matchAnyResult = data.MatchAny(d => d.Title == expected.Title && d.Amount == expected.Amount);

            // Assert
            Assert.IsFalse(matchAnyResult.IsMatch);
            Assert.AreEqual("Expected 'Amount' to be '1', actual '2'", matchAnyResult.Message);
        }

        [TestMethod]
        public void MatchAny_MultipleObjectsInCollectionOneMatching_ReturnsIsMatchTrueAndMessageIsEmptyString()
        {
            // Arrange
            var expected = new Data { Title = "MyTitle", Amount = 1 };

            var data = new List<Data>
            {
                new() { Title = "MyTitle", Amount = 1 },
                new() { Title = "MyTitle", Amount = 2 }
            };

            // Act
            MatchAnyResult matchAnyResult = data.MatchAny(d => d.Title == expected.Title && d.Amount == expected.Amount);

            // Assert
            Assert.IsTrue(matchAnyResult.IsMatch);
            Assert.AreEqual(string.Empty, matchAnyResult.Message);
        }

        [TestMethod]
        public void MatchAny_MultipleObjectsInCollectionNoneMatching_ReturnsObjectWithMostMatchingProperties()
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
            MatchAnyResult matchAnyResult = data.MatchAny(d => d.Title == expected.Title && d.Amount == expected.Amount && d.Published == expected.Published);

            // Assert
            Assert.IsFalse(matchAnyResult.IsMatch);
            Assert.AreEqual("Expected 'Amount' to be '1', actual '2'", matchAnyResult.Message);
        }
    }
}
