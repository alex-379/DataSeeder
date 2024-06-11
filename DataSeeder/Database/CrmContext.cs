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
}
