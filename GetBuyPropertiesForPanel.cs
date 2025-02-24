
using Newtonsoft.Json;

namespace PropertyManagement.Core.DTOs.Sps;


public class GetBuyPropertiesForPanelDto
{
    [JsonProperty("buy_property_id")]
    public int BuyPropertyId { get; set; }

    [JsonProperty("short_description")]
    public string? ShortDescription { get; set; } = string.Empty;

    [JsonProperty("long_description")]
    public string? LongDescription { get; set; } = string.Empty;

    [JsonProperty("building_name")]
    public string? BuildingName { get; set; } = string.Empty;

    [JsonProperty("area_id")]
    public int AreaId { get; set; }

    [JsonProperty("longitude")]
    public double Longitude { get; set; }

    [JsonProperty("latitude")]
    public double Latitude { get; set; }

    [JsonProperty("square_feet")]
    public int SquareFeet { get; set; }

    [JsonProperty("square_meter")]
    public int SquareMeter { get; set; }

    [JsonProperty("bed_rooms")]
    public int BedRooms { get; set; }

    [JsonProperty("have_made_room")]
    public bool HaveMadeRoom { get; set; }

    [JsonProperty("bathrooms")]
    public int Bathrooms { get; set; }

    [JsonProperty("property_type_id")]
    public int PropertyTypeId { get; set; }

    [JsonProperty("is_active")]
    public bool IsActive { get; set; }

    [JsonProperty("is_approved")]
    public bool IsApproved { get; set; }

    [JsonProperty("approved_by")]
    public int? ApprovedBy { get; set; }

    [JsonProperty("create_by")]
    public int CreateBy { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("update_by")]
    public int? UpdateBy { get; set; }

    [JsonProperty("updated_at")]
    public DateTime? UpdatedAt { get; set; }
}




public class GetBuyPropertiesForPanelPaginatedDto
{
    [JsonProperty("pagination")]
    public PaginationDto? Pagination { get; set; }

    [JsonProperty("data")]
    public List<GetBuyPropertiesForPanelDto>? Data { get; set; }
}