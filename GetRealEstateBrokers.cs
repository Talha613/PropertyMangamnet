

using Newtonsoft.Json;


namespace PropertyManagement.Core.DTOs.Sps;

public class GetRealEstateBrokersDto
{
    [JsonProperty("real_estate_broker_id")]
    public int RealEstateBrokerId { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("orn")]
    public int Orn { get; set; }

    [JsonProperty("office")]
    public string Office { get; set; } = string.Empty;

    [JsonProperty("building")]
    public string Building { get; set; } = string.Empty;

    [JsonProperty("area_id")]
    public int AreaId { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; } = string.Empty;

    [JsonProperty("phone")]
    public string Phone { get; set; } = string.Empty;

    [JsonProperty("logo")]
    public string Logo { get; set; } = string.Empty;

    [JsonProperty("about")]
    public string About { get; set; } = string.Empty;

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("create_by")]
    public int? CreateBy { get; set; }

    [JsonProperty("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [JsonProperty("update_by")]
    public string? UpdateBy { get; set; }

}

public class GetRealEstateBrokersPaginatedDto
{

    [JsonProperty("pagination")]
    public PaginationDto? Pagination { get; set; }
    [JsonProperty("data")]
    public List<GetRealEstateBrokersDto>? Data { get; set; }
}




