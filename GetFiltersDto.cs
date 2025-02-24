using PropertyManagement.Core.DTOs.Shared;
using Newtonsoft.Json;
namespace PropertyManagement.Core.DTOs.Sps;
public class GetFiltersDto
{

    [JsonProperty("property_type")]
    public List<KeyValue>? PropertyType { get; set; }

    [JsonProperty("amenities")]
    public List<KeyValue>? Amenities { get; set; }

    [JsonProperty("furnishing")]
    public List<KeyValue>? Furnishing { get; set; }
    [JsonProperty("completion_status")]
    public List<KeyValue>? CompletionStatus { get; set; }

}

