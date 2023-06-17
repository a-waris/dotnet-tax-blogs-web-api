using Taxbox.Domain.Entities;
using Taxbox.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Taxbox.Infrastructure.Configuration;

public class UserPaymentMethodConfiguration : IEntityTypeConfiguration<UserPaymentMethod>
{
    public void Configure(EntityTypeBuilder<UserPaymentMethod> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion<UserPaymentMethodId.EfCoreValueConverter>();
        builder.Property(x => x.BillingAddressId)
            .HasConversion<BillingAddressId.EfCoreValueConverter>();
        builder.Property(x => x.UserId)
            .HasConversion<UserId.EfCoreValueConverter>();
    }
}