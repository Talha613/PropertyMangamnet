
using Newtonsoft.Json;

namespace PropertyManagement.Core.DTOs.Sps;
public class GetCitiesDto
{
    [JsonProperty("city_id")]
    public int CityId { get; set; }

    [JsonProperty("country_id")]
    public int CountryId { get; set; }

    [JsonProperty("city_name")]
    public string CityName { get; set; } = string.Empty;

    [JsonProperty("is_active")]
    public bool IsActive { get; set; }

    [JsonProperty("create_by")]
    public int CreateBy { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("update_by")]
    public int? UpdateBy { get; set; }

    [JsonProperty("updated_at")]
    public DateTime? UpdatedAt { get; set; }
}

public class GetCitiesPaginatedDto
{
    [JsonProperty("pagination")]
    public PaginationDto? Pagination { get; set; }

    [JsonProperty("data")]
    public List<GetCitiesDto>? Data { get; set; }
}