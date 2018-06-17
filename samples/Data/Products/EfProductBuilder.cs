using Data.Products.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Products
{
    public static class EfProductBuilder
    {
        public static void Configure(this EntityTypeBuilder<ProductEf> config)
        {
            config.ToTable("Product", "Production");
            config.HasKey(t => t.Id);
            config.Property(t => t.Id).HasColumnName("ProductID");
            config.Property(t => t.Name);
            config.Property(t => t.Code).HasColumnName("ProductNumber");
            config.Property(t => t.Price).HasColumnName("ListPrice").HasColumnType("money");
        }
    }
}
