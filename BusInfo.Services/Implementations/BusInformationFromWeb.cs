using BusInfo.Models.JsonModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BusInfo.Models.DomainModels;

namespace BusInfo.Services.Implementations
{
    public class BusInformationFromWeb : IBusInformationService
    {
        private static HttpClient _client = new HttpClient();
        private static HttpResponseMessage _response;
        private static RouteInfoModel _rootElement;
        private static TrafficRouteInfoModel _routeInfo;

        public async Task<IEnumerable<string>> GetBusStopList(int routeNumber, char direction)
        {
            _routeInfo = await GetRouteInfo(routeNumber, direction);
            return _routeInfo?.BusStopList.Select(bs => bs.Name);
        }        

        public async Task<int> GetActiveBusesCount(int routeNumber, char direction = 't')
        {
            _routeInfo = await GetRouteInfo(routeNumber, direction);            
            return _routeInfo.ActiveBuses.Count();
        }

        public async Task<string> GetBusArrivalTime(int mRoute, string busStopName, char direction = 't')
        {
            _routeInfo = await GetRouteInfo(mRoute, direction);
            var time = _routeInfo.BusStopList.FirstOrDefault(bs => bs.Name == busStopName).ArrivalTime;
            return $"Ваш автобус прибудет в {time}";
        }

        private async Task<TrafficRouteInfoModel> GetRouteInfo(int routeNumber, char direction)
        {
            _response = await _client.GetAsync($"https://mu-kgt.ru/informing/wap/marsh/?m={routeNumber}&action=getMarshData");
            if (_response.IsSuccessStatusCode)
            {
                var resp = await _response.Content.ReadAsStringAsync();
                if (resp == "[]")
                    return null;
                _rootElement = JsonConvert.DeserializeObject<RouteInfoModel>(resp);
                var routeInfo = new TrafficRouteInfoModel();

                if (direction == 't')
                {
                    routeInfo.ActiveBuses = _rootElement.TrafficAndBusStopInfo.DirectRoute
                        .Where(x => x.Type == "ts")
                        .Select(x => new ActiveBusModel
                        {
                            Model = x.BusModel,
                            Number = x.BusNumber
                        });
                    routeInfo.BusStopList = _rootElement.TrafficAndBusStopInfo.DirectRoute
                        .Where(x => x.Type == "stop")
                        .Select(x => new BusStopModel
                        {
                            Name = x.Title,
                            ArrivalTime = x.ArrivalTime == " " ? new TimeSpan(0,0,0) : TimeSpan.Parse(x.ArrivalTime)
                        });
                }
                else
                {
                    routeInfo.ActiveBuses = _rootElement.TrafficAndBusStopInfo.ReturnRoute
                        .Where(x => x.Type == "ts")
                        .Select(x => new ActiveBusModel
                        {
                            Model = x.BusModel,
                            Number = x.BusNumber
                        });
                    routeInfo.BusStopList = _rootElement.TrafficAndBusStopInfo.ReturnRoute
                        .Where(x => x.Type == "stop")
                        .Select(x => new BusStopModel
                        {
                            Name = x.Title,
                            ArrivalTime = x.ArrivalTime == " " ? new TimeSpan(0, 0, 0) : TimeSpan.Parse(x.ArrivalTime)
                        });
                }
                return routeInfo;
            }
            return null;
        }

        public async Task<RoutesListModel> GetAllRoutes()
        {
            _response = await _client.GetAsync("https://mu-kgt.ru/informing/wap/marsh/?action=getListRoute");
            if (_response.IsSuccessStatusCode)
            {
                var textResponse = await _response.Content.ReadAsStringAsync();
                var allRoutes = JsonConvert.DeserializeObject<RoutesListModel>(textResponse);
                return allRoutes;
            }
            else return null;
        }
    }
}
