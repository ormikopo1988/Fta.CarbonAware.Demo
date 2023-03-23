using System.Text.Json.Serialization;
using System;

namespace Fta.CarbonAware.Web.Models
{
    [Serializable]
    public class IpInfo
    {
        [JsonPropertyName("ip")]
        public string Ip { get; set; } = default!;

        [JsonPropertyName("hostname")]
        public string Hostname { get; set; } = default!;

        [JsonPropertyName("city")]
        public string City { get; set; } = default!;

        [JsonPropertyName("region")]
        public string Region { get; set; } = default!;

        [JsonPropertyName("country")]
        public string Country { get; set; } = default!;

        [JsonPropertyName("loc")]
        public string Loc { get; set; } = default!;

        [JsonPropertyName("org")]
        public string Org { get; set; } = default!;
        
        [JsonPropertyName("timezone")]
        public string Timezone { get; set; } = default!;
    }
}
