using AutoMapper;
using jobApplicationTrackerApi.DataModels;
using jobApplicationTrackerApi.ViewModels;

namespace jobApplicationTrackerApi.Controllers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Mapping from DataModels -> ViewModels
        CreateMap<JobApplication, JobApplicationView>().ReverseMap();    //UserId exists in one, //4

        CreateMap<JobApplicationHistory, JobApplicationHistoryView>()
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.StatusId))
            .ReverseMap();

        CreateMap<Status, StatusView>().ReverseMap();

        CreateMap<Interview,InterviewView>().ReverseMap();

        CreateMap<FinancialInformation, FinancialInformationView>().ReverseMap();

        CreateMap<ContractType, ContractTypeView>().ReverseMap();

        //CreateMap<Source, Destination>()
        //.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));


        // Ignoring unnecessary properties
        CreateMap<JobApplicationView, JobApplication>()
    .ForMember(dest => dest.UserId, opt => opt.Ignore());

        //ex data - view (in controller)
        //var jobApplication = new JobApplication { Id = 1, JobTitle = "Software Developer", UserId = 123 };
        //var jobApplicationView = _mapper.Map<JobApplicationView>(jobApplication);

        //ex view - data (in controller)
        //var jobApplicationView = new JobApplicationView { Id = 1, JobTitle = "Software Developer" };
        //var jobApplication = _mapper.Map<JobApplication>(jobApplicationView);

        // Mapping for User -> UserViewModel
        //CreateMap<User, UserViewModel>();     // ex. Status -> StatusView  extra value is just ignored


        // If needed, mapping for UserViewModel -> User (Reverse Mapping)
        //CreateMap<UserViewModel, User>()
        //.ForMember(dest => dest.Id, opt => opt.Ignore()); // Ignore Id in reverse mapping
    }
}
