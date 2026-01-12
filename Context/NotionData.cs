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
        public DbSet<TodoTasks> Tasks { get; set; }
        public DbSet<TaskDescription> TaskDescription { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>()
                .HasKey(user => user.Id);

            modelBuilder.Entity<TodoTasks>()
                .HasKey(task => task.Id);

            modelBuilder.Entity<TaskDescription>()
                .HasKey(desc => desc.Id);

            modelBuilder.Entity<TodoTasks>()
                .HasMany(descr => descr.TaskDescription)
                .WithOne(task => task.TodaTask)
                .HasForeignKey(f => f.TaskTd);

            modelBuilder.Entity<TodoTasks>()
                .HasOne(u => u.Users)
                .WithMany(t => t.Tasks)
                .HasForeignKey(f => f.UserId);

            base.OnModelCreating(modelBuilder);
        }

    }
}
