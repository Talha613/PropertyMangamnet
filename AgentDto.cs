using Newtonsoft.Json;

namespace PropertyManagement.Core.DTOs.Shared;
public class AgentInfoDto
{
    [JsonProperty("agent_phone")]
    public string Phone { get; set; } = string.Empty;
    [JsonProperty("profile_picture")]
    public string ProfilePicture { get; set; } = string.Empty;
    [JsonProperty("agent_name")]
    public string Name { get; set; } = string.Empty;
    [JsonProperty("agent_whatsapp")]
    public string WhatsApp { get; set; } = string.Empty;
    [JsonProperty("agent_email")]
    public string Email { get; set; } = string.Empty;
}





