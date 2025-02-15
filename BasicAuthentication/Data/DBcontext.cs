using BasicAuthentication.Models;
using Microsoft.EntityFrameworkCore;

namespace BasicAuthentication.Data
{
    public class DBcontext: DbContext
    {
        public DBcontext(DbContextOptions<DBcontext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
    }
}
