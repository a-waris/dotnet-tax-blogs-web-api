using Taxbox.Domain.Entities;
using Taxbox.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Taxbox.Infrastructure.Configuration;

public class PageConfiguration : IEntityTypeConfiguration<UserSubscription>
{
    public void Configure(EntityTypeBuilder<UserSubscription> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion<UserSubscriptionId.EfCoreValueConverter>();
    }
}