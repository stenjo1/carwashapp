using AutoMapper;
using CarWashApp.DTOs;
using CarWashApp.Entities;
using CarWashApp.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using System.Linq.Dynamic.Core;

namespace CarWashApp.Controllers
{
    [ApiController]
    [Route("/api/carwashes")]
    public class CarWashController : ControllerBase
    {
        private readonly ApplicationDbContext context;  
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;
        private readonly GeometryFactory geometryFactory;

        public CarWashController(ApplicationDbContext context, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
            geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
        }

        /// <summary>
        /// Show carwashes near logged in customer with the best rating
        /// </summary>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "CustomerAccess")]
        [HttpGet("all", Name = "getCarWashes")]
        public async Task<ActionResult<List<CarWashDTO>>> GetCarWashes([FromQuery] CarWashFilterDTO carWashFilterDTO)
        {
            var carwashesQueryable = context.CarWashes.Include(x => x.Services).AsQueryable();

            var currCustomer = await context.Customers.Where(x => x.UserName == User.Identity.Name).AsNoTracking().FirstAsync();

            if (carWashFilterDTO.Rating != 0u)
            {
                carwashesQueryable = carwashesQueryable.Where(x => (double)carWashFilterDTO.Rating - 0.5 < (double)x.Rating / x.Votes
                && (double)x.Rating / x.Votes <= (double)carWashFilterDTO.Rating + 0.5);
            }

            if (carWashFilterDTO.MinSize != 0 || carWashFilterDTO.MaxSize != 0)
            {
                carwashesQueryable = carwashesQueryable.Where(x => (carWashFilterDTO.MinSize != 0 && carWashFilterDTO.MaxSize == 0 && x.Size > carWashFilterDTO.MinSize)
                || (carWashFilterDTO.MaxSize != 0 && carWashFilterDTO.MinSize == 0 && x.Size < carWashFilterDTO.MaxSize)
                || (carWashFilterDTO.MaxSize != 0 && carWashFilterDTO.MinSize != 0 && carWashFilterDTO.MinSize <= x.Size && x.Size <= carWashFilterDTO.MaxSize));
            }

            if (carWashFilterDTO.IsOpen)
            {
                var currHour = DateTime.Now.Hour;
                carwashesQueryable = carwashesQueryable.Where(x => x.OpeningHour <= currHour && currHour < x.ClosingHour);
            }

            if (carWashFilterDTO.DistanceInKms != 50) {
                carwashesQueryable = carwashesQueryable.OrderByDescending(x => x.Location.Distance(currCustomer.Location))
                                                   .Where(x => x.Location.IsWithinDistance(currCustomer.Location, carWashFilterDTO.DistanceInKms * 1000));
            }

            if (!string.IsNullOrWhiteSpace(carWashFilterDTO.OrderingField))
            {
                try
                {
                    if (carWashFilterDTO.OrderingField == "Rating")
                    {
                        carwashesQueryable = carwashesQueryable.OrderByDescending(x => x.Votes != 0 ? (double)x.Rating / x.Votes : x.Rating);
                    } else
                    {
                        carwashesQueryable = carwashesQueryable
                        .OrderBy($"{carWashFilterDTO.OrderingField} {(carWashFilterDTO.Ascending ? "ascending" : "descending")}");
                    }
                }
                catch
                {
                    // log this
                }
            }
            else
            {
                carwashesQueryable = carwashesQueryable.OrderByDescending(x => x.Votes != 0 ? (double)x.Rating / x.Votes : x.Rating);
            }

            var paginationDTO = new PaginationDTO() { Page = carWashFilterDTO.Page, RecordsPerPage = carWashFilterDTO.RecordsPerPage };
            await HttpContext.InsertPaginationParametersInResponse(carwashesQueryable, paginationDTO.RecordsPerPage);

            var carwashes = await carwashesQueryable.Paginate(paginationDTO)
                                                    .ToListAsync();
            return mapper.Map<List<CarWashDTO>>(carwashes);
        }

        /// <summary>
        /// Show carwashes owned by logged in owner
        /// </summary>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
        [HttpGet("owner", Name = "getOwnerCarWashes")]
        public async Task<ActionResult<List<CarWashDetailsDTO>>> GetOwnerCarWashes([FromQuery] CarWashFilterDTO carWashFilterDTO)
        {
            var carwashesQueryable = context.CarWashes.AsQueryable();
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var id = user.Id;

            if (carwashesQueryable.Any())
            {
                carwashesQueryable = carwashesQueryable.Where(x => x.OwnerId == id);
            }
            else
            {
                return NotFound();
            }

            if (carWashFilterDTO.Rating != 0u)
            {
                carwashesQueryable = carwashesQueryable.Where(x => (double)carWashFilterDTO.Rating - 0.5 < (double)x.Rating / x.Votes 
                && (double)x.Rating / x.Votes <= (double)carWashFilterDTO.Rating + 0.5);
            }

            if (carWashFilterDTO.MinSize != 0 || carWashFilterDTO.MaxSize != 0)
            {
                carwashesQueryable = carwashesQueryable.Where(x => 
                (carWashFilterDTO.MinSize != 0 && carWashFilterDTO.MaxSize == 0 && x.Size > carWashFilterDTO.MinSize)
                || (carWashFilterDTO.MaxSize != 0 && carWashFilterDTO.MinSize == 0 && x.Size < carWashFilterDTO.MaxSize)
                || (carWashFilterDTO.MaxSize != 0 && carWashFilterDTO.MinSize != 0 && carWashFilterDTO.MinSize <= x.Size && x.Size <= carWashFilterDTO.MaxSize));
            }

            if (carWashFilterDTO.IsOpen)
            {
                var currHour = DateTime.Now.Hour;
                carwashesQueryable = carwashesQueryable.Where(x => x.OpeningHour <= currHour && currHour < x.ClosingHour);
            }

            if (!string.IsNullOrWhiteSpace(carWashFilterDTO.OrderingField))
            {
                try
                {
                    carwashesQueryable = carwashesQueryable
                        .OrderBy($"{carWashFilterDTO.OrderingField} {(carWashFilterDTO.Ascending ? "ascending" : "descending")}");
                }
                catch
                {
                    // log this
                }
            } else
            {
                carwashesQueryable = carwashesQueryable.OrderByDescending(x => x.Votes != 0 ? (double)x.Rating / x.Votes : x.Rating);
            }

            var paginationDTO = new PaginationDTO() { Page = carWashFilterDTO.Page, RecordsPerPage = carWashFilterDTO.RecordsPerPage };
            await HttpContext.InsertPaginationParametersInResponse(carwashesQueryable, paginationDTO.RecordsPerPage);

            var carwashes = await carwashesQueryable.Include(x => x.Services)
                                                    .Include(x => x.Appointments)
                                                    .ThenInclude(x => x.Customer)
                                                    .Paginate(paginationDTO)
                                                    .ToListAsync();
            return mapper.Map<List<CarWashDetailsDTO>>(carwashes);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
        [HttpGet("{id:int}", Name = "getCarWash")]
        public async Task<ActionResult<CarWashDetailsDTO>> GetCarWash(int id)
        {
            var carWash = await context.CarWashes.Include(x => x.Services)
                                                 .Include(x => x.Appointments)
                                                 .ThenInclude(x => x.Customer)
                                                 .FirstOrDefaultAsync(x => x.Id == id);
            if (carWash == null) return NotFound();

            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var currUserId = user.Id;
            if (carWash.OwnerId != currUserId) return StatusCode(403);

            var CarWashDetailsDTO = mapper.Map<CarWashDetailsDTO>(carWash);
            return CarWashDetailsDTO;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
        [HttpGet("{id:int}/revenue", Name = "getCarWashRevenue")]
        public async Task<ActionResult<RevenueDTO>> GetCarWashRevenue(int id)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var currUserId = user.Id;
            var revenue = await context.Revenues.Include(x => x.CarWash)
                                                .FirstOrDefaultAsync(x => x.CarWashId == id && x.CarWash.OwnerId == currUserId );
            if (revenue == null) return NotFound();

            var revenueDTO = mapper.Map<RevenueDTO>(revenue);
            return revenueDTO;
        }

        //ne znam sta je zahtev nek ima oba
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
        [HttpGet("{Id:int}/revenue/filter", Name = "getCarWashRevenueFilter")]
        public async Task<ActionResult<RevenueFilterDTO>> GetCarWashRevenueFilter([FromQuery] RevenueFilterCreationDTO revenueFilter)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var appointmentsQueryable = context.Appointments.Where(x => x.CarWashId == revenueFilter.Id).AsQueryable();

            if (revenueFilter.Year != 0)
            {
                appointmentsQueryable = appointmentsQueryable.Where(x => x.Date.Year == revenueFilter.Year);
            }

            if (revenueFilter.Month != 0)
            {
                appointmentsQueryable = appointmentsQueryable.Where(x => x.Date.Month == revenueFilter.Month);
            }

            if (revenueFilter.Day != 0)
            {
                appointmentsQueryable = appointmentsQueryable.Where(x => x.Date.Day == revenueFilter.Day);
            }

            var appointments = await appointmentsQueryable.Include(x => x.CarWash)
                                                          .ThenInclude(x => x.Services)
                                                          .ToListAsync();

            if (appointments.Any() && appointments.First().CarWash.OwnerId != user.Id)
                return StatusCode(403);

            var totalSum = 0d;
            foreach (var app in appointments)
            {
                totalSum += app.CarWash.Services.Where(x => x.CarWashId == revenueFilter.Id && x.ServiceType == app.ServiceType)
                                                .First().Price;
            }

            return new RevenueFilterDTO { TotalRevenue = totalSum};
            
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
        [HttpPost("create", Name = "createCarWash")]
        public async Task<ActionResult> CreateCarWash([FromBody] CarWashCreationDTO carWashCreationDTO)
        {
            var carWash = mapper.Map<CarWash>(carWashCreationDTO);
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var ownerId = user.Id;
            carWash.OwnerId = ownerId;
            carWash.Revenue = new Revenue() { CarWashId = carWash.Id};
            carWash.Services = new List<Service>();
            carWash.Appointments = new List<Appointment>();

            carWash.Location = geometryFactory.CreatePoint(new Coordinate(carWash.Latitude, carWash.Longitude));

            context.Add(carWash);
            await context.SaveChangesAsync();
            
            var CarWashDetailsDTO = mapper.Map<CarWashDetailsDTO>(carWash);
            return new CreatedAtRouteResult("getCarWash", new { id = carWash.Id}, CarWashDetailsDTO);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
        [HttpPut("{Id:int}", Name = "modifyCarWash")]
        public async Task<ActionResult> ModifyCarWash([FromBody] CarWashPutDTO carWashPutDTO)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var currUserId = user.Id;

            var carWash = await context.CarWashes.FirstOrDefaultAsync(x => x.Id == carWashPutDTO.Id && x.OwnerId == currUserId);
            if (carWash == null) return NotFound();

            mapper.Map(carWashPutDTO, carWash);
            if(carWash.Longitude != 0 && carWash.Latitude != 0)
            {
                carWash.Location = geometryFactory.CreatePoint(new Coordinate(carWash.Latitude, carWash.Longitude));
            }
            await context.SaveChangesAsync();
            return NoContent();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
        [HttpDelete("{id:int}", Name = "deleteCarWash")]
        public async Task<ActionResult> DeleteCarWash(int id)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var currUserId = user.Id;
            var exists = await context.CarWashes.AnyAsync(x => x.Id == id && x.OwnerId == currUserId);
            if (!exists) return NotFound();

            context.Remove(new CarWash { Id = id });
            await context.SaveChangesAsync();

            return NoContent();
        }

    }
}
