using APZMS.Application.DTOs;
using APZMS.Domain.Models;
using AutoMapper;

namespace APZMS.Application.Common
{
    public class MappingProfile : Profile
    {
        //This Condition ensures only non-null fields in the DTO overwrite existing values and it is Perfect for PATCH.
        public MappingProfile() 
        {
            CreateMap<BookingPatchDto, Booking>()
            .ForAllMembers(options =>
                options.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
