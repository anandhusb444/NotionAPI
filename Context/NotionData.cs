using Microsoft.EntityFrameworkCore;
using NotionAPI.Models;

namespace NotionAPI.Context
{
    public class NotionData : DbContext
    {
        public NotionData(DbContextOptions<NotionData> options):base(options)
        {

        }

        public DbSet<Users> users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>()
                .HasKey(user => user.Id);

            base.OnModelCreating(modelBuilder);
        }

    }
}
