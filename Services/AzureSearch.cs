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
        public IQueryable<Product> RunQuery(string filter)
        {
            DocumentSearchResult<Product> results;
            results = indexClient.Documents.Search<Product>(filter);
            return results.Results.Select(x => x.Document).AsQueryable();
        }
    }
}