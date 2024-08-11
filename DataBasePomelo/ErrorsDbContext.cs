using DataBasePomelo.Models;
using Microsoft.EntityFrameworkCore;

namespace DataBasePomelo
{
    public class ErrorsDbContext : DbContext
    {
        public ErrorsDbContext(DbContextOptions<ErrorsDbContext> options) : base(options) { }

        public DbSet<EnumsErrorsEntity> EnumeErrors { get; set; }
        public DbSet<ErrorsEntity> Errors { get; set; }
    }
}
