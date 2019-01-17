using DISATPN.Web.Models.Context;
using Microsoft.EntityFrameworkCore;

namespace DISATPN.Web.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public DbSet<Visit> Visits { get; set; }

        public DbSet<Path> Paths { get; set; }
    }
}
