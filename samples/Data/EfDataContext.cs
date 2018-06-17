using Data.Products;
using Data.Products.ReadModels;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class EfDataContext : DbContext
    {
        public EfDataContext(DbContextOptions<EfDataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductEf>().Configure();
        }

        public DbSet<ProductEf> Products { get; set; }
    }
}
