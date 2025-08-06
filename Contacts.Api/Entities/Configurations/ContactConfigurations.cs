using Microsoft.EntityFrameworkCore;

namespace Contacts.Api.Entities.Configurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Contacts.Api.Entities;

public class ContactConfigurations : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.HasKey(c => c.Id);
        builder.HasIndex(c => c.PhoneNumber).IsUnique();
        builder.Property(c => c.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(c => c.LastName).IsRequired().HasMaxLength(50);
        builder.Property(c => c.Email).HasMaxLength(100);
        builder.Property(c => c.PhoneNumber).IsRequired().HasMaxLength(13).IsFixedLength();
        builder.Property(c => c.Address).HasMaxLength(200);
        builder.Property(c => c.CreatedAt);
        builder.Property(c => c.UpdatedAt);
    }
}