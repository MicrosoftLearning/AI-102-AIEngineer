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

// Import search namespaces
using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;



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
            AzureKeyCredential credential = new AzureKeyCredential(QueryKey);
            SearchClient searchClient = new SearchClient(SearchEndpoint, IndexName, credential);



            // Submit search query
            var options = new SearchOptions{
                IncludeTotalCount = true,
                SearchMode = SearchMode.All,
                Filter = FilterExpression,
                OrderBy = {SortOrder},
                Facets = {"metadata_author"},
                HighlightFields = {"merged_content-3","imageCaption-3"} 
            };
            options.Select.Add("url");
            options.Select.Add("metadata_storage_name");
            options.Select.Add("metadata_author");
            options.Select.Add("metadata_storage_size");
            options.Select.Add("metadata_storage_last_modified");
            options.Select.Add("language");
            options.Select.Add("sentiment");
            options.Select.Add("merged_content");
            options.Select.Add("keyphrases");
            options.Select.Add("locations");
            options.Select.Add("imageTags");
            options.Select.Add("imageCaption");
            SearchResults<SearchResult> results = searchClient.Search<SearchResult>(SearchTerms, options);
            return results;



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
