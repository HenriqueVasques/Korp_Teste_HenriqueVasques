using Microsoft.EntityFrameworkCore;
using Stock.API.Models;

namespace Stock.API.Data.Context
{
    public class AppDbContext : DbContext
    {
       public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions) { }
       public DbSet<Product> Products { get; set; }
    }

    
}
