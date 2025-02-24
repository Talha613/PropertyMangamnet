using Newtonsoft.Json;

namespace PropertyManagement.Core.DTOs.Sps;


public class GetDevelopersDto
{
    [JsonProperty("developer_id")]
    public int DeveloperId { get; set; }
    [JsonProperty("developer_name")]
    public string DeveloperName { get; set; } = string.Empty;
    [JsonProperty("developer_logo")]
    public string DeveloperLogo { get; set; } = string.Empty;
    [JsonProperty("is_active")]
    public bool IsActive { get; set; }

}

public class GetDevelopersPaginatedDto
{

    [JsonProperty("pagination")]
    public PaginationDto? Pagination { get; set; }
    [JsonProperty("data")]
    public List<GetDevelopersDto>? Data { get; set; }
}
