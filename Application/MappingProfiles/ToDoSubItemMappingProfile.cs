using Application.DTOs.ToDoSubItem;
using Application.Models.Pagination;
using AutoMapper;
using Domain.Entities;

namespace Application.MappingProfiles
{
    public class ToDoSubItemMappingProfile : Profile
    {
        public ToDoSubItemMappingProfile()
        {
            CreateMap<ToDoSubItemDto, ToDoSubItemEntity>().ReverseMap();

            CreateMap<CreateToDoSubItemDto, ToDoSubItemEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<UpdateToDoSubItemDto, ToDoSubItemEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<PagedResult<ToDoSubItemEntity>, PagedResult<ToDoSubItemDto>>();
        }
    }
}
