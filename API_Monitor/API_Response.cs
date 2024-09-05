using Newtonsoft.Json;

namespace API_Monitor
{
    public class API_Response
    {
        [JsonProperty(PropertyName = "shop")]
        public string shop_type {  get; set; }

        [JsonProperty(PropertyName = "api_type")]
        public string api_type { get; set; }

        [JsonProperty(PropertyName = "status")]
        public bool status { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string url { get; set; }

        [JsonProperty(PropertyName = "code")]
        public string code { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string message { get; set; }
    }
}
