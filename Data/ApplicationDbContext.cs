using Microsoft.EntityFrameworkCore;
using workload.Models;

namespace workload.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {

        }
        
        public DbSet<Teacher> Teachers { get; set; }
    }
}
