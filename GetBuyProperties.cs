using Newtonsoft.Json;
using PropertyManagement.Core.DTOs.Shared;

namespace PropertyManagement.Core.DTOs.Sps;
public class BuyPropertyDto
{

    [JsonProperty("buy_property_id")]
    public int BuyPropertyId { get; set; }
    [JsonProperty("property_details")]
    public BuyPropertyDetailsDto? PropertyDetails { get; set; }

    // Broker Information Section
    [JsonProperty("broker_info")]
    public BrokerInfoDto? BrokerInfo { get; set; }
    [JsonProperty("media_info")]

    // Media Information Section
    public List<MediaInfoDto>? MediaInfo { get; set; }

}
public class BuyPropertyDetailsDto
{

    [JsonProperty("property_type")]
    public int PropertyType { get; set; }
    [JsonProperty("price")]
    public decimal Price { get; set; }

    [JsonProperty("short_description")]
    public string ShortDescription { get; set; } = string.Empty;

    [JsonProperty("location")] 
    public string Location { get; set; } = string.Empty;

    [JsonProperty("bedrooms")]
    public int Bedrooms { get; set; }

    [JsonProperty("bathrooms")]
    public int Bathrooms { get; set; }

    [JsonProperty("square_feet")]
    public int SquareFeet { get; set; }

    [JsonProperty("square_meter")]
    public int SquareMeter { get; set; }

    [JsonProperty("listed")]
    public string Listed { get; set; } = string.Empty;
}



// Sub DTO for Pagination Information
public class BuyPropertyPaginatedDto
{


    [JsonProperty("pagination")]
    public PaginationDto? Pagination { get; set; }

    [JsonProperty("property_type_counts")]
    public List<GetBuyPropertiesAssociatedCountsDto>? PropertyTypeCounts { get; set; }
    [JsonProperty("city_counts")]
    public List<GetBuyPropertiesAssociatedCountsDto>? CityCounts { get; set; }

    [JsonProperty("data")]
    public List<BuyPropertyDto>? Data { get; set; }
}


public class GetBuyPropertiesAssociatedCountsDto
{
    [JsonProperty("id")]
    public int Id { get; set; }
    [JsonProperty("value")]
    public string Value { get; set; } = string.Empty;
    [JsonProperty("count")]
    public int Count { get; set; }

}