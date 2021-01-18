using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.WebUtilities;
using search_client.Models;
using Microsoft.Extensions.Configuration;

// Import namespaces



namespace search_client.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        private Uri SearchEndpoint;
        private string QueryKey;
        private string IndexName;
        public string SearchTerms { get; set; } = "";
        public string SortOrder { get; set; } = "search.score()";

        public string FilterExpression { get; set; } = "";

        public SearchResults<SearchResult> search_results;

        //Wrapper function for request to search index
        public SearchResults<SearchResult> search_query(string searchText, string filterBy, string sortOrder)
        {

            // Create a search client



            // Submit search query



        }

        public void OnGet()
        {

            // Get the search endpoint and key
            IConfigurationBuilder _builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            IConfigurationRoot _configuration = _builder.Build();
            SearchEndpoint = new Uri(_configuration["SearchServiceEndpoint"]);
            QueryKey = _configuration["SearchServiceQueryApiKey"];
            IndexName = _configuration["SearchIndexName"];


            if (Request.QueryString.HasValue){
                var queryString = QueryHelpers.ParseQuery(Request.QueryString.ToString());
                SearchTerms = queryString["search"];

                if (queryString.Keys.Contains("sort")){
                    SortOrder = queryString["sort"];
                }

                if (queryString.Keys.Contains("facet")){
                    FilterExpression = "metadata_author eq '" + queryString["facet"] + "'";
                }
                else
                {
                    FilterExpression = "";
                }
                

                search_results = search_query(SearchTerms, SortOrder, FilterExpression);

            }
            else{
                SearchTerms="";
            }
        }


    }

}
