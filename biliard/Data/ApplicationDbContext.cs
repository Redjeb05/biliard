using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using biliard.Data;

namespace biliard.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<biliard.Data.Table>? Table { get; set; }
        public DbSet<biliard.Data.User>? User { get; set; }
        public DbSet<biliard.Data.Reservation>? Reservation { get; set; }
    }
}