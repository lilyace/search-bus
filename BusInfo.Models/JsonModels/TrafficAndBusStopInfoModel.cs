using Newtonsoft.Json;
using System.Collections.Generic;

namespace BusInfo.Models.JsonModels
{
    public class TrafficAndBusStopInfoModel
    {
        [JsonProperty("A")]
        public IEnumerable<RouteItemModel> DirectRoute { get; set; }

        [JsonProperty("B")]
        public IEnumerable<RouteItemModel> ReturnRoute { get; set; }
    }
}
