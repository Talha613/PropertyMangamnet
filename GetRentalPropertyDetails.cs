
using Newtonsoft.Json;
using PropertyManagement.Core.DTOs.Shared;

namespace PropertyManagement.Core.DTOs.Sps;
public class GetRentPropertyDetailDto
{

    [JsonProperty("rental_property_id")]
    public int RentalPropertyId { get; set; }

    [JsonProperty("property_details")]
    public RentPropertyDetailOfDetailsDto? PropertyDetails { get; set; }

    [JsonProperty("rental_property_costs")]
    public RentalPropertyCostsDto? GetRentalPropertyCosts { get; set; }

    [JsonProperty("broker_info")]
    public BrokerInfoDto? BrokerInfo { get; set; }
    [JsonProperty("agent_info")]
    public AgentInfoDto? AgentInfoDto { get; set; }

    [JsonProperty("media_info")]
    public List<MediaInfoDto>? MediaInfo { get; set; }
    [JsonProperty("amenities")]
    public List<KeyValueString>? Amenities { get; set; }

}
public class RentPropertyDetailOfDetailsDto
{

    [JsonProperty("property_type")]
    public string PropertyType { get; set; } = string.Empty;

    [JsonProperty("short_description")]
    public string ShortDescription { get; set; } = string.Empty;
    [JsonProperty("long_description")]
    public string LongDescription { get; set; } = string.Empty;

    [JsonProperty("location")]
    public string Location { get; set; } = string.Empty;

    [JsonProperty("bedrooms")]
    public int Bedrooms { get; set; }

    [JsonProperty("have_made_room")]
    public bool HaveMadeRoom { get; set; }

    [JsonProperty("bathrooms")]
    public int Bathrooms { get; set; }

    [JsonProperty("square_feet")]
    public int SquareFeet { get; set; }

    [JsonProperty("square_meter")]
    public int SquareMeter { get; set; }

    [JsonProperty("available_from")]
    public DateTime? AvailableFrom { get; set; }

    [JsonProperty("listed")]
    public string Listed { get; set; } = string.Empty;
}

public class RentalPropertyCostsDto
{
    [JsonProperty("upfront_cost_id")]
    public int UpfrontCostId { get; set; }

    [JsonProperty("rent_property_id")]
    public int RentPropertyId { get; set; }

    [JsonProperty("annual_rent")]
    public decimal AnnualRent { get; set; }

    [JsonProperty("agency_fee_percentage")]
    public decimal AgencyFeePercentage { get; set; }

    [JsonProperty("agency_fee_vat_percentage")]
    public decimal AgencyFeeVatPercentage { get; set; }

    [JsonProperty("security_deposit")]
    public decimal SecurityDeposit { get; set; }

    [JsonProperty("dewa_deposit")]
    public decimal DewaDeposit { get; set; }

    [JsonProperty("ejari_fee")]
    public decimal EjariFee { get; set; }

    [JsonProperty("total_upfront_costs")]
    public decimal TotalUpfrontCosts { get; set; }
}

