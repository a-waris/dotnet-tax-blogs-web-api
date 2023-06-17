using Taxbox.Domain.Entities;
using Taxbox.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Taxbox.Infrastructure.Configuration;

public class UserSubscriptionConfiguration : IEntityTypeConfiguration<UserSubscription>
{
    public void Configure(EntityTypeBuilder<UserSubscription> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion<UserSubscriptionId.EfCoreValueConverter>();
        builder.Property(x => x.SubscriptionId)
            .HasConversion<SubscriptionId.EfCoreValueConverter>();
        builder.Property(x => x.UserId)
            .HasConversion<UserId.EfCoreValueConverter>();
        builder.Property(x => x.DiscountAmount)
            .HasColumnType("decimal(18,2)");
    }
}