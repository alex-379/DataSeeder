using DataSeeder.Models;
using Bogus;
using DataSeeder.Enums;

namespace DataSeeder;

public class DataGenerator
{
    public static readonly List<Lead> Leads = [];
    private const int numberOfLeads = 40000;
    private const int percentRegularLeads = 80;
    private const int percentVipLeads = 20;
    private const int percentAll = 100;
    private const string passwordLead = "Password_ENVIRONMENT";
    private const string secret = "SecretPassword_ENVIRONMENT";
    private const string lastName = "";
    private const string provider = "crm.ru";

    
    private static Faker<Account> GetAccountGenerator(Guid leadId, Currency currency)
    {
        return new Faker<Account>()
            .RuleFor(e => e.Id, _ => Guid.NewGuid())
            .RuleFor(e => e.Currency, _ => currency);
    }
    
    private static Faker<Lead> GetLeadGenerator(LeadStatus status)
    {
        var password = PasswordsService.HashPassword(Environment.GetEnvironmentVariable(passwordLead), Environment.GetEnvironmentVariable(secret));
        var counter = 0;
        
        return new Faker<Lead>()
            .RuleFor(e => e.Id, _ => Guid.NewGuid())
            .RuleFor(e => e.Name, f => f.Name.FirstName())
            .RuleFor(e => e.Mail, (f, e) => f.Internet.Email(e.Name, lastName, provider,(counter++).ToString()).ToLower())
            .RuleFor(e => e.Phone, f => f.Phone.PhoneNumberFormat())
            .RuleFor(e => e.Address, f => f.Address.StreetAddress())
            .RuleFor(e => e.BirthDate, f => f.Date.BetweenDateOnly(new DateOnly(1950, 1, 1), new DateOnly(2005, 1, 1)))
            .RuleFor(e => e.Status, _ => status)
            .RuleFor(e => e.Password, _ => password.hash)
            .RuleFor(e => e.Salt, _ => password.salt);
    }
    
    public static void InitBogusData()
    {
        GeneratedLeads(numberOfLeads * percentRegularLeads/percentAll);
        GeneratedLeads(numberOfLeads * percentVipLeads/percentAll, LeadStatus.Vip);

        foreach (var lead in Leads)
        {
            lead.Accounts = GeneratedAccountsForLead(lead);
            if (lead.Status == LeadStatus.Vip)
            {
                lead.Accounts.AddRange(GeneratedAccountsForVipLead(lead));
            }
        }
    }

    private static void GeneratedLeads(int numberOfLeadsWithStatus, LeadStatus status = LeadStatus.Regular)
    {
        var leadGenerator = GetLeadGenerator(status);
        var generatedLeads = leadGenerator.Generate(numberOfLeadsWithStatus);
        Leads.AddRange(generatedLeads);
    }

    private static List<Account> GeneratedAccountsForLead(Lead lead)
    {
        List<Account> accountsForLead =
        [
            new Account()
            {
                Id = Guid.NewGuid(),
                Currency = Currency.Rub,
                LeadId = lead.Id,
                Status = AccountStatus.Active

            },
            new Account()
            {
                Id = Guid.NewGuid(),
                Currency = Currency.Usd,
                LeadId = lead.Id,
                Status = AccountStatus.Active
            },
            new Account()
            {
                Id = Guid.NewGuid(),
                Currency = Currency.Eur,
                LeadId = lead.Id,
                Status = AccountStatus.Active
            }
        ];

        return accountsForLead;
    }
    
    private static List<Account> GeneratedAccountsForVipLead(Lead lead)
    {
        List<Account> accountsForVipLead =
        [
            new Account()
            {
                Id = Guid.NewGuid(),
                Currency = Currency.Jpy,
                LeadId = lead.Id,
                Status = AccountStatus.Active

            },
            new Account()
            {
                Id = Guid.NewGuid(),
                Currency = Currency.Cny,
                LeadId = lead.Id,
                Status = AccountStatus.Active
            },
            new Account()
            {
                Id = Guid.NewGuid(),
                Currency = Currency.Rsd,
                LeadId = lead.Id,
                Status = AccountStatus.Active
            },
            new Account()
            {
                Id = Guid.NewGuid(),
                Currency = Currency.Bgn,
                LeadId = lead.Id,
                Status = AccountStatus.Active
            },
            new Account()
            {
                Id = Guid.NewGuid(),
                Currency = Currency.Ars,
                LeadId = lead.Id,
                Status = AccountStatus.Active
            }
        ];

        return accountsForVipLead;
    }
}
