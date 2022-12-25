using AutoMapper;
using CarWashApp.DTOs;
using CarWashApp.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarWashApp.Controllers
{
    [ApiController]
    [Route("/api/services")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
    public class ServiceController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;

        public ServiceController(ApplicationDbContext context, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpGet("carwash/{id:int}", Name = "getCarWashServices")]
        public async Task<ActionResult<List<ServiceDTO>>> GetCarWashServices(int id)
        {
            var carWash = await context.CarWashes.FirstOrDefaultAsync(x => x.Id == id);
            if(carWash == null) return NotFound();
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            if (carWash.OwnerId != user.Id) return StatusCode(403);

            var services = context.Services.Where(x => x.CarWashId == id).AsQueryable();
            var serviceDTOS = mapper.Map<List<ServiceDTO>>(services);
            return serviceDTOS;
        }

        [HttpPost("carwash/{Id:int}", Name = "addCarWashService")]
        public async Task<ActionResult> AddCarWashService([FromBody] ServiceCreationDTO serviceCreationDTO)
        {
            var carWash = await context.CarWashes.FirstOrDefaultAsync(x => x.Id == serviceCreationDTO.Id);
            if (carWash == null) return NotFound();
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            if (carWash.OwnerId != user.Id) return StatusCode(403);

            var service = mapper.Map<Service>(serviceCreationDTO);
            service.CarWashId = serviceCreationDTO.Id;
            context.Add(service);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("carwash/{id:int}/{serviceType}", Name = "deleteCarWashService")]
        public async Task<ActionResult> DeleteCarWashService(int id, string serviceType)
        {
            var exists = await context.Services.AnyAsync(x => x.CarWashId == id && x.ServiceType == serviceType);
            if (!exists) return NotFound();

            var carWash = await context.CarWashes.FirstOrDefaultAsync(x => x.Id == id);
            if (carWash == null) return NotFound();
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var ownerId = user.Id;
            if (carWash.OwnerId != ownerId) return StatusCode(403);

            context.Remove(new Service { CarWashId = id,ServiceType = serviceType});
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("carwash/{id:int}/{serviceName}", Name = "modifyService")]
        public async Task<ActionResult> ModifyService(int id, string serviceName, [FromBody] JsonPatchDocument<ServicePatchDTO> doc)
        {
            if (doc == null) return BadRequest();

            var service = await context.Services.FirstOrDefaultAsync(x => x.CarWashId == id && x.ServiceType == serviceName);
            if(service == null) return NotFound();

            var carWash = await context.CarWashes.FirstOrDefaultAsync(x => x.Id == id);
            if (carWash == null) return NotFound();
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            if (carWash.OwnerId != user.Id) return StatusCode(403);

            var servicePatchDTO = mapper.Map<ServicePatchDTO>(service);
            doc.ApplyTo(servicePatchDTO, ModelState);

            if (!TryValidateModel(servicePatchDTO)) return BadRequest(ModelState);

            mapper.Map(servicePatchDTO, service);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
