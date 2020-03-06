using System;

namespace BusInfo.Models.DomainModels
{
    public class BusStopModel
    {
        public string Name { get; set; }
        public TimeSpan ArrivalTime { get; set; }
    }
}
