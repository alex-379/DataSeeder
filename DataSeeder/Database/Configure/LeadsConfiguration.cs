using DataSeeder.Enums;
using DataSeeder.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataSeeder.Database.Configure;

public class LeadsConfiguration : IEntityTypeConfiguration<Lead>
{
    public void Configure(EntityTypeBuilder<Lead> builder)
    {
        builder
            .HasKey(d => d.Id);
        builder
            .Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(DatabaseProperties.NameLength);
        builder
            .Property(u => u.Mail)
            .IsRequired()
            .HasMaxLength(DatabaseProperties.MailLength);
        builder
            .Property(u => u.Phone)
            .IsRequired()
            .HasMaxLength(DatabaseProperties.PhoneLength);
        builder
            .Property(u => u.Address)
            .IsRequired()
            .HasMaxLength(DatabaseProperties.AddressLength);
        builder
            .Property(u => u.BirthDate)
            .IsRequired();
        builder
            .Property(u => u.Password)
            .IsRequired()
            .HasMaxLength(DatabaseProperties.PasswordLength);
        builder
            .Property(u => u.Salt)
            .IsRequired()
            .HasMaxLength(DatabaseProperties.SaltLength);
        builder
            .Property(u => u.Status)
            .HasDefaultValue(LeadStatus.Regular)
            .HasSentinel(LeadStatus.Unknown);
        builder
            .Property(u => u.IsDeleted)
            .HasDefaultValue(false);
    }
}
