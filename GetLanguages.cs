
using Newtonsoft.Json;

namespace PropertyManagement.Core.DTOs.Sps;
public class GetLanguagesDto
{
    [JsonProperty("language_id")]
    public int LanguageId { get; set; }

    [JsonProperty("language_code")]
    public string LanguageCode { get; set; } = string.Empty;

    [JsonProperty("language_name")]
    public string LanguageName { get; set; } = string.Empty;

    [JsonProperty("is_active")]
    public bool IsActive { get; set; }
    [JsonProperty("create_at")]
    public DateTime CreatedAt { get; set; }
    [JsonProperty("update_at")]
    public DateTime? UpdatedAt { get; set; }

}

public class GetLanguagesPaginatedDto
{
    [JsonProperty("pagination")]
    public PaginationDto? Pagination { get; set; }

    [JsonProperty("data")]
    public List<GetLanguagesDto>? Data { get; set; }
}
