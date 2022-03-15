using EMS.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Student>? Students { get; set; }
    }
}
