using DataSeeder.Models;
using Bogus;
using DataSeeder.Database;
using DataSeeder.Enums;
using Microsoft.EntityFrameworkCore;

namespace DataSeeder;

public class DataGenerator
{
    
    public static readonly List<Lead> Leads = [];
    public static readonly List<Account> Accounts = [];

    private const int numberOfRegularLeads = 10;
    private const int numberOfVipLeads = 2;
    private const int numberOfAccountsPerLead = 3;
    
    public static List<Lead> GetSeededLeadsFromDb()
    {
        using var context = new CrmContext();
        context.Database.EnsureCreated();
        var dbLeadsWithAccounts = context.Leads
            .Include(e => e.Accounts)
            .ToList();
        return dbLeadsWithAccounts;
    }
    
    private static Faker<Account> GetAccountGenerator(Guid leadId)
    {
        return new Faker<Account>()
            .RuleFor(e => e.Id, _ => Guid.NewGuid())
            .RuleFor(e => e.Currency, f => f.PickRandom<Currency>());
    }
    
    private static Faker<Lead> GetLeadGenerator(LeadStatus status)
    {
        var password = PasswordsService.HashPassword("Password123!", @"$^$%GTRGRTVdsfcdsr3dsadsa%^%#\$tgregf345#$%34534RTferfer32423DFESd23\$232#$%;%%%\$TRGFSFDSdsfsee3r3*^U^HGFHF");
        
        return new Faker<Lead>()
            .RuleFor(e => e.Id, _ => Guid.NewGuid())
            .RuleFor(e => e.Name, f => f.Name.FirstName())
            .RuleFor(e => e.Mail, (f, e) => f.Internet.Email(e.Name).ToLower())
            .RuleFor(e => e.Phone, f => f.Phone.PhoneNumber())
            .RuleFor(e => e.Address, f => f.Address.FullAddress())
            .RuleFor(e => e.BirthDate, f => f.Date.BetweenDateOnly(new DateOnly(1950, 1, 1), new DateOnly(2005, 1, 1)))
            .RuleFor(e => e.Status, _ => status)
            .RuleFor(e => e.Password, _ => password.hash)
            .RuleFor(e => e.Salt, _ => password.salt);
    }
    
    private static List<Account> GetBogusAccountData(Guid leadId)
    {
        var accountGenerator = GetAccountGenerator(leadId);
        var generatedLeads = accountGenerator.Generate(numberOfAccountsPerLead);

        return generatedLeads;
    }
    
    public static void InitBogusData()
    {
        GeneratedLeads(numberOfRegularLeads);
        GeneratedLeads(numberOfVipLeads, LeadStatus.Vip);

        //generatedLeads.ForEach(e => Accounts.AddRange(GetBogusAccountData(e.Id)));
    }

    private static void GeneratedLeads(int numberOfLeads, LeadStatus status = LeadStatus.Regular)
    {
        var leadGenerator = GetLeadGenerator(status);
        var generatedLeads = leadGenerator.Generate(numberOfLeads);
        Leads.AddRange(generatedLeads);
    }
    

}
