using Taxbox.Domain.Entities;
using Taxbox.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BC = BCrypt.Net.BCrypt;

namespace Taxbox.Infrastructure.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion<UserId.EfCoreValueConverter>();
        builder.Property(x => x.Email).IsRequired().HasMaxLength(254);
        builder.HasIndex(x => x.Email).IsUnique();
        builder.Property(x => x.FirstName).HasMaxLength(254);
        builder.Property(x => x.LastName).HasMaxLength(254);
        builder.Property(x => x.DisplayPicture).HasMaxLength(2048);
    }
}