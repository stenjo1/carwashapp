using CarWashApp.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;
using System.Security.Claims;

namespace CarWashApp
{
    public class ApplicationDbContext : IdentityDbContext
    {
        private readonly IWebHostEnvironment env;

        public ApplicationDbContext(DbContextOptions options, IWebHostEnvironment env) : base(options)
        {
            this.env = env;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

            modelBuilder.Entity<Revenue>().HasKey( x => x.CarWashId);
            modelBuilder.Entity<Service>().HasKey( x => new { x.CarWashId, x.ServiceType});
            modelBuilder.Entity<Owner>().HasMany( x => x.CarWashes).WithOne( x => x.Owner);
            modelBuilder.Entity<Customer>().HasMany(x => x.Appointments).WithOne(x => x.Customer);
            modelBuilder.Entity<CarWash>().HasMany(x => x.Appointments).WithOne(x => x.CarWash);

            var claims = new List<IdentityUserClaim<string>>();
            var passwordHasher = new PasswordHasher<IdentityUser>();

            //seed data
            var owners = SeedData<Owner>("owners");
            for(int i = 0; i < owners.Count; i++)
            {
                var owner = owners[i];
                owner.PasswordHash = passwordHasher.HashPassword(null, owner.PasswordHash);
                owner.NormalizedUserName = owner.UserName.ToUpper();
                owner.NormalizedEmail = owner.Email.ToUpper();
                claims.Add(new IdentityUserClaim<string>()
                {
                    Id = i + 1,
                    UserId = owner.Id,
                    ClaimType = ClaimTypes.Role,
                    ClaimValue = "Owner"
                });
            }
            modelBuilder.Entity<Owner>().HasData(owners);

            var carWashes = SeedData<CarWash>("carWashes");
            foreach(var cw in carWashes)
            {
                cw.Location = geometryFactory.CreatePoint(new Coordinate(cw.Latitude, cw.Longitude));
            }
            modelBuilder.Entity<CarWash>().HasData(carWashes);

            var revenues = new List<Revenue>();
            foreach (var carWash in carWashes)
            {
                var revenue = new Revenue() { CarWashId = carWash.Id };
                revenues.Add(revenue);
            }
            modelBuilder.Entity<Revenue>().HasData(revenues);

            var services = SeedData<Service>("services");
            modelBuilder.Entity<Service>().HasData(services);

            var customers = SeedData<Customer>("customers");
            for (int i = 0; i < customers.Count; i++)
            {
                var customer = customers[i];
                customer.PasswordHash = passwordHasher.HashPassword(null, customer.PasswordHash);
                customer.NormalizedUserName = customer.UserName.ToUpper();
                customer.NormalizedEmail = customer.Email.ToUpper();
                customer.Location = geometryFactory.CreatePoint(new Coordinate(customer.Latitude, customer.Longitude));
                claims.Add(new IdentityUserClaim<string>()
                {
                    Id = i + 1 + owners.Count,
                    UserId = customer.Id,
                    ClaimType = ClaimTypes.Role,
                    ClaimValue = "Customer"
                });
            }
            modelBuilder.Entity<Customer>().HasData(customers);

            var appointments = SeedData<Appointment>("appointments");
            modelBuilder.Entity<Appointment>().HasData(appointments);

            //add admin
            modelBuilder.Entity<IdentityUser>().HasData(new IdentityUser
            {
                Id = "c4d17476-bd1d-412b-b989-009ab6d3d167",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                PasswordHash = passwordHasher.HashPassword(null, "admin")
            });
            claims.Add(new IdentityUserClaim<string>()
            {
                Id = owners.Count + customers.Count + 1,
                UserId = "c4d17476-bd1d-412b-b989-009ab6d3d167",
                ClaimType = ClaimTypes.Role,
                ClaimValue = "Admin"
            });


            modelBuilder.Entity<IdentityUserClaim<string>>().HasData(claims);

            base.OnModelCreating(modelBuilder);
        }

        private List<T> SeedData<T>(string entityName)
        {  
            var fullPath = Path.Combine(env.WebRootPath + Path.DirectorySeparatorChar
                                                              + "resources" + Path.DirectorySeparatorChar
                                                              + "json" + Path.DirectorySeparatorChar
                                                              + entityName + ".json");
            string entityJson = File.ReadAllText(fullPath);
            List<T>? entitiesList = JsonConvert.DeserializeObject<List<T>>(entityJson);

            if (entitiesList == null)
                return new List<T>();

            return entitiesList;
        }

        public DbSet<Owner> Owners { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CarWash> CarWashes { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Revenue> Revenues { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
    }
}
