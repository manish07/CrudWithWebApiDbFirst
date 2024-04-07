using CrudWithWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CrudWithWebAPI.DataAccess
{
    public class MyAppDbContext : DbContext
    {
        public MyAppDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
    }
}