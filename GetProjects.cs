
using Newtonsoft.Json;

namespace PropertyManagement.Core.DTOs.Sps;


public class GetProjectsDto
{
    [JsonProperty("project_id")]
    public int ProjectId { get; set; }

    [JsonProperty("project_status")]
    public int ProjectStatus { get; set; }

    [JsonProperty("developer_id")]
    public int DeveloperId { get; set; }

    [JsonProperty("area_id")]
    public int AreaId { get; set; }

    [JsonProperty("community_name")]
    public string? CommunityName { get; set; } = string.Empty;

    [JsonProperty("gov_fee")]
    public decimal? GovFee { get; set; }

    [JsonProperty("short_description")]
    public string? ShortDescription { get; set; } = string.Empty;

    [JsonProperty("long_description")]
    public string? LongDescription { get; set; } = string.Empty;

    [JsonProperty("launch_price")]
    public decimal? LaunchPrice { get; set; }

    [JsonProperty("delivery_date")]
    public DateTime? DeliveryDate { get; set; }

    [JsonProperty("sales_start")]
    public DateTime? SalesStart { get; set; }

    [JsonProperty("number_of_buildings")]
    public int? NumberOfBuildings { get; set; }

    [JsonProperty("property_type_id")]
    public int? PropertyTypeId { get; set; }


    [JsonProperty("create_by")]
    public int CreateBy { get; set; }

    [JsonProperty("create_at")]
    public DateTime CreateAt { get; set; }

    [JsonProperty("update_by")]
    public int? UpdateBy { get; set; }

    [JsonProperty("update_at")]
    public DateTime? UpdateAt { get; set; }
}
public class GetProjectsPaginatedDto
{

    [JsonProperty("pagination")]
    public PaginationDto? Pagination { get; set; }
    [JsonProperty("data")]
    public List<GetProjectsDto>? Data { get; set; }
}


