
using Newtonsoft.Json;

namespace PropertyManagement.Core.DTOs.Sps;
public class GetAreasDto
{
    [JsonProperty("area_id")]
    public int AreaId { get; set; }

    [JsonProperty("city_id")]
    public int CityId { get; set; }

    [JsonProperty("area_name")]
    public string AreaName { get; set; } = string.Empty;

    [JsonProperty("postal_code")]
    public string PostalCode { get; set; } = string.Empty;

    [JsonProperty("is_active")]
    public bool IsActive { get; set; }
    [JsonProperty("create_at")]
    public DateTime CreatedAt { get; set; }
    [JsonProperty("update_at")]
    public DateTime? UpdatedAt { get; set; }
}

public class GetAreasPaginatedDto
{
    [JsonProperty("pagination")]
    public PaginationDto? Pagination { get; set; }

    [JsonProperty("data")]
    public List<GetAreasDto>? Data { get; set; }
}