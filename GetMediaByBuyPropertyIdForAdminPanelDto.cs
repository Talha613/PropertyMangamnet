
using Newtonsoft.Json;

namespace PropertyManagement.Core.DTOs.Sps;

public class GetMediaByBuyPropertyIdForAdminPanelDto
{

    [JsonProperty("buy_property_media_id")]
    public int BuyPropertyMediaId { get; set; }

    [JsonProperty("buy_property_id")]
    public int BuyPropertyId { get; set; }

    [JsonProperty("media_menu_id")]
    public int MediaMenuId { get; set; }

    [JsonProperty("media_url")]
    public string MediaUrl { get; set; } = string.Empty;

    [JsonProperty("media_description")]
    public string? MediaDescription { get; set; }

    [JsonProperty("media_type")]
    public string MediaType { get; set; } = string.Empty;

    [JsonProperty("create_by")]
    public int CreateBy { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

}



