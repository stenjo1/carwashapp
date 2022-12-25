using CarWashApp.Controllers;
using CarWashApp.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWashApp.Tests.UnitTests
{
    [TestClass]
    public class CarWashControllerTests : BaseTests
    {
        [TestMethod]
        public async Task GetCustomerCarWashes()
        {
            var databaseName = Guid.NewGuid().ToString();
            SeedData(databaseName);

            var carWashFilter = new CarWashFilterDTO()
            {
                Page = 1,
                RecordsPerPage = 1,
                DistanceInKms = 10
            };

            var controller = BuildCarWashController(databaseName);
            controller.ControllerContext = BuildControllerContextWithDefaultCustomer();

            var response = await controller.GetCarWashes(carWashFilter);
            Assert.IsNotNull(response);

            var result = response.Value;
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public async Task GetOwnerCarWashesNotFound()
        {
            var databaseName = Guid.NewGuid().ToString();
            SeedData(databaseName);

            var context = BuildContext(databaseName);
            var carWash = context.CarWashes.First();
            carWash.OwnerId = "owner2";

            context.SaveChanges();

            var controller = BuildCarWashController(databaseName);
            controller.ControllerContext = BuildControllerContextWithDefaultOwner();

            var response = await controller.GetOwnerCarWashes(new CarWashFilterDTO());
            var result = response.Value;
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task GetOwnerCarWashes()
        {
            var databaseName = Guid.NewGuid().ToString();
            SeedData(databaseName);

            var controller = BuildCarWashController(databaseName);
            controller.ControllerContext = BuildControllerContextWithDefaultOwner();

            var response = await controller.GetOwnerCarWashes(new CarWashFilterDTO());
            var result = response.Value;
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public async Task GetCarWashRevenueFilter()
        {
            var databaseName = Guid.NewGuid().ToString();
            SeedData(databaseName);

            var controller = BuildCarWashController(databaseName);
            controller.ControllerContext = BuildControllerContextWithDefaultOwner();

            var revenueFilter = new RevenueFilterCreationDTO()
            {
                Id = 1,
                Year = DateTime.Today.Year,
                Month = DateTime.Today.Month,
                Day = DateTime.Today.Day
            }; 

            var response = await controller.GetCarWashRevenueFilter(revenueFilter);
            var result = response.Value;
            Assert.AreEqual(100, result.TotalRevenue);
        }

        private CarWashController BuildCarWashController(string databaseName)
        {
            var context = BuildContext(databaseName);
            var myUserStore = new UserStore<IdentityUser>(context);
            var userManager = BuildUserManager(myUserStore);
            var mapper = BuildMap();

            var httpcontext = new DefaultHttpContext();
            MockAuth(httpcontext);

            return new CarWashController(context, mapper, userManager);
        }
    }
}
