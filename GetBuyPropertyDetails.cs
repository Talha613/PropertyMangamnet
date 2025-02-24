
using Newtonsoft.Json;
using PropertyManagement.Core.DTOs.Shared;

namespace PropertyManagement.Core.DTOs.Sps;
public class GetBuyPropertyDetailDto
{

    [JsonProperty("buy_property_id")]
    public int BuyPropertyId { get; set; }
    [JsonProperty("property_details")]
    public GetBuyPropertyDetailOfDetailsDto? PropertyDetails { get; set; }

    [JsonProperty("property_payments_cash")]
    public GetBuyPropertyPurchaseCostsCashDto? BuyPropertyPurchaseCostsCashDto { get; set; }

    [JsonProperty("property_payments_mortgage")]
    public GetBuyPropertyPurchaseMortgageCostsDto? BuyPropertyPurchaseMortgageCostsDto { get; set; }

    [JsonProperty("broker_info")]
    public BrokerInfoDto? BrokerInfo { get; set; }
    [JsonProperty("agent_info")]
    public AgentInfoDto? AgentInfoDto { get; set; }

    [JsonProperty("media_info")]
    public List<MediaInfoDto>? MediaInfo { get; set; }
    [JsonProperty("amenities")]

    public List<KeyValueString>? Amenities { get; set; }

}

public class GetBuyPropertyDetailOfDetailsDto
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

    [JsonProperty("listed")]
    public string Listed { get; set; } = string.Empty;
}

public class GetBuyPropertyPurchaseCostsCashDto
{
    [JsonProperty("cash_cost_id")]
    public int CashCostId { get; set; }

    [JsonProperty("buy_property_id")]
    public int BuyPropertyId { get; set; }

    [JsonProperty("purchase_price")]
    public decimal PurchasePrice { get; set; }

    [JsonProperty("land_dept_fee_percentage")]
    public decimal LandDeptFeePercentage { get; set; }

    [JsonProperty("agency_fee_percentage")]
    public decimal AgencyFeePercentage { get; set; }

    [JsonProperty("agency_fee_vat_percentage")]
    public decimal AgencyFeeVatPercentage { get; set; }

    [JsonProperty("trustee_fee")]
    public decimal TrusteeFee { get; set; }

    [JsonProperty("conveyancer_fee")]
    public decimal ConveyancerFee { get; set; }

    [JsonProperty("total_cost")]
    public decimal TotalCost { get; set; }
}

public class GetBuyPropertyPurchaseMortgageCostsDto
{
    [JsonProperty("mortgage_cost_id")]
    public int MortgageCostId { get; set; }

    [JsonProperty("buy_property_id")]
    public int BuyPropertyId { get; set; }

    [JsonProperty("purchase_price")]
    public decimal PurchasePrice { get; set; }

    [JsonProperty("down_payment")]
    public decimal DownPayment { get; set; }

    [JsonProperty("land_dept_fee")]
    public decimal LandDeptFee { get; set; }

    [JsonProperty("trustee_fee")]
    public decimal TrusteeFee { get; set; }

    [JsonProperty("mortgage_registration_fee")]
    public decimal MortgageRegistrationFee { get; set; }

    [JsonProperty("agency_fee")]
    public decimal AgencyFee { get; set; }

    [JsonProperty("bank_arrangement_fee")]
    public decimal BankArrangementFee { get; set; }

    [JsonProperty("valuation_fee")]
    public decimal ValuationFee { get; set; }

    [JsonProperty("amount_required_upfront")]
    public decimal AmountRequiredUpfront { get; set; }
}
