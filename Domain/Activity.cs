namespace Domain
{
    // entity AKA models is just something that we will store in our DB
    // class will be the table name in the DB and each properties is a column
    public class Activity
    {
        // must be this naming so that entity framework knows to recognise this as the primary key
        public Guid Id { get; set; }

        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string City { get; set; }
        public string Venue { get; set; }
        public ICollection<ActivityAttendee> Attendees { get; set; } = new List<ActivityAttendee>();
    }
}