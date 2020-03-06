using BusLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BusInfo
{
    public class BusRoute
    {
        private static HttpClient client = new HttpClient();
        private static HttpResponseMessage response;
        private static RootElement re = null;
        public static async Task<IEnumerable<string>> GetBusStopList(int routeNumber)
        {
            response = await client.GetAsync($"https://mu-kgt.ru/informing/wap/marsh/?m={routeNumber}&action=getMarshData");
            if (response.IsSuccessStatusCode)
            {
                var resp = await response.Content.ReadAsStringAsync();
                if (resp == "[]")
                    return null;
                re = JsonConvert.DeserializeObject<RootElement>(resp);
                var busstopList = re.Line.ThereRoute.Select(x => x.Title).Where(x => x != null);
                return busstopList;
            }
            else return null;
        }

        public static async Task<int> ActiveBusesCount(int routeNumber, char direction='t')
        {
            int activeBusesCount=0;
            response = await client.GetAsync($"https://mu-kgt.ru/informing/wap/marsh/?m={routeNumber}&action=getMarshData");
            if (response.IsSuccessStatusCode)
            {
                var resp = await response.Content.ReadAsStringAsync();
                re = JsonConvert.DeserializeObject<RootElement>(resp);
                if (direction == 't')
                    activeBusesCount = re.Line.ThereRoute.Where(x => x.Type == "ts").Count();
                else activeBusesCount = re.Line.BackRoute.Where(x => x.Type == "ts").Count();
            }
            return activeBusesCount;
        }

        public static async Task<string> GetBusstopInfo(int mRoute, string busstopName, char dir = 't')
        {
            IEnumerable<BusstopItemModel> currentLine;
            response = await client.GetAsync($"https://mu-kgt.ru/informing/wap/marsh/?m={mRoute}&action=getMarshData");
            if (response.IsSuccessStatusCode)
            {
                var resp = await response.Content.ReadAsStringAsync();
                re = JsonConvert.DeserializeObject<RootElement>(resp);
            }
            if (dir == 't')
            {
                currentLine = re.Line.ThereRoute;
            }
            else currentLine = re.Line.BackRoute;
            var time = currentLine.Where(s => s.Title == busstopName).FirstOrDefault().TimeArrive;
            return $"Ваш автобус прибудет в {time}";
        }
    }
}
