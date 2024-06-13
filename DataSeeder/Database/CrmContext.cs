using DataSeeder.Enums;
using DataSeeder.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace DataSeeder.Database;

public class CrmContext : DbContext
{
    private const string database = "CrmLocalDb_ENVIRONMENT";
    public virtual DbSet<Lead> Leads { get; init; } = default;
    public virtual DbSet<Account> Accounts { get; init; } = default;
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql(ConfigureDataSource());
        options.UseSnakeCaseNamingConvention();
        options.EnableSensitiveDataLogging();
    }

    private static NpgsqlDataSource ConfigureDataSource()
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(Environment.GetEnvironmentVariable(database));
        dataSourceBuilder.MapEnum<AccountStatus>();
        dataSourceBuilder.MapEnum<Currency>();
        dataSourceBuilder.MapEnum<LeadStatus>();
        var dataSource = dataSourceBuilder.Build();

        return dataSource;
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Lead>().Property(m => m.Name)
            .HasMaxLength(100);
        modelBuilder.Entity<Lead>().Property(m => m.Mail)
            .HasMaxLength(100);
        modelBuilder.Entity<Lead>().Property(m => m.Phone)
            .HasMaxLength(100);
        modelBuilder.Entity<Lead>().Property(m => m.Address)
            .HasMaxLength(100);
        modelBuilder.Entity<Lead>().Property(m => m.Password)
            .HasMaxLength(100);
        modelBuilder.Entity<Lead>().Property(m => m.Salt)
            .HasMaxLength(100);
        modelBuilder.Entity<Lead>().Property(m => m.RefreshToken)
            .HasMaxLength(100);
    }
}
