using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using SimpleEchoBot.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SimpleEchoBot.Services
{
    public class AzureSearch
    {
        private readonly ISearchIndexClient indexClient;

        public AzureSearch(ISearchIndexClient indexClient)
        {
            this.indexClient = indexClient;
        }
        public IQueryable<Product> RunQuery(string product)
        {
            DocumentSearchResult<Product> results;
            results = indexClient.Documents.Search<Product>(product);
            return results.Results.Select(x => x.Document).AsQueryable();
        }

        public IQueryable<Product> RunQuery(string product, string color)
        {
            DocumentSearchResult<Product> results;
            var parameters =
                new SearchParameters()
                {
                    Filter = $"Color eq '{color}'"
                };
            results = indexClient.Documents.Search<Product>(product, parameters);
            return results.Results.Select(x => x.Document).AsQueryable();
        }
    }
}