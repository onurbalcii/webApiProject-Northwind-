using Microsoft.EntityFrameworkCore;

namespace NorthwindMVC
{
    public class NorthwindDbContext : DbContext
    {
        public NorthwindDbContext(DbContextOptions<NorthwindDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Customer> Customers { get; set; }

    }
}
