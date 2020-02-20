using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;

namespace elastic
{
    public class ElasticSearchOperations
    {
        private readonly ElasticClient _client;

        public ElasticSearchOperations(string url)
        {
            var node = new Uri(url);

            var settings = new ConnectionSettings(node);

            _client = new ElasticClient(settings);
        }

        public async Task CreateIndex(Tweet tweet, string index)
        {
            var response = await _client.IndexAsync(tweet, idx => idx.Index(index));

            if (!response.IsValid)
            {
                throw new Exception (response.OriginalException.Message ?? response.Result.ToString());
            }
        }

        public async Task DeleteIndex(long id, string index)
        {
            var response = await _client.DeleteAsync<Tweet>(id, idx => idx.Index(index));

            if (!response.IsValid)
            {
                throw new Exception (response.OriginalException.Message ?? response.Result.ToString());
            }
        }

        public async Task<Tweet> GetIndex(long id, string index)
        {
            var response = await _client.GetAsync<Tweet>(id, idx => idx.Index(index));

            if (!response.IsValid)
            {
                throw new Exception (response.OriginalException.Message);
            }

            return response.Source;
        }

        public async Task<List<Tweet>> SerchByMessage(string index, string term)
        {
            var response = await _client.SearchAsync<Tweet>(s => s
                .Index(index)
                .Query(q => q
                   .MatchPhrase(m => m
                        .Field(f3 => f3.Message)
                        .Query(term)
                   )
                )
            );

            return response.Documents.ToList();
        }

        public async Task<List<Tweet>> SerchByAdd(string index, string term)
        {
            var response = await _client.SearchAsync<Tweet>(s => s
                .Index(index)
                .Query(q => q
                   .MatchPhrase(m => m
                        .Field(f1 => f1.Add.Name)
                        .Field(f2 => f2.Add.Value)
                        .Query(term)
                   )
                )
            );

            if (!response.IsValid)
            {
                throw new Exception (response.OriginalException.Message);
            }

            return response.Documents.ToList();
        }

        public async Task<List<Tweet>> SerchByUser(string index, string term)
        {
            var response = await _client.SearchAsync<Tweet>(s => s
                .Index(index) //or specify index via settings.DefaultIndex("mytweetindex");
                .From(0)
                .Size(10)
                .Query(q => q
                    .Term(t => t.User, term)
                )
            );

            if (!response.IsValid)
            {
                throw new Exception (response.OriginalException.Message);
            }

            return response.Documents.ToList();
        }

         public async Task<List<Tweet>> SerchByAllFilter(string index, string term)
        {
            var response = await _client.SearchAsync<Tweet>(s => s
                .Index(index)
                .Query(q => q
                   .MatchPhrase(m => m
                        .Field(f1 => f1.Add.Name)
                        .Field(f2 => f2.Add.Value)
                        .Field(f3 => f3.User)
                        .Field(f4 => f4.Message)
                        .Query(term)
                   )
                )
            );

            if (!response.IsValid)
            {
                throw new Exception (response.OriginalException.Message);
            }

            return response.Documents.ToList();
        }
    }
}