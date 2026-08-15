using Application.DTOs.ToDoList;
using AutoMapper;
using Domain.Entities;

namespace Application.MappingProfiles
{
    public class ToDoListMappingProfile : Profile
    {
        public ToDoListMappingProfile()
        {
            CreateMap<ToDoListDto, ToDoListEntity>().ReverseMap();

            CreateMap<CreateToDoListDto, ToDoListEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<UpdateToDoListDto, ToDoListEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
