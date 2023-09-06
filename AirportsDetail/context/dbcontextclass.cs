using CarRental.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CarRental.context
{
    public class DbContextclass:DbContext
    {
        public DbContextclass(DbContextOptions<DbContextclass> options) : base(options)
        {

        }
        public DbSet<admins> admin{ get; set; }
        public DbSet<Cars> Car { get; set; }
        public DbSet<Customers> Customer { get; set; }
        public DbSet<CarRents> CarRent { get; set; }

    }
}
