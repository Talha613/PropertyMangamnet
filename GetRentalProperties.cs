using Newtonsoft.Json;
using PropertyManagement.Core.DTOs.Shared;

namespace PropertyManagement.Core.DTOs.Sps;
public class RentalPropertyDto
{
    [JsonProperty("rental_property_id")]
    public int RentalPropertyId { get; set; }
    [JsonProperty("property_details")]
    public RentalPropertyDetailsDto? PropertyDetails { get; set; }

    // Broker Information Section
    [JsonProperty("broker_info")]
    public BrokerInfoDto? BrokerInfo { get; set; }
    [JsonProperty("media_info")]

    // Media Information Section
    public List<MediaInfoDto>? MediaInfo { get; set; }

}

// Sub DTO for Property Details
public class RentalPropertyDetailsDto
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


// Sub DTO for Media Information

// Sub DTO for Pagination Information
public class RentalPropertyPaginatedDto
{

    [JsonProperty("pagination")]
    public PaginationDto? Pagination { get; set; }
    [JsonProperty("property_type_counts")]
    public List<GetRentalPropertiesAssociatedCountsDto>? PropertyTypeCounts { get; set; }
    [JsonProperty("city_counts")]
    public List<GetRentalPropertiesAssociatedCountsDto>? CityCounts { get; set; }
    [JsonProperty("data")]
    public List<RentalPropertyDto>? Data { get; set; }
}


public class GetRentalPropertiesAssociatedCountsDto
{
    [JsonProperty("id")]
    public int Id { get; set; }
    [JsonProperty("value")]
    public string Value { get; set; } = string.Empty;
    [JsonProperty("count")]
    public int Count { get; set; }

}