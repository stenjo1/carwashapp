using CarWashApp.Controllers;
using CarWashApp.DTOs;
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
    public class AppointmentControllerTests : BaseTests
    {
        [TestMethod]
        public async Task GetAppointmentNotFound()
        {
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);

            var customer = new Customer()
            {
                UserName = "test.customer",
                Email = "test.customer@test.com",
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

            context.Add(customer);
            context.SaveChanges();

            var controller = BuildAppointmentController(databaseName);
            controller.ControllerContext = BuildControllerContextWithDefaultCustomer();

            var id = 1;
            var response = await controller.GetAppointment(id);
            var result = response.Result as StatusCodeResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }

        [TestMethod]
        public async Task GetAppointmentForbidden()
        {
            var databaseName = Guid.NewGuid().ToString();
            SeedData(databaseName);

            var context = BuildContext(databaseName);

            var customer2 = new Customer()
            {
                Id = "customer2",
                UserName = "test.customer2",
                Email = "test.customer2@test.com",
                FirstName = "Test",
                LastName = "Customer2",
                PhoneNumber = "060123456",
                Gender = 0,
                DateOfBirth = new DateTime(2000, 12, 31),
                Latitude = 44.0000,
                Longitude = 20.0000,
                Wallet = 0,

            };
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            customer2.Location = geometryFactory.CreatePoint(new Coordinate(customer2.Latitude, customer2.Longitude));

            context.Add(customer2);
            context.SaveChanges();

            var context2 = BuildContext(databaseName);
            var appointment = context2.Appointments.First();
            appointment.CustomerId = "customer2";
            context2.SaveChanges();

            var controller = BuildAppointmentController(databaseName);
            controller.ControllerContext = BuildControllerContextWithDefaultCustomer();

            var id = 1;
            var response = await controller.GetAppointment(id);
            var result = response.Result as StatusCodeResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(403, result.StatusCode);
        }

        [TestMethod]
        public async Task GetAppointment()
        {
            var databaseName = Guid.NewGuid().ToString();
            SeedData(databaseName);

            var controller = BuildAppointmentController(databaseName);
            controller.ControllerContext = BuildControllerContextWithDefaultCustomer();

            var id = 1;
            var response = await controller.GetAppointment(id);
            var result = response.Value;

            var context2 = BuildContext(databaseName);
            var carWash = context2.CarWashes.First();
            var appointment = context2.Appointments.First();

            Assert.IsNotNull(result);
            Assert.AreEqual(carWash.Name, result.CarWashName);
            Assert.AreEqual(appointment.ServiceType, result.ServiceType);
            Assert.AreEqual(appointment.Date, result.Date);
        }

        [TestMethod]
        public async Task CustomerCancelsAppointmentInTime()
        {
            var databaseName = Guid.NewGuid().ToString();
            SeedData(databaseName);

            var context2 = BuildContext(databaseName);
            var appointment = context2.Appointments.First();
            appointment.Date = DateTime.Today;
            appointment.StartHour = DateTime.Now.Hour + 3;
            appointment.EndHour = appointment.StartHour + 1;

            context2.SaveChanges();

            var controller = BuildAppointmentController(databaseName);
            controller.ControllerContext = BuildControllerContextWithDefaultCustomer();

            var response =  await controller.CancelAppointment(1);
            var result = response as StatusCodeResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(204, result.StatusCode);

            var context3 = BuildContext(databaseName);
            var appointmentUpdated = context3.Appointments.First();

            Assert.AreEqual(Status.Declined, appointmentUpdated.Status);
        }

        [TestMethod]
        public async Task CustomerDoesNotCancelAppointmentInTime()
        {
            var databaseName = Guid.NewGuid().ToString();
            SeedData(databaseName);

            var context2 = BuildContext(databaseName);
            var appointment = context2.Appointments.First();
            appointment.Date = DateTime.Today;
            appointment.StartHour = DateTime.Now.Hour;
            appointment.EndHour = appointment.StartHour + 1;

            context2.SaveChanges();

            var controller = BuildAppointmentController(databaseName);
            controller.ControllerContext = BuildControllerContextWithDefaultCustomer();

            var response = await controller.CancelAppointment(1);
            var result = response as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("Cancellation time limited to 15 minutes", result.Value);

            var context3 = BuildContext(databaseName);
            var appointmentUpdated = context3.Appointments.First();

            Assert.AreEqual(appointment.Status, appointmentUpdated.Status);
        }

        [TestMethod]
        public async Task GetAllCustomerAppointments()
        {
            var databaseName = Guid.NewGuid().ToString();
            SeedData(databaseName);

            var controller = BuildAppointmentController(databaseName);
            controller.ControllerContext = BuildControllerContextWithDefaultCustomer();

            var appointmentFilter = new AppointmentFilter()
            {
                Page = 1,
                RecordsPerPage = 1
            };

            var response = await controller.GetCustomerAppointments(appointmentFilter);

            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Count);
        }

        [TestMethod]
        public async Task GetAllCustomerAppointmentsWithFiltering()
        {
            var databaseName = Guid.NewGuid().ToString();
            SeedData(databaseName);

            var controller = BuildAppointmentController(databaseName);
            controller.ControllerContext = BuildControllerContextWithDefaultCustomer();

            var appointmentFilter = new AppointmentFilter()
            {
                IsFinished = true,
                Page = 1,
                RecordsPerPage = 1
            };

            var response = await controller.GetCustomerAppointments(appointmentFilter);

            Assert.IsNotNull(response);
            Assert.AreEqual(0, response.Count);
        }

        [TestMethod]
        public async Task GetAllCarWashAppointments()
        {
            var databaseName = Guid.NewGuid().ToString();
            SeedData(databaseName);

            var controller = BuildAppointmentController(databaseName);
            controller.ControllerContext = BuildControllerContextWithDefaultOwner();

            var pagination = new PaginationDTO()
            {
                Page = 1,
                RecordsPerPage = 1
            };

            var carWashId = 1;
            var response = await controller.GetCarWashAppointments(carWashId, pagination);
            var result = response.Value;

            Assert.IsNotNull(response);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public async Task GetAllCarWashAppointmentsForbidden()
        {
            var databaseName = Guid.NewGuid().ToString();
            SeedData(databaseName);

            var controller = BuildAppointmentController(databaseName);
            controller.ControllerContext = BuildControllerContextWithDefaultCustomer();

            var pagination = new PaginationDTO()
            {
                Page = 1,
                RecordsPerPage = 1
            };

            var carWashId = 1;
            var response = await controller.GetCarWashAppointments(carWashId, pagination);
            var result = response.Result as StatusCodeResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(403, result.StatusCode);
        }

        [TestMethod]
        public async Task CreateAnAppointmentOutsideWorkingHours() 
        { 
            var databaseName = Guid.NewGuid().ToString();
            SeedData(databaseName);

            var controller = BuildAppointmentController(databaseName);
            controller.ControllerContext = BuildControllerContextWithDefaultCustomer();

            var newAppointment = new AppointmentCreationDTO()
            {
                CarWashId = 1,
                StartHour = 7,
                EndHour = 8,
                Date = DateTime.Today,
                ServiceType = "Regular"
            };
            var response = await controller.PostCustomerAppointment(newAppointment);
            var result = response as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("CarWash is closed", result.Value);
        }

        [TestMethod]
        public async Task CreateAnAppointmentWithNonexistentServiceType()
        {
            var databaseName = Guid.NewGuid().ToString();
            SeedData(databaseName);

            var controller = BuildAppointmentController(databaseName);
            controller.ControllerContext = BuildControllerContextWithDefaultCustomer();

            var newAppointment = new AppointmentCreationDTO()
            {
                CarWashId = 1,
                StartHour = 8,
                EndHour = 9,
                Date = DateTime.Today,
                ServiceType = "Nonexistent"
            };
            var response = await controller.PostCustomerAppointment(newAppointment);
            var result = response as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("Specified service is not available in this CarWash", result.Value);
        }

        [TestMethod]
        public async Task CreateAnAppointmentNoAvailableSlots()
        {
            var databaseName = Guid.NewGuid().ToString();
            SeedData(databaseName);

            var controller = BuildAppointmentController(databaseName);
            controller.ControllerContext = BuildControllerContextWithDefaultCustomer();

            var newAppointment = new AppointmentCreationDTO()
            {
                CarWashId = 1,
                StartHour = 9,
                EndHour = 10,
                Date = DateTime.Today,
                ServiceType = "Regular"
            };
            var response = await controller.PostCustomerAppointment(newAppointment);
            var result = response as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("No available slots", result.Value);
        }

        [TestMethod]
        public async Task CreateAnAppointment()
        {
            var databaseName = Guid.NewGuid().ToString();
            SeedData(databaseName);

            var controller = BuildAppointmentController(databaseName);
            controller.ControllerContext = BuildControllerContextWithDefaultCustomer();

            var newAppointment = new AppointmentCreationDTO()
            {
                CarWashId = 1,
                StartHour = 10,
                EndHour = 11,
                Date = DateTime.Today,
                ServiceType = "Regular"
            };
            var response = await controller.PostCustomerAppointment(newAppointment);
            var result = response as CreatedAtRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(201, result.StatusCode);

            var context2 = BuildContext(databaseName);
            var createdAppointment = context2.Appointments.First(x => x.Id == 2);

            Assert.AreEqual(Status.Pending, createdAppointment.Status);
            Assert.AreEqual(false, createdAppointment.IsFinished);
            Assert.AreEqual(1, createdAppointment.CarWashId);
            Assert.AreEqual("customer", createdAppointment.CustomerId);
        }

        [TestMethod]
        public async Task RateAnAppointmentNotFinished()
        {
            var databaseName = Guid.NewGuid().ToString();
            SeedData(databaseName);

            var controller = BuildAppointmentController(databaseName);
            controller.ControllerContext = BuildControllerContextWithDefaultCustomer();

            var id = 1;
            var rating = 4;

            var response = await controller.RateCarWash(id, rating);
            var result = response as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("Appointment has not finished yet", result.Value);
        }

        [TestMethod]
        public async Task RateAnAppointment()
        {
            var databaseName = Guid.NewGuid().ToString();
            SeedData(databaseName);

            var context = BuildContext(databaseName);
            var appointment = context.Appointments.First();
            appointment.IsFinished = true;

            context.SaveChanges();

            var controller = BuildAppointmentController(databaseName);
            controller.ControllerContext = BuildControllerContextWithDefaultCustomer();

            var id = 1;
            var rating = 4;

            var response = await controller.RateCarWash(id, rating);
            var result = response as StatusCodeResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(204, result.StatusCode);

            var context2 = BuildContext(databaseName);
            var updatedAppointment = context2.Appointments.First();
            var carwash = context2.CarWashes.First();

            Assert.AreEqual(rating, (int)updatedAppointment.Rating);
            Assert.AreEqual(1, updatedAppointment.CarWashId);
            Assert.AreEqual("customer", updatedAppointment.CustomerId);
            Assert.AreEqual(rating, (int)carwash.Rating);
            Assert.AreEqual(1, carwash.Votes);
        }

        [TestMethod]
        public async Task ApproveAppointmentNotPending()
        {
            var databaseName = Guid.NewGuid().ToString();
            SeedData(databaseName);

            var context = BuildContext(databaseName);
            var appointment = context.Appointments.First();
            appointment.Status = Status.Declined;

            context.SaveChanges();

            var controller = BuildAppointmentController(databaseName);
            controller.ControllerContext = BuildControllerContextWithDefaultOwner();

            var response = await controller.ApproveAppointment(1, true);
            var result = response as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("Status not pending", result.Value);
        }

        [TestMethod]
        public async Task ApproveAppointment()
        {
            var databaseName = Guid.NewGuid().ToString();
            SeedData(databaseName);

            var controller = BuildAppointmentController(databaseName);
            controller.ControllerContext = BuildControllerContextWithDefaultOwner();

            var response = await controller.ApproveAppointment(1, true);
            var result = response as StatusCodeResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(204, result.StatusCode);

            var context = BuildContext(databaseName);
            var appointment = context.Appointments.First();

            Assert.AreEqual(Status.Approved, appointment.Status);
            Assert.AreEqual(1, appointment.CarWashId);
            Assert.AreEqual("customer", appointment.CustomerId);
        }

        private AppointmentController BuildAppointmentController(string databaseName)
        {
            var context = BuildContext(databaseName);
            var myUserStore = new UserStore<IdentityUser>(context);
            var userManager = BuildUserManager(myUserStore);
            var mapper = BuildMap();

            var httpcontext = new DefaultHttpContext();
            MockAuth(httpcontext);

            return new AppointmentController(context, mapper, userManager);
        }
    }
}
