using DataSeeder.Core;
using DataSeeder.Core.Database;
using DataSeeder.Core.Exceptions;
using DataSeeder.Core.Models;
using EFCore.BulkExtensions;

namespace DataSeeder.DataSeeder;

public static class Program
{
    private const int batchSize = 500000;

    public static async Task Main()
    {
        DataGenerator.InitBogusData();
        if (!ValidationAccounts.CheckAccounts(DataGenerator.Leads))
        {
            throw new ValidationException();
        };
        
        await AddBatches();
    }

    private static async Task AddBatches()
    {
        List<List<Lead>> batches = [];
        var counter = 0;
        await using var context = new CrmContext();
        for (var i = 0; i < DataGenerator.Leads.Count; i += batchSize)
        {
            batches.Add(DataGenerator.Leads.Skip(i).Take(batchSize).ToList());
        }

        foreach (var batch in batches)
        {
            await CrmContext.ExecuteWithRetryAsync(async (cancellationToken) =>
            {
                await context.BulkInsertAsync(batch, options => options.IncludeGraph = true, cancellationToken: cancellationToken);
            });
            Console.WriteLine($"Iteration {counter} on {batchSize} leads");
            counter++;
        }
    }
}

