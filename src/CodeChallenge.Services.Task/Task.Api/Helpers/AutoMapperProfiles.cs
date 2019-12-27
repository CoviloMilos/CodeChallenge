using AutoMapper;
using Task.Api.Models;
using TaskManagementAPI.Dtos;

namespace Task.Api.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<DtoCreateCase, Case>()
                .ForMember(dest => dest.CaseId, opt => opt.Ignore())
                .ForMember(dest => dest.CaseNum, opt => opt.Ignore())
                .ForMember(dest => dest.TaskGuid, opt => opt.Ignore());

            CreateMap<DtoCreateTask, TaskModel>()
                .ForMember(dest => dest.TaskId, opt => opt.Ignore())
                .ForMember(dest => dest.TaskNum, opt => opt.Ignore())
                .ForMember(dest => dest.TaskGuid, opt => opt.Ignore())
                .ForMember(dest => dest.Cases, opt => opt.MapFrom(src => src.CreateCases));   

            CreateMap<DtoUpdateTask, TaskModel>()
                .ForMember(dest => dest.TaskId, opt => opt.Ignore())
                .ForMember(dest => dest.TaskNum, opt => opt.Ignore())
                .ForMember(dest => dest.TaskGuid, opt => opt.Ignore());

            CreateMap<DtoUpdateCase, Models.Case>()
                .ForMember(dest => dest.CaseId, opt => opt.Ignore())
                .ForMember(dest => dest.CaseNum, opt => opt.Ignore())
                .ForMember(dest => dest.TaskGuid, opt => opt.Ignore());
                
            CreateMap<Case, DtoGetCase>();

            CreateMap<TaskModel, DtoGetTask>();
        }
    }
}