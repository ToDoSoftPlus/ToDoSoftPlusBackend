using Application.DTOs.Identity;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.Seeders
{
    public class ApplicationUserMappingProfile : Profile
    {
        public ApplicationUserMappingProfile()
        {
            CreateMap<RegisterDto, ApplicationUser>()
                .ConstructUsing(src => new ApplicationUser()
                {
                    Email = src.Email,
                    UserName = src.UserName,
                })
                .ForAllMembers(opt => opt.Ignore());
        }
    }
}
