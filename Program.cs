using System;
using Nest;

namespace elastic
{
    static class Program
    {
        static void Main(string[] args)
        {
            var elastic = new ElasticSearchOperations("http://localhost:9200");

            var tweet = new Tweet
            {
                Id = 1,
                User = "kimchys",
                PostDate = new DateTime(2009, 11, 15),
                Message = "este funciona?",
                Add = new TestGeneric {Name = "fa", Value = "teste"}
            };

            elastic.CreateIndex(tweet, "1").Start();

            var result = elastic.SerchByAllFilter("1", "teste").Result;

            result.ForEach(x => Console.WriteLine(x.Message));
        }
    }
}
