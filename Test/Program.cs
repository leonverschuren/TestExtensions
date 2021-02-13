using System;
using System.Linq.Expressions;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var d1 = new Data
            {
                Title = "Title1",
                Published = DateTime.Now
            };

            var d2 = new Data()
            {
                Title = "Title2",
                Published = DateTime.Now
            };

            var compare = AreEqual();
            var result = compare.Compile().Invoke(d1, d2);
        }

        public static Expression<Func<Data, Data, bool>> AreEqual() =>
            (d1, d2) => d1.Title == d2.Title && d1.Published == d2.Published && d1.Amount == d2.Amount;
    }

    public class Data
    {
        public string Title { get; set; }
        public DateTime Published { get; set; }
        public int Amount { get; set; }
    }
}
