using System.Text.Json.Serialization;

namespace Application.Profiles
{
    public class UserActivityDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }

        // if we want to add a property to our class,
        // but we dont want to return it to the client, we can use [JsonIgnore]
        [JsonIgnore]
        public string HostUsername { get; set; }
    }
}