using AutoMapper;
using CarWashApp.DTOs;
using CarWashApp.Entities;

namespace CarWashApp.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //Customer mappings
            CreateMap<Customer, CustomerDTO>();
            CreateMap<Customer, CustomerPatchDTO>().ReverseMap();

            //Appointment mappings
            CreateMap<Appointment, AppointmentCarWashDTO>().ForMember(x => x.Customer, o => o.MapFrom(x => x.Customer.FirstName + " " + x.Customer.LastName));
            CreateMap<Appointment, AppointmentCustomerDTO>().ForMember(x => x.CarWashName, o => o.MapFrom(x => x.CarWash.Name))
                                                            .ForMember(x => x.ServiceType, o => o.MapFrom(x => x.ServiceType))
                                                            .ForMember(x => x.Status, o => o.MapFrom(x => x.Status.ToString()));
            CreateMap<Appointment, AppointmentDetailsDTO>().ForMember(x => x.CarWashName, o => o.MapFrom(x => x.CarWash.Name))
                                                           .ForMember(x => x.Customer, o => o.MapFrom(x => x.Customer.FirstName + " " + x.Customer.LastName));
            CreateMap<AppointmentCreationDTO, Appointment>();

            //Owner mappings
            CreateMap<Owner, OwnerDTO>();
            CreateMap<Owner, OwnerPatchDTO>().ReverseMap();

            //CarWash mappings
            CreateMap<CarWash, CarWashDTO>().ForMember(x => x.WorkingHours, o => o.MapFrom(x => $"{x.OpeningHour}h - {x.ClosingHour}h"))
                                            .ForMember(x => x.TotalRating, o => o.MapFrom(x => x.Votes != 0 ? (double)x.Rating / x.Votes : 0.0));
            CreateMap<CarWash, CarWashDetailsDTO>().ForMember(x => x.WorkingHours, o => o.MapFrom(x => $"{x.OpeningHour}h - {x.ClosingHour}h"))
                                            .ForMember(x => x.TotalRating, o => o.MapFrom(x => x.Votes!=0 ? (double)x.Rating/x.Votes : 0.0))
                                            .ForMember(x => x.Appointments, o => o.MapFrom(MapCarWashAppointments))
                                            .ForMember(x => x.Services, o => o.MapFrom(MapCarWashServices));
            CreateMap<CarWashCreationDTO, CarWash>();

            //Revenue mappings
            CreateMap<Revenue, RevenueDTO>();

            //Service mappings
            CreateMap<Service, ServiceDTO>();
            CreateMap<ServiceCreationDTO, Service>();
            CreateMap<ServicePatchDTO, Service>().ReverseMap();
        }           

        private List<AppointmentCarWashDTO> MapCarWashAppointments(CarWash carWash, CarWashDetailsDTO carWashDTO)
        {
            var appointmentDTOs = new List<AppointmentCarWashDTO>();
            foreach(var app in carWash.Appointments)
            {
                var appointmentDTO = new AppointmentCarWashDTO()
                {
                    Id = app.Id,
                    Customer = app.Customer.FirstName + " " + app.Customer.LastName,
                    Date = app.Date,
                    StartHour = app.StartHour,
                    EndHour = app.EndHour,
                    ServiceType = app.ServiceType,
                    Status = app.Status.ToString()
                };
                appointmentDTOs.Add(appointmentDTO);
            }
            return appointmentDTOs;
        }

        private List<ServiceDTO> MapCarWashServices(CarWash carWash, CarWashDetailsDTO carWashDTO)
        {
            var serviceDTOs = new List<ServiceDTO>();
            foreach(var service in carWash.Services)
            {
                var serviceDTO = new ServiceDTO()
                {
                    ServiceType = service.ServiceType,
                    Duration = service.Duration,
                    Price = service.Price
                };
                serviceDTOs.Add(serviceDTO);
            }

            return serviceDTOs;
        }

    }
}
