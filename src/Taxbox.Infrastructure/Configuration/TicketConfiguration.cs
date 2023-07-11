using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taxbox.Domain.Entities;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Infrastructure.Configuration;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion<TicketId.EfCoreValueConverter>();
        builder.Property(x => x.SubscriptionId)
            .HasConversion<SubscriptionId.EfCoreValueConverter>();

        builder.HasOne(x => x.Subscription)
            .WithMany(x => x.Tickets)
            .HasForeignKey(x => x.SubscriptionId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}