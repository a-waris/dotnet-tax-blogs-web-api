using Taxbox.Domain.Entities;
using Taxbox.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Taxbox.Infrastructure.Configuration;

public class BillingAddressConfiguration : IEntityTypeConfiguration<BillingAddress>
{
    public void Configure(EntityTypeBuilder<BillingAddress> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion<BillingAddressId.EfCoreValueConverter>();
        
        builder.Property(x => x.UserId)
            .HasConversion<UserId.EfCoreValueConverter>();
    }
}