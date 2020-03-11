using Newtonsoft.Json;

namespace BusInfo.Models.JsonModels
{
    public class DirectionModel
    {
        [JsonProperty("first")]
        public string FirstBusStop { get; set; }

        [JsonProperty("last")]
        public string LastBusStop { get; set; }
    }
}
