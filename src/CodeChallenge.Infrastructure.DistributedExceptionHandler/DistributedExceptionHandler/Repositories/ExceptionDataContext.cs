using DistributedExceptionHandler.Models;
using Microsoft.EntityFrameworkCore;

namespace DistributedExceptionHandler.Repositories
{
    public class ExceptionDataContext : DbContext
    {
        public ExceptionDataContext(DbContextOptions<ExceptionDataContext> options)
            : base(options) {}

        public DbSet<ExceptionModel> ExceptionModels { get; set;}
    }
}