using Microsoft.EntityFrameworkCore;
using StarChart.Models;

namespace StarChart.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<CelestialObject> CelesstialObjects;

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
