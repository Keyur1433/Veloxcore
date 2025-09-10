using APZMS.DTOs;
using APZMS.Models;
using AutoMapper;

namespace APZMS.Utilities
{
    public class MappingProfile : Profile
    {
        //That Condition ensures only non-null fields in the DTO overwrite existing values → perfect for PATCH.
        public MappingProfile() 
        {
            CreateMap<BookingPatchDto, Booking>()
            .ForAllMembers(options =>
                options.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
