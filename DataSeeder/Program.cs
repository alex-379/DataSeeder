using DataSeeder.Database;
using EFCore.BulkExtensions;

namespace DataSeeder;

public static class Program
{
    public static async Task Main(string[] args)
    {
        DataGenerator.InitBogusData();

        await using var context = new CrmContext();
        await context.BulkInsertAsync(DataGenerator.Leads);
        foreach (var lead in DataGenerator.Leads)
        {
            await context.BulkInsertAsync(lead.Accounts);
        }
        
        Console.WriteLine("-----------------");

        Console.WriteLine("DB Seeded Employees: ");
        DataGenerator.GetSeededLeadsFromDb().ForEach(Console.WriteLine);
    }
}
