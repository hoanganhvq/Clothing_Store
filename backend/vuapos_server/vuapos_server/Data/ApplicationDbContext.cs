
using Microsoft.EntityFrameworkCore;
using vuapos_server.Models;
namespace vuapos_server.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get;  set; }
    }
}
