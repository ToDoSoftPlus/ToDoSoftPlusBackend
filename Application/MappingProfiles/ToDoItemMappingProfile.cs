using Application.DTOs.ToDoItem;
using AutoMapper;
using Domain.Entities;

namespace Application.MappingProfiles
{
    public class ToDoItemMappingProfile : Profile
    {
        public ToDoItemMappingProfile()
        {
            CreateMap<ToDoItemDto, ToDoItemEntity>().ReverseMap();

            CreateMap<CreateToDoItemDto, ToDoItemEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<UpdateToDoItemDto, ToDoItemEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
