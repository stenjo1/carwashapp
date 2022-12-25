using AutoMapper;
using CarWashApp.DTOs;
using CarWashApp.Entities;
using CarWashApp.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace CarWashApp.Controllers
{
    [ApiController]
    [Route("/api/appointments")]
    public class AppointmentController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;

        public AppointmentController(ApplicationDbContext context, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "UserAccess")]
        [HttpGet("{id:int}", Name = "getAppointment")]
        public async Task<ActionResult<AppointmentDetailsDTO>> GetAppointment(int id)
        {
            var appointment = await context.Appointments.Include(x => x.CarWash)
                                                        .Include(x => x.Customer)
                                                        .FirstOrDefaultAsync(a => a.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var currUserId = user.Id;

            if (appointment.CustomerId != currUserId && appointment.CarWash.OwnerId != currUserId)
            {
                return StatusCode(403);
            }

            var appointmentDTO = mapper.Map<AppointmentDetailsDTO>(appointment);
            return appointmentDTO;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "UserAccess")]
        [HttpPatch("cancel/{id:int}")]
        public async Task<ActionResult> CancelAppointment(int id)
        {
            var appointment = await context.Appointments.Include(x => x.CarWash)
                                                        .FirstOrDefaultAsync(x => x.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var currUserId = user.Id;

            if (appointment.CustomerId != currUserId && appointment.CarWash.OwnerId != currUserId)
            {
                return StatusCode(403);
            }

            var appDateTime = appointment.Date.AddHours(appointment.StartHour);
            var currentTime = DateTime.Now.AddMinutes(15);
            if (currentTime > appDateTime)
            {
                //cancellation time limited to 15 minutes
                return BadRequest("Cancellation time limited to 15 minutes");
            }

            appointment.Status = Status.Declined;
            await context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Get all appointments made by logged in customer
        /// </summary>
        /// <returns></returns>
        //filtering in sql
        [HttpGet("customer", Name = "getCustomerAppointments")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Customer")]
        public async Task<List<AppointmentCustomerDTO>> GetCustomerAppointments([FromQuery] AppointmentFilter appointmentFilter)
        {
            var appointmentsQueryable = context.Appointments.Include(x => x.Customer)
                .Include(a => a.CarWash)
                .AsQueryable()
                .Where(x => x.Customer.UserName == User.Identity.Name);

            if (!String.IsNullOrWhiteSpace(appointmentFilter.ServiceType))
            {
                appointmentsQueryable = appointmentsQueryable.Where(x => x.ServiceType == appointmentFilter.ServiceType);
            }

            if (!String.IsNullOrWhiteSpace(appointmentFilter.Status))
            {
                try
                {
                    var statusInt = (Status)Enum.Parse(typeof(Status), appointmentFilter.Status);
                    appointmentsQueryable = appointmentsQueryable.Where(x => x.Status == statusInt);
                }
                catch
                {
                    //log this
                }
            }

            if (appointmentFilter.Rating != 0u)
            {
                appointmentsQueryable = appointmentsQueryable.Where(x => x.Rating == appointmentFilter.Rating);
            }

            if (appointmentFilter.IsFinished)
            {
                appointmentsQueryable = appointmentsQueryable.Where(x => x.IsFinished);
            }

            if (appointmentFilter.CarWashId != 0)
            {
                appointmentsQueryable = appointmentsQueryable.Where(x => x.CarWashId == appointmentFilter.CarWashId);
            }

            if (!string.IsNullOrWhiteSpace(appointmentFilter.OrderingField))
            {
                try
                {
                    appointmentsQueryable = appointmentsQueryable
                        .OrderBy($"{appointmentFilter.OrderingField} {(appointmentFilter.Ascending ? "ascending" : "descending")}");
                }
                catch
                {
                    // log this
                }
            }

            await HttpContext.InsertPaginationParametersInResponse(appointmentsQueryable, appointmentFilter.RecordsPerPage);

            var pagination = new PaginationDTO() { Page = appointmentFilter.Page, RecordsPerPage = appointmentFilter.RecordsPerPage };
            var appointments = await appointmentsQueryable.Paginate(pagination).ToListAsync();
            return mapper.Map<List<AppointmentCustomerDTO>>(appointments);
        }

        //provere u front
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Customer")]
        [HttpPost("create", Name = "postCustomerAppointment")]
        public async Task<ActionResult> PostCustomerAppointment([FromBody] AppointmentCreationDTO appointmentCreationDTO)
        {
            var carWashId = appointmentCreationDTO.CarWashId;
            var carWash = await context.CarWashes.Include(x => x.Appointments)
                                                 .Include(x => x.Services)
                                                 .FirstOrDefaultAsync(x => x.Id == carWashId);

            if (appointmentCreationDTO.StartHour < carWash.OpeningHour || appointmentCreationDTO.StartHour >= carWash.ClosingHour)
            {
                //carwash doesnt work 
                return BadRequest("CarWash is closed");
            }
            if (appointmentCreationDTO.StartHour > appointmentCreationDTO.EndHour) 
                return BadRequest("Invalid start and end time");

            var appointments = carWash.Appointments.Where(x => x.Date == appointmentCreationDTO.Date).Select(x => (x.StartHour, x.EndHour)).ToList();
            appointments.Add((appointmentCreationDTO.StartHour, appointmentCreationDTO.EndHour));

            var service = carWash.Services.FirstOrDefault(x => x.ServiceType == appointmentCreationDTO.ServiceType);
            if (service == null)
            {
                //no available service
                return BadRequest("Specified service is not available in this CarWash");
            }

            if (appointmentCreationDTO.EndHour != appointmentCreationDTO.StartHour + service.Duration)
            {
                //service has a different duration, endhour updated
                return BadRequest($"Service duration is {service.Duration}");
                //appointmentCreationDTO.EndHour = appointmentCreationDTO.StartHour + service.Duration;
            }

            var isAvailable = IsAvailable(appointments, carWash.OpeningHour, carWash.ClosingHour, carWash.Size);
            if (!isAvailable)
            {
                //log cannot make an appointment
                return BadRequest("No available slots");
            }

            var appointment = mapper.Map<Appointment>(appointmentCreationDTO);
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var customerId = user.Id;
            appointment.CustomerId = customerId;
            appointment.IsFinished = false;
            appointment.Status = Status.Pending;
            appointment.DateCreated = DateTime.Now;
            context.Add(appointment);

            await context.SaveChangesAsync();
            var appointmentDTO = mapper.Map<AppointmentCustomerDTO>(appointment);
            return new CreatedAtRouteResult("getAppointment", new { id = appointment.Id }, appointmentDTO);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Customer")]
        [HttpPatch("{id:int}/rate/{rating:int}")]
        public async Task<ActionResult> RateCarWash(int id, int rating)
        {
            if (rating > 5 || rating < 0)
            {
                return BadRequest();
            }

            var appointment = await context.Appointments.Include(x => x.CarWash)
                                                        .FirstOrDefaultAsync(x => x.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var currUserId = user.Id;
            if (appointment.CustomerId != currUserId)
                return StatusCode(403);

            if (!appointment.IsFinished)
            {
                return BadRequest("Appointment has not finished yet");
            }

            appointment.Rating = Convert.ToUInt32(rating);

            //move carwash rating to carwash controller or service
            appointment.CarWash.Rating += rating;
            appointment.CarWash.Votes++;
            await context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Get all appointments scheduled in carwash
        /// </summary>
        /// <returns></returns>
        [HttpGet("carWash/{id:int}", Name = "getCarWashAppointments")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
        public async Task<ActionResult<List<AppointmentCarWashDTO>>> GetCarWashAppointments(int id, [FromQuery] PaginationDTO paginationDTO)
        {
            //var ownerId = userManager.Users.Where(x => x.UserName == User.FindFirstValue(ClaimTypes.Name)).First().Id;
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var ownerId = user.Id;

            var appointmentsQueryable = context.Appointments.Include(x => x.CarWash)
                                                            .Include(x => x.Customer)
                                                            .Where(x => x.CarWashId == id /*&& x.CarWash.OwnerId == ownerId*/)
                                                            .AsQueryable();

            if (!appointmentsQueryable.Any())
            {
                return NotFound();
            }

            if (appointmentsQueryable.FirstOrDefault().CarWash.OwnerId != ownerId)
            {
                return StatusCode(403);
            }

            await HttpContext.InsertPaginationParametersInResponse(appointmentsQueryable, paginationDTO.RecordsPerPage);

            var appointments = await appointmentsQueryable.Paginate(paginationDTO).ToListAsync();

            var appointmentDTOs = mapper.Map<List<AppointmentCarWashDTO>>(appointments);
            return appointmentDTOs;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Owner")]
        [HttpPatch("approve/{id:int}")]
        public async Task<ActionResult> ApproveAppointment(int id, bool isApproved)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var appointment = await context.Appointments.Include(x => x.CarWash)
                                                        .FirstOrDefaultAsync(x => x.Id == id);
            if (appointment == null) return NotFound();
            if (appointment.CarWash.OwnerId != user.Id)
            {
                return StatusCode(403);
            }

            if (appointment.Status != Status.Pending)
            {
                return BadRequest("Status not pending");
            }

            if (isApproved)
            {
                appointment.Status = Status.Approved;
            }
            else
            {
                appointment.Status = Status.Declined;
            }

            await context.SaveChangesAsync();

            return NoContent();
        }


        private bool IsAvailable(List<(int,int)> appointments, uint opening, uint closing, uint slots)
        {
            var size = -opening + closing;
            var tmp = new int[size];

            for (int i = 0; i < appointments.Count; i++)
            {
                var appstart = appointments[i].Item1;
                var append = appointments[i].Item2;
                var index = appstart - opening;
                for (int j = 0; j < append - appstart; j++)
                {
                    tmp[index + j]++;
                    if (tmp[index + j] > slots)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
