using Common.Configurations;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();

        builder.Property(p => p.InsertTime)
            .IsRequired()
            .ValueGeneratedOnUpdate()
            .HasDefaultValueSql("GETDATE()");

        builder.Property(b => b.Name).IsRequired().HasMaxLength(FieldConfig.NameLength);
    }
}