

using Newtonsoft.Json;

namespace PropertyManagement.Core.DTOs.Sps;

    public class GetPropertyPurchaseCostsCashByBuyPropertyIdForPanelDto
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

        [JsonProperty("created_by")]
        public int CreatedBy { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_by")]
        public int? UpdatedBy { get; set; }

        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }
}

