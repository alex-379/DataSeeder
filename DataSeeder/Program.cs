using DataSeeder.Database;
using DataSeeder.Models;
using EFCore.BulkExtensions;
using Microsoft.Extensions.Options;

namespace DataSeeder;

public static class Program
{
    private const int batchSize = 1000;

    public static async Task Main()
    {
        List<List<Lead>> batches = [];

        DataGenerator.InitBogusData();
        await using var context = new CrmContext();
        for (int i = 0; i < DataGenerator.Leads.Count; i += batchSize)
        {
            batches.Add(DataGenerator.Leads.Skip(i).Take(batchSize).ToList());
        }

        foreach (var batch in batches)
        {
            await context.BulkInsertAsync(batch, options => options.IncludeGraph = true);
            Console.WriteLine($"Iteration {batch} on {batchSize} leads");
        }
    }
}

