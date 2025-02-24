

using Newtonsoft.Json;

namespace PropertyManagement.Core.DTOs.Sps;

    public class GetProjectTimelineByProjectIdForAdminDto
{
        [JsonProperty("project_timeline_id")]
        public int ProjectTimelineId { get; set; }

        [JsonProperty("project_id")]
        public int ProjectId { get; set; }

        [JsonProperty("timeline_config_id")]
        public int? TimeLineConfigId { get; set; } // Nullable for optional field

        [JsonProperty("date")]
        public DateTime? Date { get; set; } // Nullable for optional date

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("created_by")]
        public int CreatedBy { get; set; }

        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; } // Nullable for optional field

        [JsonProperty("updated_by")]
        public int? UpdatedBy { get; set; } // Nullable for optional field
}

