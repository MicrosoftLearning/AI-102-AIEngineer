using System;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;


namespace search_client.Models
{
    public partial class SearchResult
    {
        [SearchableField(IsFilterable=true)]
        public string url { get; set; }

        [SearchableField()]
        public string merged_content { get; set; }

        [SearchableField(IsFilterable=true, IsSortable=true)]
        public string metadata_storage_name { get; set; }

        [SearchableField(IsFilterable=true, IsSortable=true, IsFacetable=true)]
        public string metadata_author { get; set; }

        [SearchableField(IsFilterable=true, IsSortable=true)]
        public int metadata_storage_size { get; set; }

        [SearchableField(IsFilterable=true, IsSortable=true)]
        public DateTime metadata_storage_last_modified { get; set; }

        [SimpleField(IsFilterable=true, IsSortable=true)]
        public double sentiment { get; set; }

        [SearchableField(IsFilterable=true)]
        public string language { get; set; }

        [SearchableField(IsFilterable=true)]
        public string[] locations { get; set; }

        [SearchableField()]
        public string[] keyphrases { get; set; }

        [SearchableField()]
        public string[] imageTags { get; set; }

        [SearchableField()]
        public string[] imageCaption { get; set; }
    }

}