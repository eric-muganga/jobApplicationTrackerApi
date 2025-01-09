using AutoMapper;
using jobApplicationTrackerApi.DataModels;
using jobApplicationTrackerApi.Services;
using jobApplicationTrackerApi.ViewModels;

namespace jobApplicationTrackerApi.Controllers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Mapping from DataModels -> ViewModels

        CreateMap<JobApplication, JobApplicationView>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Name))
            .ReverseMap()
            .ForMember(dest => dest.Status, opt => opt.Ignore());

        CreateMap<JobApplicationHistory, JobApplicationHistoryView>()
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.StatusId.ToString()));

        CreateMap<Status, StatusView>().ReverseMap();

        CreateMap<Interview, InterviewView>().ReverseMap();

        CreateMap<FinancialInformation, FinancialInformationView>().ReverseMap();

        CreateMap<ContractType, ContractTypeView>().ReverseMap();


        //Mapping from ViewModels -> DataModels

        CreateMap<JobApplicationHistoryView, JobApplicationHistory>()
            .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => Guid.Parse(src.StatusName)));


        //CreateMap<InterviewView, Interview>();


        CreateMap<ServiceResponse<JobApplication>,  ServiceResponse<JobApplicationView>>().ReverseMap();

        CreateMap<ServiceResponse<Interview>, ServiceResponse<InterviewView>>().ReverseMap();
    }
}
