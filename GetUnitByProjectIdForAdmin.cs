

using Newtonsoft.Json;

namespace PropertyManagement.Core.DTOs.Sps;

public class GetUnitByProjectIdForAdminDto
{
    [JsonProperty("project_unit_id")]
    public int ProjectUnitId { get; set; }

    [JsonProperty("project_id")]
    public int ProjectId { get; set; }

    [JsonProperty("property_type_id")]
    public int PropertyTypeId { get; set; }

    [JsonProperty("bed")]
    public int Bed { get; set; }

    [JsonProperty("layout_type")]
    public string LayoutType { get; set; } = string.Empty;

    [JsonProperty("price")]
    public decimal? Price { get; set; }

    [JsonProperty("sqft")]
    public int Sqft { get; set; }

    [JsonProperty("floor_plan_url")]
    public string? FloorPlanUrl { get; set; }

    [JsonProperty("create_at")]
    public DateTime CreateAt { get; set; }

    [JsonProperty("create_by")]
    public int CreateBy { get; set; }

    [JsonProperty("update_at")]
    public DateTime? UpdateAt { get; set; }

    [JsonProperty("update_by")]
    public int? UpdateBy { get; set; }
}
