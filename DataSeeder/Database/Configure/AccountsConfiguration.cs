using DataSeeder.Enums;
using DataSeeder.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataSeeder.Database.Configure;

public class AccountsConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder
            .HasKey(d => d.Id);
        builder
            .HasOne(a => a.Lead)
            .WithMany(l => l.Accounts);
        builder
            .Property(d => d.Currency)
            .IsRequired();
        builder
            .Property(u => u.Status)
            .HasDefaultValue(AccountStatus.Active)
            .HasSentinel(AccountStatus.Unknown);
    }
}
