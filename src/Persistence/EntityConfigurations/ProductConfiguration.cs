using Bogus;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Description).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Category).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Unit).IsRequired();
            builder.Property(x => x.UnitPrice).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.CreatedDate).IsRequired();

            var ids = 1;

            var product = new Faker<Product>()
                .RuleFor(x => x.Id, f => ids++)
                .RuleFor(x => x.Description, f => f.Commerce.ProductDescription())
                .RuleFor(x => x.Unit, f => f.Random.Number(1, 30))
                .RuleFor(x => x.Category, f => f.Commerce.Categories(1).First())
                .RuleFor(x => x.UnitPrice, f => f.Commerce.Price(1).First())
                .RuleFor(x => x.Status, f => f.Random.Bool())
                .RuleFor(x => x.CreatedDate, f => f.Date.Past());

            builder.HasData(product.Generate(1000));
        }
    }
}
