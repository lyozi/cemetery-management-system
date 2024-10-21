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
                options.UseNpgsql("Host=localhost;Port=5432;Database=TemetoKataszter;Username=lyozi;Password=jozsika20030101;Include Error Detail=true",
                b => b.MigrationsAssembly("Infrastructure")));

        }

        public DbSet<Grave> GraveItems { get; set; }
        public DbSet<GraveUIPolygon> GraveUIPolygons { get; set; }
        public DbSet<Point> Points { get; set; }
        public DbSet<Deceased> DeceasedItems { get; set; }
        public DbSet<Message> MessageItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Grave>()
                .HasMany(g => g.DeceasedList)
                .WithOne(d => d.Grave)
                .HasForeignKey(d => d.GraveId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Grave>()
                .HasOne(g => g.GraveUIPolygon)
                .WithOne(p => p.Grave)
                .HasForeignKey<GraveUIPolygon>(p => p.GraveId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
