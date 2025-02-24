

using Newtonsoft.Json;
using PropertyManagement.Core.DTOs.Shared;

namespace PropertyManagement.Core.DTOs.Sps;


public class GetNewProjectDetailsDto
{

    [JsonProperty("project_id")]
    public int ProjectId { get; set; }

    [JsonProperty("project_details")]
    public GetNewProjectDetailOfDetailsDto? ProjectDetails { get; set; }

    [JsonProperty("developer_info")]
    public DeveloperDto? DeveloperInfo { get; set; }

    [JsonProperty("media_info")]
    public List<MediaInfoDto>? MediaInfo { get; set; }
    [JsonProperty("payment_plan_info")]
    public List<GetProjectPaymentPLanDto>? PaymentPlanInfo { get; set; }

    [JsonProperty("time_line_info")]
    public List<GetProjectTimelineDto>? TimeLineInfo { get; set; }


    [JsonProperty("unit_info")]
    public List<GetProjectUnitDto>? UnitInfo { get; set; }


    [JsonProperty("amenities")]
    public List<KeyValueString>? Amenities { get; set; }

}
public class GetNewProjectDetailOfDetailsDto
{
    [JsonProperty("project_status")]
    public string? ProjectStatus { get; set; }
    [JsonProperty("short_description")]
    public string? ShortDescription { get; set; }
    [JsonProperty("long_description")]
    public string? LongDescription { get; set; }
    [JsonProperty("launch_price")]
    public string? LaunchPrice { get; set; }
    [JsonProperty("delivery_date")]
    public DateTime? DeliveryDate { get; set; }
    [JsonProperty("sales_start")]
    public DateTime? SalesStart { get; set; }
    [JsonProperty("location")]
    public string? Location { get; set; }
    [JsonProperty("number_of_buildings")]
    public int? NumberOfBuildings { get; set; }
    [JsonProperty("property_types")]
    public string? PropertyTypes { get; set; }
    [JsonProperty("gov_fee")]
    public decimal GovFee { get; set; }

}



public class GetProjectPaymentPLanDto
{
    [JsonProperty("id")]
    public int Id { get; set; }
    [JsonProperty("payment_plan_name")]
    public string PaymentPlanName { get; set; } = string.Empty;
    [JsonProperty("plan_description")]
    public string PlanDescription { get; set; } = string.Empty;
    [JsonProperty("plan_sub_description")]
    public string? PlanSubDescription { get; set; }
    [JsonProperty("plan_percentage")]
    public int PlanPercentage { get; set; }
}

public class GetProjectTimelineDto
{
    [JsonProperty("id")]
    public int Id { get; set; }
    [JsonProperty("date")]
    public DateTime? Date { get; set; }
    [JsonProperty("title")]
    public string Title { get; set; } = string.Empty;
}
public class GetProjectUnitDto
{
    [JsonProperty("project_unit_id")]

    public int ProjectUnitId { get; set; }
    [JsonProperty("property_type")]
    public string? PropertyType { get; set; }
    [JsonProperty("bed_type")]
    public string? BedType { get; set; }
    [JsonProperty("layout_type")]
    public string? LayoutType { get; set; }
    [JsonProperty("price")]
    public string? Price { get; set; }
    [JsonProperty("sqft")]

    public int Sqft { get; set; }
    [JsonProperty("floor_plan_url")]

    public string? FloorPlanUrl { get; set; }
}