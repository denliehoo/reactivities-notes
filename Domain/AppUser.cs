using Microsoft.AspNetCore.Identity;

namespace Domain
{
    // by deriving from IdentityUser,
    // we got lots of extra properties e.g.
    // Email,Username,PasswordHash,etc...
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public string Bio { get; set; }
        public ICollection<ActivityAttendee> Activities { get; set; }
        public ICollection<Photo> Photos { get; set; }
    }
}