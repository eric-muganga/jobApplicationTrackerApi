using AutoMapper;
using jobApplicationTrackerApi.DataModels;
using jobApplicationTrackerApi.ViewModels;

namespace jobApplicationTrackerApi.Controllers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Mapping from DataModels -> ViewModels

        CreateMap<JobApplication, JobApplicationView>().ReverseMap();

        CreateMap<JobApplicationHistory, JobApplicationHistoryView>()
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.StatusId.ToString()));

        CreateMap<Status, StatusView>().ReverseMap();

        CreateMap<Interview,InterviewView>().ReverseMap();

        CreateMap<FinancialInformation, FinancialInformationView>().ReverseMap();

        CreateMap<ContractType, ContractTypeView>().ReverseMap();


        //Mapping from ViewModels -> DataModels

        CreateMap<JobApplicationHistoryView, JobApplicationHistory>()
            .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => Guid.Parse(src.StatusName)));
    }
}
