using Newtonsoft.Json;


namespace PropertyManagement.Core.DTOs.Sps;
public class GetAgentsListDto
{
    [JsonProperty("user_id")]
    public int UserId { get; set; }
    [JsonProperty("agent_name")]
    public string AgentName { get; set; } = String.Empty;
    [JsonProperty("agent_email")]
    public string AgentEmail { get; set; } = String.Empty;
    [JsonProperty("experience")]
    public decimal Experience { get; set; }
    [JsonProperty("profile_picture")]
    public string ProfilePicture { get; set; } = String.Empty;
    [JsonProperty("phone")]
    public string Phone { get; set; } = String.Empty;
    [JsonProperty("whatsapp")]
    public string WhatsApp { get; set; } = String.Empty;
    [JsonProperty("broker_id")]
    public int RealEstateBrokerId { get; set; }
}

public class GetAgentsListPaginatedDto
{

    [JsonProperty("pagination")]
    public PaginationDto? Pagination { get; set; }
    [JsonProperty("data")]
    public List<GetAgentsListDto>? Data { get; set; }
}
