using AutoMapper;
using CarWashApp.DTOs;
using CarWashApp.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using System.Linq.Dynamic.Core;

namespace CarWashApp.Controllers
{
    [ApiController]
    [Route("/api/customer")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Customer")]
    public class CustomerController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;
        private readonly GeometryFactory geometryFactory;

        public CustomerController(ApplicationDbContext context, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
            geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
        }

        [HttpGet(Name = "getCustomer")]
        public async Task<ActionResult<CustomerDTO>> GetCustomer()
        {
            var customer = await context.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.UserName == User.Identity.Name);
            if (customer == null) return NotFound();

            var customerDTO = mapper.Map<CustomerDTO>(customer);
            return customerDTO;
        }

        [HttpPatch(Name = "patchCustomer")]
        public async Task<ActionResult> PatchCustomer([FromBody] JsonPatchDocument<CustomerPatchDTO> doc)
        {
            if (doc == null) return BadRequest();

            var customer = await context.Customers.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
            if (customer == null) return NotFound();

            var customerPatchDTO = mapper.Map<CustomerPatchDTO>(customer);
            doc.ApplyTo(customerPatchDTO, ModelState);

            if (!TryValidateModel(customerPatchDTO)) return BadRequest(ModelState);

            mapper.Map(customerPatchDTO, customer);
            if (customer.Longitude != 0 && customer.Latitude != 0)
            {
                customer.Location = geometryFactory.CreatePoint(new Coordinate(customer.Latitude, customer.Longitude));
            }
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut(Name = "putCustomer")]
        public async Task<ActionResult> PutCustomer([FromForm] CustomerPatchDTO customerPatchDTO)
        {
            var customer = await context.Customers.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
            if(customer == null) return BadRequest();

            mapper.Map(customerPatchDTO, customer);
            if (customer.Longitude != 0 && customer.Latitude != 0)
            {
                customer.Location = geometryFactory.CreatePoint(new Coordinate(customer.Latitude, customer.Longitude));
            }
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete(Name = "deleteCustomer")]
        public async Task<ActionResult> DeleteCustomer()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var id = user.Id;

            var currUser = await userManager.Users.FirstOrDefaultAsync(x => x.Id == id);

            var userAppointments = await context.Appointments.Where(x => x.CustomerId == id).ToListAsync();
            if (userAppointments.Any())
            {
                context.RemoveRange(userAppointments);
            }

            await context.SaveChangesAsync();

            var result = await userManager.DeleteAsync(currUser);
            if (!result.Succeeded) return BadRequest();

            return NoContent();
        }
    }
}
