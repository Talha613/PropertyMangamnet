

using Newtonsoft.Json;

namespace PropertyManagement.Core.DTOs;
public class PaginationDto
{

        [JsonProperty("total_record")]
        public int TotalRecords { get; set; }
        [JsonProperty("filtered_record")]
        public int? FilteredRecords { get; set; }
        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

         [JsonProperty("current_page")]
        public int CurrentPage { get; set; }

        [JsonProperty("page_size")]
        public int PageSize { get; set; }

}

