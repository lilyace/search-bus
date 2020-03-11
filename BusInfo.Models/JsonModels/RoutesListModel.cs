using BusInfo.Models.JsonModels.CustomConverter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusInfo.Models.JsonModels
{
    public class RoutesListModel
    {
        [JsonProperty("1")]
        [JsonConverter(typeof(CustomJSONConverter))]
        public IEnumerable<RouteDirectionsModel> Buses{ get; set; }
    }
}
