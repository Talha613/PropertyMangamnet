
using Newtonsoft.Json;

namespace PropertyManagement.Core.DTOs.Sps;


public class GetAgentUsersDto
{

        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("email")]
        public string Email { get; set; } = string.Empty;

        [JsonProperty("experience")]
        public decimal Experience { get; set; }

        [JsonProperty("profile_picture")]
        public string ProfilePicture { get; set; } = string.Empty;

        [JsonProperty("phone")]
        public string Phone { get; set; } = string.Empty;

        [JsonProperty("whats_app")]
        public string WhatsApp { get; set; } = string.Empty;

        [JsonProperty("real_estate_broker_id")]
        public int RealEstateBrokerId { get; set; }

        [JsonProperty("is_active")]
        public bool IsActive { get; set; }

        [JsonProperty("create_by")]
        public int CreateBy { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("update_by")]
        public string? UpdateBy { get; set; }

        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }



public class GetAgentUsersPaginatedDto
{

    [JsonProperty("pagination")]
    public PaginationDto? Pagination { get; set; }
    [JsonProperty("data")]
    public List<GetAgentUsersDto>? Data { get; set; }
}
