using DataSeeder.Core.Enums;
using DataSeeder.Core.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace DataSeeder.Core.Database
{
    public class CrmContext : DbContext
    {
        private const string database = "CrmDb_ENVIRONMENT";

        public virtual DbSet<Lead> Leads { get; init; } = default;
        public virtual DbSet<Account> Accounts { get; init; } = default;

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var dataSource = ConfigureDataSource();
            options.UseNpgsql(dataSource);
            options.UseSnakeCaseNamingConvention();
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

        public static async Task ExecuteWithRetryAsync(Func<CancellationToken, Task> action, CancellationToken cancellationToken = default)
        {
            var retryCount = 0;
            const int maxRetryCount = 100;
            const int initialRetryDelay = 500;

            while (retryCount < maxRetryCount)
            {
                try
                {
                    await action(cancellationToken);
                    return;
                }
                catch (Exception)
                {
                    retryCount++;
                    var retryDelay = (int)Math.Pow(2, retryCount - 1) * initialRetryDelay;
                    Console.WriteLine($"Database connection error. Retry #{retryCount} after {retryDelay} ms.");
                    await Task.Delay(retryDelay, cancellationToken);
                }
            }

            throw new Exception("Failed to connect to the database after several attempts.");
        }
    }
}
