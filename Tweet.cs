using System;

namespace elastic
{
    public class Tweet
    {
        public long Id { get; set; }
        public string User { get; set; }
        public DateTime PostDate { get; set; }
        public string Message { get; set; }
        public TestGeneric Add { get; set; }
    }
}