using Application.Activities;
using Application.Comments;
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

            // for the activity, maps host user name to the first or default attendee which is the host and gives the username in the ActivityDto
            CreateMap<Activity, ActivityDto>()
                .ForMember(d => d.HostUsername, o => o.MapFrom(s => s.Attendees
                .FirstOrDefault(x => x.IsHost).AppUser.UserName));

            CreateMap<ActivityAttendee, AttendeeDto>()
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.AppUser.DisplayName))
                .ForMember(d => d.Username, o => o.MapFrom(s => s.AppUser.UserName))
                .ForMember(d => d.Bio, o => o.MapFrom(s => s.AppUser.Bio))
                .ForMember(d => d.Image, o => o.MapFrom(s => s.AppUser.Photos.FirstOrDefault(x => x.IsMain).Url));
            CreateMap<AppUser, Profiles.Profile>()
                .ForMember(d => d.Image, o => o.MapFrom(s => s.Photos.FirstOrDefault(x => x.IsMain).Url));

            CreateMap<Comment, CommentDto>()
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.Author.DisplayName))
                .ForMember(d => d.Username, o => o.MapFrom(s => s.Author.UserName))
                .ForMember(d => d.Image, o => o.MapFrom(s => s.Author.Photos.FirstOrDefault(x => x.IsMain).Url));

        }
    }
}