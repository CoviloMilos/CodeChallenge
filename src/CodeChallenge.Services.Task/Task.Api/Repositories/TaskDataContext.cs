using Microsoft.EntityFrameworkCore;
using Task.Api.Models;
namespace Task.Api.Repositories
{
    public class TaskDataContext : DbContext
    {
        public TaskDataContext(DbContextOptions<TaskDataContext> options)
            : base(options) {}

        public DbSet<TaskModel> Tasks { get; set; }
        public DbSet<Case> Cases { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskModel>()
                        .Property(t => t.TaskId)
                        .ValueGeneratedOnAdd();

            modelBuilder.Entity<TaskModel>()
                        .Property(t => t.TaskNum)
                        .HasDefaultValueSql("nextval('seq_task_tasknum')");

            modelBuilder.Entity<Case>()
                        .Property(t => t.CaseId)
                        .ValueGeneratedOnAdd();
            
            modelBuilder.Entity<Case>()
                        .Property(t => t.CaseNum)
                        .HasDefaultValueSql("nextval('seq_case_casenum')");

            modelBuilder.Entity<Case>()
                        .HasOne(c => c.Task)
                        .WithMany(t => t.Cases)
                        .HasForeignKey(c => c.TaskGuid)
                        .HasPrincipalKey(t => t.TaskGuid);
        }
        
    }
}