using Microsoft.EntityFrameworkCore;

namespace webmvc.Models
{
    public class ArtContext : DbContext
    {
        public DbSet<Art> Arts { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Exhibition> Exhibitions { get; set; }
        public DbSet<User> Users { get; set; }

        public ArtContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Art>()
                .Property(a => a.Price)
                .HasColumnType("decimal(15, 2)");

            base.OnModelCreating(modelBuilder);
        }
    }
}
