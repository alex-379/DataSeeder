namespace DataSeeder;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Single Employee: ");
        Console.WriteLine(DataGenerator.Leads.First());
    }
}