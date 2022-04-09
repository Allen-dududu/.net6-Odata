using Microsoft.EntityFrameworkCore;

namespace OdataDemo
{
    public class OdataOrderContext : DbContext
    {
        public OdataOrderContext(DbContextOptions<OdataOrderContext> dbContext):base(dbContext)
        {

        }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Customer> Customers { get; set; }
    }
}
