using AutoMapper;
using CarWashApp.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using CarWashApp.Entities;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace CarWashApp.Tests
{
    public class BaseTests
    {
        protected ApplicationDbContext BuildContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName).Options;
            var mockEnvironment = new Mock<IWebHostEnvironment>();
            mockEnvironment.Setup(m => m.EnvironmentName)
                           .Returns("Hosting:UnitTestEnvironment");
            mockEnvironment.Setup(m => m.WebRootPath)
                           .Returns("wwwroot");

            var dbContext = new ApplicationDbContext(options, mockEnvironment.Object);
            return dbContext;
        }

        protected IMapper BuildMap()
        {
            var config = new MapperConfiguration(options =>
            {
                options.AddProfile(new AutoMapperProfile());
            });

            return config.CreateMapper();
        }

        protected ControllerContext BuildControllerContextWithDefaultCustomer()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "test.customer"),
                new Claim(ClaimTypes.Email, "test.customer@test.com"),
                new Claim(ClaimTypes.Role, "customer")
            }, "test"));

            return new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        protected ControllerContext BuildControllerContextWithDefaultOwner()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "test.owner"),
                new Claim(ClaimTypes.Email, "test.owner@test.com"),
                new Claim(ClaimTypes.Role, "owner")
            }, "test"));

            return new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        public void SeedData(string databaseName)
        {
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
            carWash.Location = geometryFactory.CreatePoint(new Coordinate(carWash.Latitude, carWash.Longitude));

            var appointment = new Appointment()
            {
                Id = 1,
                CustomerId = "customer",
                CarWashId = 1,
                StartHour = 9,
                EndHour = 10,
                Date = DateTime.Today,
                DateCreated = DateTime.Today,
                ServiceType = "Regular"
            };

            var service = new Service()
            {
                CarWashId = 1,
                ServiceType = "Regular",
                Price = 100,
                Duration = 1
            };
            var revenue = new Revenue()
            {
                CarWashId = 1
            };

            context.Add(customer);
            context.Add(owner);
            context.Add(carWash);
            context.Add(appointment);
            context.Add(service);
            context.Add(revenue);
            context.SaveChanges();
        }

        protected UserManager<TUser> BuildUserManager<TUser>(IUserStore<TUser> store = null) where TUser : class
        {
            store = store ?? new Mock<IUserStore<TUser>>().Object;
            var options = new Mock<IOptions<IdentityOptions>>();
            var idOptions = new IdentityOptions();
            idOptions.Lockout.AllowedForNewUsers = false;

            options.Setup(o => o.Value).Returns(idOptions);

            var userValidators = new List<IUserValidator<TUser>>();

            var validator = new Mock<IUserValidator<TUser>>();
            userValidators.Add(validator.Object);
            var pwdValidators = new List<PasswordValidator<TUser>>();
            pwdValidators.Add(new PasswordValidator<TUser>());

            var userManager = new UserManager<TUser>(store, options.Object, new PasswordHasher<TUser>(),
                userValidators, pwdValidators, new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(), null,
                new Mock<ILogger<UserManager<TUser>>>().Object);

            validator.Setup(v => v.ValidateAsync(userManager, It.IsAny<TUser>()))
                .Returns(Task.FromResult(IdentityResult.Success)).Verifiable();

            return userManager;
        }

        protected static SignInManager<TUser> SetupSignInManager<TUser>(UserManager<TUser> manager,
            HttpContext context, ILogger logger = null, IdentityOptions identityOptions = null,
            IAuthenticationSchemeProvider schemeProvider = null) where TUser : class
        {
            var contextAccessor = new Mock<IHttpContextAccessor>();
            contextAccessor.Setup(a => a.HttpContext).Returns(context);
            identityOptions = identityOptions ?? new IdentityOptions();
            var options = new Mock<IOptions<IdentityOptions>>();
            options.Setup(a => a.Value).Returns(identityOptions);
            var claimsFactory = new UserClaimsPrincipalFactory<TUser>(manager, options.Object);
            schemeProvider = schemeProvider ?? new Mock<IAuthenticationSchemeProvider>().Object;
            var sm = new SignInManager<TUser>(manager, contextAccessor.Object, claimsFactory, options.Object, null, schemeProvider, new DefaultUserConfirmation<TUser>());
            sm.Logger = logger ?? (new Mock<ILogger<SignInManager<TUser>>>()).Object;
            return sm;
        }

        protected Mock<IAuthenticationService> MockAuth(HttpContext context)
        {
            var auth = new Mock<IAuthenticationService>();
            context.RequestServices = new ServiceCollection().AddSingleton(auth.Object).BuildServiceProvider();
            return auth;
        }

    }
}
