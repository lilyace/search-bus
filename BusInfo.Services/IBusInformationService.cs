using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusInfo.Services
{
    public interface IBusInformationService
    {
        Task<IEnumerable<string>> GetBusStopList(int routeNumber, char direction);
        Task<int> GetActiveBusesCount(int routeNumber, char direction);
        Task<string> GetBusArrivalTime(int routeNumber, string busStopName, char direction);
    }
}
