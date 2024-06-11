using DataSeeder.Database;
using EFCore.BulkExtensions;

namespace DataSeeder;

public static class Program
{
    public static async Task Main(string[] args)
    {
        DataGenerator.InitBogusData();
        await using var context = new CrmContext();
        await using var transaction = await context.Database.BeginTransactionAsync();
        await context.BulkInsertAsync(DataGenerator.Leads);
        await context.BulkInsertAsync(DataGenerator.Accounts);
        await transaction.CommitAsync();
    }
}
