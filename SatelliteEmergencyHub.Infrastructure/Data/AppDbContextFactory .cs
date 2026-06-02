using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SatelliteEmergencyHub.Infrastructure.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql("Host=localhost;Port=5432;Database=satellite_emergency_hub;Username=postgres;Password=postgres")
                .Options;

            return new AppDbContext(options);
        }
    }
}
