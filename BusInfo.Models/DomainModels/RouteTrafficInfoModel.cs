using System.Collections.Generic;

namespace BusInfo.Models.DomainModels
{
    public class TrafficRouteInfoModel
    {
        public IEnumerable<ActiveBusModel> ActiveBuses { get; set; }
        public IEnumerable<BusStopModel> BusStopList { get; set; }
    }
}
