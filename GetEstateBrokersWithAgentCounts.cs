using Newtonsoft.Json;

namespace PropertyManagement.Core.DTOs.Sps;

    public class GetEstateBrokersWithAgentCountDto
    {
        [JsonProperty("broker_id")]
        public int RealEstateBrokerId { get; set; }
        [JsonProperty("broker_name")]
        public string BrokerName { get; set; } = string.Empty;
        [JsonProperty("orn_number")]
        public int Orn { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; } = string.Empty;
        [JsonProperty("broker_email")]
        public string BrokerEmail { get; set; } = string.Empty;
        [JsonProperty("broker_phone")]
        public string BrokerPhone { get; set; } = string.Empty;
        [JsonProperty("logo")]
        public string Logo { get; set; } = string.Empty;
        [JsonProperty("about")]
        public string About { get; set; } = string.Empty;
        [JsonProperty("agent_count")]
        public int AgentCount { get; set; }
    }

    public class GetEstateBrokersWithAgentCountDtoPaginatedDto
    {

        [JsonProperty("pagination")]
        public PaginationDto? Pagination { get; set; }
        [JsonProperty("data")]
        public List<GetEstateBrokersWithAgentCountDto>? Data { get; set; }
    }
