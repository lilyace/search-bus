using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BusInfo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace BusInfo.Web.Controllers
{
    public class BusController : Controller
    {
        private readonly IBusInformationService _busInformationService;
        public BusController(IBusInformationService busInformationService)
        {
            _busInformationService = busInformationService;
        }

        public async Task<ViewResult> RoutesList()
        {
            return View((await _busInformationService.GetAllRoutes()).Buses);
        }

        public async Task<IActionResult> BusStopList(int routeNum)
        {
            ViewBag.RouteNumber = routeNum;
            var list = await _busInformationService.GetBusStopList(routeNum);
            return View(list);
        }
    }
}
