using Microsoft.EntityFrameworkCore;
using TechCatRegistry.Core;

namespace TechCatRegistry.Data
{
    public class TechCatDbContext : DbContext
    {
        public DbSet<Ping> Ping { get; set; }

        public TechCatDbContext(DbContextOptions<TechCatDbContext> options) : base(options)
        {
        }
    }
}
