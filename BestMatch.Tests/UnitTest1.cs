using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BestMatch.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            var d1 = new Data { Title = "Title", Amount = 1 };

            var data = new List<Data>
            {
                new Data { Title = "Title", Amount = 2 }
            };

            // Act
            var result = data.AnyBestMatch(Comparison(d1));

            // Assert
            Assert.IsFalse(result.IsEqual);
            Assert.AreEqual("Expected Amount to be 1, actual 2", result.Message);
        }

        Expression<Func<Data, bool>> Comparison(Data data) =>
            d => d.Title == data.Title && d.Amount == data.Amount;
    }
}
