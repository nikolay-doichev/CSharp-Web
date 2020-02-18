namespace SharedTrip
{
    using Microsoft.EntityFrameworkCore;
    using SharedTrip.Models;

    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<UserTrip> UserTrips { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(DatabaseConfiguration.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserTrip>(entity =>
            {
                entity
                    .HasKey(ut => new {ut.UserId, ut.TripId });

                entity
                    .HasOne(t => t.Trip)
                    .WithMany(ut => ut.UserTrips)
                    .HasForeignKey(t => t.TripId);

                entity
                    .HasOne(u => u.User)
                    .WithMany(ut => ut.UserTrips)
                    .HasForeignKey(u => u.UserId);
            });
        }
    }
}
