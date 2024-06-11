using DataSeeder.Database;
using EFCore.BulkExtensions;

namespace DataSeeder;

public static class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Initializing data with Bogus...");
        DataGenerator.InitBogusData();

        Console.WriteLine("-----------------");

        Console.WriteLine("Single Employee: ");
        Console.WriteLine(DataGenerator.Leads.First());

        Console.WriteLine("-----------------");

        Console.WriteLine("Multiple Employees: ");
        DataGenerator.Leads.ForEach(Console.WriteLine);

        Console.WriteLine("-----------------");

        Console.WriteLine("DB Seeded Employees: ");
        DataGenerator.GetSeededLeadsFromDb().ForEach(Console.WriteLine);

        await using var context = new CrmContext();
        
        await context.BulkInsertAsync(DataGenerator.Leads);
    }
}