using BusInfo.Models.JsonModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusInfo.Services
{
    public interface IBusInformationService
    {
        Task<IEnumerable<string>> GetBusStopList(int routeNumber, char direction='t');
        Task<int?> GetActiveBusesCount(int routeNumber, char direction='t');
        Task<string> GetBusArrivalTime(int routeNumber, string busStopName, char direction);
        Task<RoutesListModel> GetAllRoutes();
    }
}
