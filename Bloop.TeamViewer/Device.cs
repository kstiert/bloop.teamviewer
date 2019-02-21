using Newtonsoft.Json;

namespace Bloop.TeamViewer
{
    public class Device
    {
        [JsonProperty("device_id")]
        public string DeviceId { get; set; }

        [JsonProperty("remotecontrol_id")]
        public string RemoteControlId { get; set; }

        public string GroupId { get; set; }

        public string Alias { get; set; }

        public string Description { get; set; }

        [JsonProperty("online_state")]
        public string Online { get; set; }

        [JsonProperty("supported_features")]
        public string SupportedFeatures { get; set; }

        [JsonProperty("assinged_to")]
        public string AssignedTo { get; set; }

        [JsonProperty("policy_id")]
        public string Policy { get; set; }

        [JsonProperty("last_seen")]
        public string LastSeen { get; set; }
    }
}
