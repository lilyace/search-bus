using Newtonsoft.Json;

namespace BusInfo.Models.JsonModels
{
    public class RouteInfoModel
    {
        [JsonProperty("ts_type_id")]
        public int Id { get; set; }

        [JsonProperty("ts_type_title")]
        public string Type { get; set; }

        [JsonProperty("marsh_title")]
        public int RouteNumber { get; set; }

        [JsonProperty("forecast")]
        public string InformationTime { get; set; }

        [JsonProperty("ts_line")]
        public TrafficAndBusStopInfoModel TrafficAndBusStopInfo { get; set; }
    }
}
