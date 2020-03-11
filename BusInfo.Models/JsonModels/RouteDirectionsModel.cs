using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusInfo.Models.JsonModels
{
    public class RouteDirectionsModel
    {
        public string Name { get; set; }

        [JsonProperty("A")]
        public DirectionModel DirectRoute { get; set; }

        [JsonProperty("B")]
        public DirectionModel ReturnRoute { get; set; }
    }


}

