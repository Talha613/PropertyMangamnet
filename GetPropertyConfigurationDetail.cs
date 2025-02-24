using Newtonsoft.Json;
namespace PropertyManagement.Core.DTOs.Sps;

public class GetPropertyConfigurationDetailDto
{
    [JsonProperty("config_detail_id")]
    public int ConfigDetailId { get; set; }
    [JsonProperty("config_master_id")]
    public int ConfigMasterId { get; set; }
    [JsonProperty("value")]
    public string? Value { get; set; }
    [JsonProperty("second_value")]
    public string? SecondValue { get; set; }
    [JsonProperty("image_url")]
    public string? ImageUrl { get; set; }
    [JsonProperty("is_active")]
    public bool IsActive { get; set; }
    [JsonProperty("create_by")]
    public int? CreateBy { get; set; }
    [JsonProperty("create_at")]
    public DateTime CreatedAt { get; set; }
    [JsonProperty("update_by")]
    public int? UpdateBy { get; set; }
    [JsonProperty("update_at")]
    public DateTime? UpdatedAt { get; set; }
}

public class GetPropertyConfigurationDetailPaginatedDto
{

    [JsonProperty("pagination")]
    public PaginationDto? Pagination { get; set; }
    [JsonProperty("data")]
    public List<GetPropertyConfigurationDetailDto>? Data { get; set; }
}
