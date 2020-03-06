using Newtonsoft.Json;

namespace BusInfo.Models.JsonModels
{
    public class RouteItemModel
    {
        [JsonProperty("obj")]
        public string Type { get; set; }

        [JsonProperty("st_title")]
        public string Title { get; set; }

        [JsonProperty("st_arrive")]
        public string ArrivalTime { get; set; }

        [JsonProperty("ts_numb")]
        public string BusNumber { get; set; }

        [JsonProperty("ts_model")]
        public string BusModel { get; set; }

        [JsonProperty("ts_comment")]
        public string BusComment { get; set; }
    }
}
