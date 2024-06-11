using DataSeeder.Database;
using EFCore.BulkExtensions;

namespace DataSeeder;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var database = Environment.GetEnvironmentVariable("CrmLocalDb_ENVIRONMENT");
        DataGenerator.InitBogusData();
        await using var context = new CrmContext();
        await context.BulkInsertAsync(DataGenerator.Leads, options => options.BatchSize = 100);
        await context.BulkInsertAsync(DataGenerator.Accounts, options => options.BatchSize = 100);
    }
}
