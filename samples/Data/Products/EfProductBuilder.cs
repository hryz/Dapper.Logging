using Data.Products.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Products
{
    public static class EfProductBuilder
    {
        public static void Configure(this EntityTypeBuilder<ProductEf> config)
        {
            config.ToTable("Product", "dbo");
            config.HasKey(t => t.Id);
            config.Property(t => t.Id);
            config.Property(t => t.Name);
            config.Property(t => t.Code);
            config.Property(t => t.Price);
            config.Property(t => t.Deleted);
        }
    }
}
