
using Newtonsoft.Json;

namespace PropertyManagement.Core.DTOs.Shared;
public class BrokerInfoDto
{
    [JsonProperty("broker_name")]
    public string BrokerName { get; set; } = string.Empty;
    [JsonProperty("broker_logo")]
    public string BrokerLogo { get; set; } = string.Empty;
    [JsonProperty("phone")]
    public string Phone { get; set; } = string.Empty;
    [JsonProperty("whatsApp")]
    public string WhatsApp { get; set; } = string.Empty;
    [JsonProperty("email")]
    public string Email { get; set; } = string.Empty;
}