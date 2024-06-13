using DataSeeder.Database;
using DataSeeder.Models;
using EFCore.BulkExtensions;

namespace DataSeeder;

public static class Program
{
    private const int batchSize = 500000;

    public static async Task Main()
    {
        List<List<Lead>> batches = [];
        var counter = 0;
        
        DataGenerator.InitBogusData();
        ValidationAccounts.CheckAccounts(DataGenerator.Leads);
        await using var context = new CrmContext();
        for (var i = 0; i < DataGenerator.Leads.Count; i += batchSize)
        {
            batches.Add(DataGenerator.Leads.Skip(i).Take(batchSize).ToList());
        }

        foreach (var batch in batches)
        {
            await context.BulkInsertAsync(batch, options => options.IncludeGraph = true);
            Console.WriteLine($"Iteration {counter} on {batchSize} leads");
            counter++;
        }
    }
}

