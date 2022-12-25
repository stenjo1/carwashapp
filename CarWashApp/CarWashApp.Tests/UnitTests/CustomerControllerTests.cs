using CarWashApp.Controllers;
using CarWashApp.DTOs;
using CarWashApp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Configuration;
using Moq;
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
    public class CustomerControllerTests : BaseTests
    {
        [TestMethod]
        public async Task GetLoggedInCustomer()
        {
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);

            var customer = new Customer()
            {
                Id = "customer",
                UserName = "test.customer",
                NormalizedUserName = "TEST.CUSTOMER",
                Email = "test.customer@test.com",
                NormalizedEmail = "TEST.CUSTOMER@TEST.COM",
                PasswordHash = "QUDCBSPFTEUSBW183@*&$nALSAHJ1",
                FirstName = "Test",
                LastName = "Customer",
                PhoneNumber = "060123456",
                Gender = 0,
                DateOfBirth = new DateTime(2000, 12, 31),
                Latitude = 44.0000,
                Longitude = 20.0000,
                Wallet = 0,

            };
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            customer.Location = geometryFactory.CreatePoint(new Coordinate(customer.Latitude, customer.Longitude));

            context.Customers.Add(customer);
            context.SaveChanges();

            var controller = BuildCustomerController(databaseName);
            controller.ControllerContext = BuildControllerContextWithDefaultCustomer();

            var response = await controller.GetCustomer();
            Assert.IsNotNull(response);

            var result = response.Value;
            Assert.AreEqual(customer.Email, result.Email);
            Assert.AreEqual(customer.FirstName, result.FirstName);
            Assert.AreEqual(customer.LastName, result.LastName);
        }

        [TestMethod]
        public async Task PatchSingleFieldForLoggedInCustomer()
        {
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);

            var customer = new Customer()
            {
                Id = "customer",
                UserName = "test.customer",
                NormalizedUserName = "TEST.CUSTOMER",
                Email = "test.customer@test.com",
                NormalizedEmail = "TEST.CUSTOMER@TEST.COM",
                PasswordHash = "QUDCBSPFTEUSBW183@*&$nALSAHJ1",
                FirstName = "Test",
                LastName = "Customer",
                PhoneNumber = "060123456",
                Gender = 0,
                DateOfBirth = new DateTime(2000, 12, 31),
                Latitude = 44.0000,
                Longitude = 20.0000,
                Wallet = 0,

            };
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            customer.Location = geometryFactory.CreatePoint(new Coordinate(customer.Latitude, customer.Longitude));

            context.Customers.Add(customer);
            context.SaveChanges();

            var controller = BuildCustomerController(databaseName);
            controller.ControllerContext = BuildControllerContextWithDefaultCustomer();

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(), It.IsAny<ValidationStateDictionary>(), It.IsAny<string>(), It.IsAny<object>()));
            controller.ObjectValidator = objectValidator.Object;

            var patchDoc = new JsonPatchDocument<CustomerPatchDTO>();
            patchDoc.Operations.Add(new Operation<CustomerPatchDTO>("replace", "/wallet", null, 1000));

            var response = await controller.PatchCustomer(patchDoc);
            var result = response as StatusCodeResult;

            Assert.AreEqual(204, result.StatusCode);

            var context2 = BuildContext(databaseName);
            var updatedCustomer = context2.Customers.First();

            Assert.AreEqual(1000, updatedCustomer.Wallet);
            Assert.AreEqual(customer.FirstName, updatedCustomer.FirstName);
            Assert.AreEqual(customer.UserName, updatedCustomer.UserName);
        }

        [TestMethod]
        public async Task PutLoggedInCustomer()
        {
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);

            var customer = new Customer()
            {
                Id = "customer",
                UserName = "test.customer",
                NormalizedUserName = "TEST.CUSTOMER",
                Email = "test.customer@test.com",
                NormalizedEmail = "TEST.CUSTOMER@TEST.COM",
                PasswordHash = "QUDCBSPFTEUSBW183@*&$nALSAHJ1",
                FirstName = "Test",
                LastName = "Customer",
                PhoneNumber = "060123456",
                Gender = 0,
                DateOfBirth = new DateTime(2000, 12, 31),
                Latitude = 44.0000,
                Longitude = 20.0000,
                Wallet = 0,

            };
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            customer.Location = geometryFactory.CreatePoint(new Coordinate(customer.Latitude, customer.Longitude));

            context.Customers.Add(customer);
            context.SaveChanges();

            var controller = BuildCustomerController(databaseName);
            controller.ControllerContext = BuildControllerContextWithDefaultCustomer();

            var customerPatch = new CustomerPatchDTO()
            {
                FirstName = "Test2",
                LastName = "Customer2"
            };

            var response = await controller.PutCustomer(customerPatch);
            var result = response as StatusCodeResult;

            Assert.AreEqual(204, result.StatusCode);

            var context2 = BuildContext(databaseName);
            var updatedCustomer = context2.Customers.First();

            Assert.AreEqual(0, updatedCustomer.Wallet);
            Assert.AreEqual(customerPatch.FirstName, updatedCustomer.FirstName);
            Assert.AreEqual(customerPatch.LastName, updatedCustomer.LastName);
            Assert.AreEqual(customer.UserName, updatedCustomer.UserName);
        }

        [TestMethod]
        public async Task DeleteLoggedInCustomer()
        {
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);

            var customer = new Customer()
            {
                Id = "customer",
                UserName = "test.customer",
                NormalizedUserName = "TEST.CUSTOMER",
                Email = "test.customer@test.com",
                NormalizedEmail = "TEST.CUSTOMER@TEST.COM",
                PasswordHash = "QUDCBSPFTEUSBW183@*&$nALSAHJ1",
                FirstName = "Test",
                LastName = "Customer",
                PhoneNumber = "060123456",
                Gender = 0,
                DateOfBirth = new DateTime(2000, 12, 31),
                Latitude = 44.0000,
                Longitude = 20.0000,
                Wallet = 0,

            };
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            customer.Location = geometryFactory.CreatePoint(new Coordinate(customer.Latitude, customer.Longitude));

            context.Customers.Add(customer);
            context.SaveChanges();

            var controller = BuildCustomerController(databaseName);
            controller.ControllerContext = BuildControllerContextWithDefaultCustomer();

            var response = await controller.DeleteCustomer();
            var result = response as StatusCodeResult;

            Assert.AreEqual(204, result.StatusCode);

            var context2 = BuildContext(databaseName);
            var customers = context2.Customers.ToList();
            Assert.AreEqual(0, customers.Count);
        }

        private CustomerController BuildCustomerController(string databaseName)
        {
            var context = BuildContext(databaseName);
            var myUserStore = new UserStore<IdentityUser>(context);
            var userManager = BuildUserManager(myUserStore);
            var mapper = BuildMap();

            var httpcontext = new DefaultHttpContext();
            MockAuth(httpcontext);

            return new CustomerController(context, mapper, userManager);
        }


    }
}
