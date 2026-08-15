using Application.DTOs.ToDoSubItem;
using AutoMapper;
using Domain.Entities;

namespace Application.MappingProfiles
{
    public class ToDoSubItemDto : Profile
    {
        public ToDoSubItemDto()
        {
            CreateMap<ToDoSubItemDto, ToDoSubItemEntity>().ReverseMap();

            CreateMap<CreateToDoSubItemDto, ToDoSubItemEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<UpdateToDoSubItemDto, ToDoSubItemEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
