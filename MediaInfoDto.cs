
using Newtonsoft.Json;

namespace PropertyManagement.Core.DTOs.Shared;
public class MediaInfoDto
{
    [JsonProperty("media_id")]
    public int MediaId { get; set; }
    [JsonProperty("media_type")]
    public string MediaType { get; set; } = string.Empty;
    [JsonProperty("media_url")]
    public string MediaUrl { get; set; } = string.Empty;
    [JsonProperty("media_menu")]
    public string MediaMenu { get; set; } = string.Empty;
    [JsonProperty("media_description")]
    public string MediaDescription { get; set; } = string.Empty;
}
