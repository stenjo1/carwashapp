using AutoMapper;
using CarWashApp.DTOs;
using CarWashApp.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Security.Claims;

namespace CarWashApp.Controllers
{
    [ApiController]
    [Route("/api/owner")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
    public class OwnerController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;

        public OwnerController(ApplicationDbContext context, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpGet(Name = "getOwner")]
        public async Task<OwnerDTO> GetOwner()
        {
            var owner = await context.Owners.AsNoTracking().FirstOrDefaultAsync(c => c.UserName == User.Identity.Name);
            var ownerDTO = mapper.Map<OwnerDTO>(owner);
            return ownerDTO;
        }

        [HttpPatch(Name = "patchOwner")]
        public async Task<ActionResult> PatchOwner([FromBody] JsonPatchDocument<OwnerPatchDTO> doc)
        {
            if (doc == null) return BadRequest();

            var owner = await context.Owners.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
            if (owner == null) return NotFound();

            var ownerPatchDTO = mapper.Map<OwnerPatchDTO>(owner);
            doc.ApplyTo(ownerPatchDTO, ModelState);

            if (!TryValidateModel(ownerPatchDTO)) return BadRequest(ModelState);

            mapper.Map(ownerPatchDTO, owner);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut(Name = "putOwner")]
        public async Task<ActionResult> PutOwner([FromForm] OwnerPatchDTO ownerPatchDTO)
        {
            var owner = await context.Owners.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
            mapper.Map(ownerPatchDTO, owner);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete(Name = "deleteOwner")]
        public async Task<ActionResult> DeleteOwner()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var ownerId = user.Id;

            var currUser = await userManager.Users.FirstOrDefaultAsync(x => x.Id == ownerId);
            var result = await userManager.DeleteAsync(currUser);
            if (!result.Succeeded) return BadRequest();

            return NoContent();
        }

        [HttpGet("totalIncome", Name = "getTotalIncome")]
        public async Task<ActionResult<double>> GetTotalIncome()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var ownerId = user.Id;
            var owner = await context.Owners.AsNoTracking().Where(x => x.Id == ownerId).FirstAsync();

            owner.TotalIncome = await context.CarWashes.Include(x => x.Revenue)
                                                       .Where(x => x.OwnerId == ownerId)
                                                       .SumAsync(x => x.Revenue.MonthlyIncome);
            owner.TotalIncome += await context.CarWashes.Include(x => x.Revenue)
                                                       .Where(x => x.OwnerId == ownerId)
                                                       .SumAsync(x => x.Revenue.WeeklyIncome);
            owner.TotalIncome += await context.CarWashes.Include(x => x.Revenue)
                                                       .Where(x => x.OwnerId == ownerId)
                                                       .SumAsync(x => x.Revenue.DailyIncome);
            owner.TotalIncome += await context.CarWashes.Include(x => x.Revenue)
                                                       .Where(x => x.OwnerId == ownerId)
                                                       .SumAsync(x => x.Revenue.CurrentValue);

            await context.SaveChangesAsync();
            return owner.TotalIncome;
        }
    }
}
