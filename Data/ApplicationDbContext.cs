using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using test_fuse.Models.Domains;

namespace test_fuse.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Logo> Logos { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }
    }
}
