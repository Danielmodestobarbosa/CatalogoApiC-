using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CatalogoApiNovo.Data
{

    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext> { 

        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            optionsBuilder.UseSqlServer(
                "Server=.;Database=DB_CatalogoApiNovo;User Id=sa;Password=daniel123;TrustServerCertificate=True;"
            );

            return new AppDbContext(optionsBuilder.Options);
        }

    }
}
