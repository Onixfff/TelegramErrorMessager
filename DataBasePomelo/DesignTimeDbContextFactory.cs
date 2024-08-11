using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataBasePomelo
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ErrorsDbContext>
    {
        public ErrorsDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ErrorsDbContext>();
            var connectionString = "Server=127.0.0.1;Database=errors;Uid=root;Pwd=12345;";
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            return new ErrorsDbContext(optionsBuilder.Options);
        }
    }
}
