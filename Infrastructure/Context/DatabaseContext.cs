using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Context
{
    public class DatabaseContext : IdentityDbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DatabaseContext>(options =>
                options.UseNpgsql("Host=localhost;Port=5432;Database=TemetoKataszter;Username=lyozi;Password=jozsika20030101;",
                b => b.MigrationsAssembly("Infrastructure")));

        }

        public DbSet<Grave> GraveItems { get; set; }
        public DbSet<GraveUIPolygon> GraveUIPolygons { get; set; }
        public DbSet<Point> Points { get; set; }
        public DbSet<Deceased> DeceasedItems { get; set; }
        public DbSet<Message> MessageItems { get; set; }
    }
}
 