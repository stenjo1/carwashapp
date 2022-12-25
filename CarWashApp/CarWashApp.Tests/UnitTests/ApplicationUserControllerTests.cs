using CarWashApp.Controllers;
using CarWashApp.DTOs;
using CarWashApp.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace CarWashApp.Tests.UnitTests
{
    [TestClass]
    public class ApplicationUserControllerTests : BaseTests
    {
        [TestMethod]
        public async Task CustomerIsCreated()
        {
            var databaseName = Guid.NewGuid().ToString();
            await CreateCustomer(databaseName);

            var context2 = BuildContext(databaseName);
            var customerCount = await context2.Customers.CountAsync();
            var ownerCount = await context2.Owners.CountAsync();

            Assert.AreEqual(1, customerCount);
            Assert.AreEqual(0, ownerCount);
        }

        [TestMethod]
        public async Task OwnerIsCreated()
        {
            var databaseName = Guid.NewGuid().ToString();
            await CreateOwner(databaseName);

            var context2 = BuildContext(databaseName);
            var customerCount = await context2.Customers.CountAsync();
            var ownerCount = await context2.Owners.CountAsync();

            Assert.AreEqual(0, customerCount);
            Assert.AreEqual(1, ownerCount);
        }

        //analogno za ownera
        [TestMethod]
        public async Task CustomerCanLogin()
        {
            var databaseName = Guid.NewGuid().ToString();
            await CreateCustomer(databaseName);

            var controller = BuildApplicationUserController(databaseName);
            controller.ControllerContext = BuildControllerContextWithDefaultCustomer(); 
            var customerInfo = new ApplicationUserSignInDTO() { UserName = "test.customer", Password = "Test.1" };

            var response = await controller.LoginUser(customerInfo);

            Assert.IsNotNull(response.Value);
        }

        [TestMethod]
        public async Task CustomerCanNotLogin()
        {
            var databaseName = Guid.NewGuid().ToString();
            await CreateCustomer(databaseName);

            var controller = BuildApplicationUserController(databaseName);
            var customerInfo = new ApplicationUserSignInDTO() { UserName = "test.customer", Password = "Invalid.1" };
            var response = await controller.LoginUser(customerInfo);

            Assert.IsNull(response.Value);

            var result = response.Result as BadRequestObjectResult;
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("Invalid login attempt", result.Value);
        }

        [TestMethod]
        public async Task GetAllCustomers()
        {
            var databaseName = Guid.NewGuid().ToString();
            await CreateCustomer(databaseName);

            var context2 = BuildContext(databaseName);
            var controller = BuildApplicationUserController(databaseName);

            var result = await controller.GetAllCustomers();

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }

        private async Task CreateCustomer(string databaseName)
        {
            var appUserController = BuildApplicationUserController(databaseName);
            var userInfo = new ApplicationUserRegisterDTO()
            {
                UserName = "test.customer",
                Email = "test.customer@test.com",
                Password = "Test.1",
                PhoneNumber = "060123456",
                FirstName = "Test",
                LastName = "Customer",
                DateOfBirth = DateTime.Today,
                Gender = 0,
                IsOwner = false,
                Latitude = 0,
                Longitude = 0
            };

            await appUserController.RegisterUser(userInfo);
        }

        private async Task CreateOwner(string databaseName)
        {
            var appUserController = BuildApplicationUserController(databaseName);
            var userInfo = new ApplicationUserRegisterDTO()
            {
                UserName = "test.owner",
                Email = "test.owner@test.com",
                Password = "Test.1",
                PhoneNumber = "060123456",
                FirstName = "Test",
                LastName = "Owner",
                DateOfBirth = DateTime.Today,
                Gender = 0,
                IsOwner = true
            };

            await appUserController.RegisterUser(userInfo);
        }


        private ApplicationUserController BuildApplicationUserController(string databaseName)
        {
            var context = BuildContext(databaseName);
            var myUserStore = new UserStore<IdentityUser>(context);
            var userManager = BuildUserManager(myUserStore);
            var mapper = BuildMap();

            var httpcontext = new DefaultHttpContext();
            MockAuth(httpcontext);
            var signInManager = SetupSignInManager(userManager, httpcontext);

            var myConfiguration = new Dictionary<string, string>
            {
                {"JWT:key", "StEfAnIjA_JE?car!NaJvEcI.nA,sVeTu#I$nE%diRaJ^lAvA(dOk*SpAvA)" }
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();

            return new ApplicationUserController(context, mapper, configuration, userManager, signInManager);
        }

        // Source: https://github.com/dotnet/aspnetcore/blob/master/src/Identity/test/Shared/MockHelpers.cs
        // Source: https://github.com/dotnet/aspnetcore/blob/master/src/Identity/test/Identity.Test/SignInManagerTest.cs
        // Some code was modified to be adapted to our project.

        
    }
}