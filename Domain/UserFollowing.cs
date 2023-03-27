namespace Domain
{
    public class UserFollowing
    {
        // Observer is the follower and target is the one being followed
        public string ObserverId { get; set; }
        public AppUser Observer { get; set; }
        public string TargetId { get; set; }
        public AppUser Target { get; set; }
    }
}