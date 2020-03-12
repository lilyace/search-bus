using BusInfo.Models.JsonModels;
using BusInfo.Services;
using BusInfo.Web.Controllers;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusInfo.Web.Tests
{
    [TestFixture]
    public class BusTests
    {
        [Test]
        public async Task RoutesList_GetAllRoutes_IsSuccess()
        {
            //Arrange
            var routes = new RoutesListModel()
            {
                Buses = Enumerable.Repeat(default(RouteDirectionsModel), 5)
            };
            var mock = new Mock<IBusInformationService>();
            mock.Setup(x => x.GetAllRoutes()).ReturnsAsync(routes);
            var controller = new BusController(mock.Object);

            //Act
            var result = await controller.RoutesList();
            //Assert
            Assert.IsInstanceOf<IEnumerable<RouteDirectionsModel>>(result.Model);
            CollectionAssert.AreEqual(routes.Buses, (IEnumerable<RouteDirectionsModel>)result.Model);

        }

        [Test]
        public async Task BusStopList_GetListByExistingNumber_IsSuccess()
        {
            //Arrange
            int routeNumber = 2;
            var busStopes = Enumerable.Repeat("bus stop name", 6);
            var mock = new Mock<IBusInformationService>();
            mock.Setup(x => x.GetBusStopList(routeNumber, 't')).ReturnsAsync(busStopes);
            var controller = new BusController(mock.Object);

            //Act
            var result = await controller.BusStopList(routeNumber);

            //Assert
            Assert.IsInstanceOf<IEnumerable<string>>(result.Model);
            CollectionAssert.AreEqual(busStopes, (IEnumerable<string>)result.Model);
        }

        [Test]
        public async Task BusStopList_GetListByInvalidNumber_ReturnNull()
        {
            //Arrange
            int routeNumber = -1;
            var mock = new Mock<IBusInformationService>();
            mock.Setup(x => x.GetBusStopList(routeNumber, 't')).ReturnsAsync((IEnumerable<string>)null);
            var controller = new BusController(mock.Object);
            //Act
            var result = await controller.BusStopList(routeNumber);

            //Assert
            Assert.IsNull(result.Model);
        }

        [Test]
        public async Task ActiveBuses_GetCountByInvalidNumber_ReturnNull()
        {
            //Arrange
            int routeNumber = -1;
            var mock = new Mock<IBusInformationService>();
            mock.Setup(x => x.GetActiveBusesCount(routeNumber, 't')).ReturnsAsync((int?)null);
            var controller = new BusController(mock.Object);

            //Act
            var result = await controller.ActiveBuses(routeNumber);

            //Assert
            Assert.IsNull(result.Model);
        }
    }
}