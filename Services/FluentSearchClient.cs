using Microsoft.Azure.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Rest;
using Newtonsoft.Json;

namespace SimpleEchoBot.Services
{
    public class FluentSearchClient
    {
        public static SearchIndexClient CreateSearchIndexClient()
        {
            var configuration = System.Configuration.ConfigurationManager.AppSettings;
            string searchServiceName = configuration["SearchServiceName"];
            string queryApiKey = configuration["SearchServiceAdminApiKey"];
            string searchServiceIndexName = configuration["SearchServiceIndexName"];

            return new SearchIndexClient(searchServiceName, searchServiceIndexName, new SearchCredentials(queryApiKey));
        }
    }
}