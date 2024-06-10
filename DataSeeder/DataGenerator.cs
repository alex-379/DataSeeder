using DataSeeder.Models;
using Bogus;
using DataSeeder.Enums;

namespace DataSeeder;

public class DataGenerator
{
    public static readonly List<Lead> Leads = [];
    public static readonly List<Account> Accounts = [];

    private const int numberOfLeads = 5;
    private const int numberOfAccountsPerLead = 3;
    
    private static Faker<Account> GetAccountGenerator(Guid leadId)
    {
        return new Faker<Account>()
            .RuleFor(e => e.Id, _ => Guid.NewGuid())
            .RuleFor(e => e.Currency, f => f.PickRandom<Currency>());
    }
    
    private static Faker<Lead> GetLeadGenerator()
    {
        return new Faker<Lead>()
            .RuleFor(e => e.Id, _ => Guid.NewGuid())
            .RuleFor(e => e.Name, f => f.Name.FirstName())
            .RuleFor(e => e.Mail, (f, e) => f.Internet.Email(e.Name))
            .RuleFor(e => e.Phone, f => f.Phone.PhoneNumber())
            .RuleFor(e => e.Address, f => f.Address.FullAddress())
            .RuleFor(e => e.BirthDate, f => f.Date.BetweenDateOnly(new DateOnly(1950, 1, 1), new DateOnly(2005, 1, 1)));
    }
    
    private static List<Account> GetBogusAccountData(Guid leadId)
    {
        var accountGenerator = GetAccountGenerator(leadId);
        var generatedLeads = accountGenerator.Generate(numberOfAccountsPerLead);

        return generatedLeads;
    }
    
    public static void InitBogusData()
    {
        var leadGenerator = GetLeadGenerator();
        var generatedLeads = leadGenerator.Generate(numberOfLeads);
        Leads.AddRange(generatedLeads);
        generatedLeads.ForEach(e => Accounts.AddRange(GetBogusAccountData(e.Id)));
    }
}
