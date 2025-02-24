
using Newtonsoft.Json;
using PropertyManagement.Core.DTOs.Shared;

namespace PropertyManagement.Core.DTOs.Sps;

public class GetNewProjectsDto
{

    [JsonProperty("project_id")]
    public int ProjectId { get; set; }
    [JsonProperty("project_details")]
    public GetNewProjectsDetailsDto? ProjectDetails { get; set; }

    [JsonProperty("media_info")]
    public List<MediaInfoDto>? MediaInfo { get; set; }

}
public class GetNewProjectsDetailsDto
{


    [JsonProperty("project_status")]
    public string ProjectStatus { get; set; } = string.Empty;

    [JsonProperty("short_description")]
    public string ShortDescription { get; set; } = string.Empty;

    [JsonProperty("launch_price")]
    public string LaunchPrice { get; set; } = string.Empty;

    [JsonProperty("delivery_date")]
    public string DeliveryDate { get; set; } = string.Empty;

    [JsonProperty("developer_name")]
    public string DeveloperName { get; set; } = string.Empty;

    [JsonProperty("location")] public string Location { get; set; } = string.Empty;


}



public class NewProjectsPaginatedDto
{


    [JsonProperty("pagination")]
    public PaginationDto? Pagination { get; set; }


    [JsonProperty("data")]
    public List<GetNewProjectsDto>? Data { get; set; }
}


