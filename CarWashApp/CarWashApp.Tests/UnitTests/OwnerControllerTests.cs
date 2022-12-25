using CarWashApp.Controllers;
using CarWashApp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWashApp.Tests.UnitTests
{
    [TestClass]
    public class OwnerControllerTests : BaseTests
    {
        [TestMethod]
        public async Task GetTotalIncomeForLoggedInOwner()
        {
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);

            var owner = new Owner()
            {
                Id = "owner",
                UserName = "test.owner",
                NormalizedUserName = "TEST.OWNER",
                Email = "test.owner@test.com",
                NormalizedEmail = "TEST.OWNER@TEST.COM",
                PasswordHash = "QUDCBSPFTEUSBW183@*&$nALSAHJ1",
                FirstName = "Test",
                LastName = "Owner",
                PhoneNumber = "060123456",
                Gender = 0,
                DateOfBirth = new DateTime(2000, 12, 31)
            };

            var carWash = new CarWash()
            {
                Id = 1,
                OwnerId = "owner",
                Name = "CarWash",
                Size = 1,
                OpeningHour = 8,
                ClosingHour = 20,
                Latitude = 44.0000,
                Longitude = 20.0000
            };
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            carWash.Location = geometryFactory.CreatePoint(new Coordinate(carWash.Latitude, carWash.Longitude));

            var revenue = new Revenue()
            {
                CarWashId = 1,
                DailyIncome = 0,
                WeeklyIncome = 0,
                MonthlyIncome = 1000
            };

            context.Owners.Add(owner);
            context.CarWashes.Add(carWash);
            context.Revenues.Add(revenue);
            context.SaveChanges();

            var controller = BuildCustomerController(databaseName);
            controller.ControllerContext = BuildControllerContextWithDefaultOwner();

            var response = await controller.GetTotalIncome();
            Assert.AreEqual(1000, response.Value);
        }


        private OwnerController BuildCustomerController(string databaseName)
        {
            var context = BuildContext(databaseName);
            var myUserStore = new UserStore<IdentityUser>(context);
            var userManager = BuildUserManager(myUserStore);
            var mapper = BuildMap();

            var httpcontext = new DefaultHttpContext();
            MockAuth(httpcontext);

            return new OwnerController(context, mapper, userManager);
        }
    }
}
