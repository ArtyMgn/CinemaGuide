using CinemaGuide.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace CinemaGuide.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public ApplicationContext(DbContextOptions options) : base(options)
        {
        }
    }
}
