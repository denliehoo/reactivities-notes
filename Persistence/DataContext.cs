using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityAttendee> ActivityAttendees { get; set; }
        // overriding the  OnModelCreating method from our IdentityDbContext
        // over here, we can add additional configurations 
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // now we have access to entity configurations

            // Telling the activityattendee to have a primary key which is a combination
            // of the app user id and the activity id
            builder.Entity<ActivityAttendee>(x => x.HasKey(aa => new { aa.AppUserId, aa.ActivityId }));

            // establishing that the activityattendee has
            // one app user, many activities, and a foreign key of the app user table
            builder.Entity<ActivityAttendee>()
                .HasOne(u => u.AppUser)
                .WithMany(a => a.Activities)
                .HasForeignKey(aa => aa.AppUserId);

            // establishing that the activityattendee has
            // one one activity, many attendees and a foreign key of the activities table
            builder.Entity<ActivityAttendee>()
                .HasOne(u => u.Activity)
                .WithMany(a => a.Attendees)
                .HasForeignKey(aa => aa.ActivityId);
        }
        // hence, we have established a many to many relationship between
        // user and activity using ActivityAttendee as a link
        // since user has one to many w/ activity attendee
        // and activity has one to many w/ activity attendee

    }
}