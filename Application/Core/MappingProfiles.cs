using AutoMapper;
using Domain;

// Note: the Core folder is responsible for things that are applicable to all our diff features in our Application folder

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Activity, Activity>();
        }
    }
}