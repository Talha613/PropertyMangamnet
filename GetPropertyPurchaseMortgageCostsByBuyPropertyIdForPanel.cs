

using Newtonsoft.Json;

namespace PropertyManagement.Core.DTOs.Sps;

    public class GetPropertyPurchaseMortgageCostsByBuyPropertyIdForPanelDto
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

        [JsonProperty("created_by")]
        public int CreatedBy { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_by")]
        public int? UpdatedBy { get; set; }

        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }
}

