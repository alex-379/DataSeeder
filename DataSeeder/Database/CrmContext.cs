using DataSeeder.Enums;
using DataSeeder.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace DataSeeder.Database;

public class CrmContext(DbContextOptions<CrmContext> options) : DbContext(options)
{
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
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(Environment.GetEnvironmentVariable("CrmLocalDb_ENVIRONMENT"));
        dataSourceBuilder.MapEnum<AccountStatus>();
        dataSourceBuilder.MapEnum<Currency>();
        dataSourceBuilder.MapEnum<LeadStatus>();
        var dataSource = dataSourceBuilder.Build();

        return dataSource;
    }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsForEntitiesInContext();
        modelBuilder.ConfigureEnums();
    }
}
