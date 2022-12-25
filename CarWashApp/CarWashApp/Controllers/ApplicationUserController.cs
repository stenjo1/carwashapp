using AutoMapper;
using CarWashApp.DTOs;
using CarWashApp.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CarWashApp.Controllers
{
    [ApiController]
    [Route("/api/accounts")]
    public class ApplicationUserController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public ApplicationUserController(ApplicationDbContext context, IMapper mapper, 
            IConfiguration configuration, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.configuration = configuration;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpPost("register", Name = "registerUser")]
        public async Task<ActionResult<UserToken>> RegisterUser([FromBody] ApplicationUserRegisterDTO userDTO)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Role, userDTO.IsOwner ? "Owner" : "Customer")
            };

            if (userDTO.IsOwner)
            {
                var user = new Owner
                {
                    UserName = userDTO.UserName,
                    Email = userDTO.Email,
                    PhoneNumber = userDTO.PhoneNumber,
                    FirstName = userDTO.FirstName,
                    LastName = userDTO.LastName,
                    DateOfBirth = userDTO.DateOfBirth,
                    Gender = userDTO.Gender,
                };
                
                var result = await userManager.CreateAsync(user, userDTO.Password);
                if (!result.Succeeded) return BadRequest(result.Errors);
                await userManager.AddClaimsAsync(user, claims);
            }
            else
            {
                var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
                var user = new Customer
                {
                    UserName = userDTO.UserName,
                    Email = userDTO.Email,
                    PhoneNumber= userDTO.PhoneNumber,
                    FirstName = userDTO.FirstName,
                    LastName = userDTO.LastName,
                    DateOfBirth = userDTO.DateOfBirth,
                    Gender = userDTO.Gender,
                    Latitude = userDTO.Latitude,
                    Longitude = userDTO.Longitude,
                    Location = geometryFactory.CreatePoint(new Coordinate(userDTO.Latitude, userDTO.Longitude))
                };
                
                var result = await userManager.CreateAsync(user, userDTO.Password);
                if (!result.Succeeded) return BadRequest(result.Errors);
                await userManager.AddClaimsAsync(user, claims);
            }

            return await BuildToken(new ApplicationUserSignInDTO { UserName = userDTO.UserName, Password = userDTO.Password});
        }

        [HttpPost("login", Name = "loginUser")]
        public async Task<ActionResult<UserLoginToken>> LoginUser([FromBody] ApplicationUserSignInDTO userSignInDTO)
        {
            var result = await signInManager.PasswordSignInAsync(userSignInDTO.UserName, userSignInDTO.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var tokenInfo = await BuildToken(userSignInDTO);
                var user = await context.Users.FirstOrDefaultAsync(user => user.UserName == userSignInDTO.UserName);
                var claim = await context.UserClaims.FirstOrDefaultAsync(userClaim => userClaim.UserId == user.Id);
                var role = claim.ClaimValue;
                return new UserLoginToken()
                {
                    Token = tokenInfo.Token,
                    ExpirationDate = tokenInfo.ExpirationDate,
                    Role = role
                };
            }
            else return BadRequest("Invalid login attempt");
        }

        [HttpPost("logout", Name  = "logoutUser")]
        public async Task<ActionResult> LogoutUser()
        {
            if (signInManager.IsSignedIn(HttpContext.User)) await signInManager.SignOutAsync();
            else return BadRequest("Invalid logout attempt");

            return Ok("Logout successful");
        }

        [HttpPost("renewToken", Name = "renewToken")]
        public async Task<ActionResult<UserToken>> RenewToken()
        {
            string? userName = (HttpContext.User.Identity != null) ? HttpContext.User.Identity.Name: null;
            if (userName == null)
                return BadRequest("Could not renew token. No user signed in.");

            var userInfo = new ApplicationUserSignInDTO()
            {
                UserName = userName
            };

            return await BuildToken(userInfo);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpGet("customers", Name = "getAllCustomers")]
        public async Task<List<CustomerDTO>> GetAllCustomers()
        {
            var customers = await context.Customers.AsNoTracking().ToListAsync();
            var customerDTOs = mapper.Map<List<CustomerDTO>>(customers);
            return customerDTOs;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpGet("owners", Name = "getAllOwners")]
        public async Task<List<OwnerDTO>> GetAllOwners()
        {
            var owners = await context.Owners.AsNoTracking().ToListAsync();
            var ownerDTOs = mapper.Map<List<OwnerDTO>>(owners);
            return ownerDTOs;
        }

        private async Task<UserToken> BuildToken(ApplicationUserSignInDTO model)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, model.UserName),
            };

            var identityUser = await userManager.FindByNameAsync(model.UserName);
            var claimsDb = await userManager.GetClaimsAsync(identityUser);

            claims.AddRange(claimsDb);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddMinutes(30);

            JwtSecurityToken token = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiration, signingCredentials: creds);

            return new UserToken() { Token = new JwtSecurityTokenHandler().WriteToken(token), ExpirationDate = expiration };
        }
    }

    public class UserToken
    {
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
    }

    public class UserLoginToken
    {
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Role { get; set; }
    }
}
