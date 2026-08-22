using Application.DTOs.Identity;
using Application.DTOs.User;
using AutoMapper;
using Domain.Entities;

namespace Application.MappingProfiles
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

            CreateMap<ApplicationUser, UserDto>()
                .ConstructUsing(src => new UserDto()
                {
                    Id = src.Id,
                    Email = src.Email ?? "",
                    Name = src.UserName ?? "",
                })
                .ForAllMembers(opt => opt.Ignore());
        }
    }
}
