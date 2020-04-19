using Data.Products.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Products
{
    public static class EfProductBuilder
    {
        public static void Configure(this EntityTypeBuilder<ProductEf> config)
        {
            config.ToTable("product", "public");
            config.HasKey(t => t.Id);
            config.Property(t => t.Id).HasColumnName("id");
            config.Property(t => t.Name).HasColumnName("name");
            config.Property(t => t.Code).HasColumnName("code");
            config.Property(t => t.Price).HasColumnName("price");
            config.Property(t => t.Deleted).HasColumnName("deleted");
        }
    }
}
