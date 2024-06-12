using DataSeeder.Models;
using Bogus;
using DataSeeder.Enums;

namespace DataSeeder;

public class DataGenerator
{
    public static readonly List<Lead> Leads = [];
    public static readonly List<Account> Accounts = [];
    private static readonly Random random = new();
    private const int numberOfLeads = 4000000;
    private const int percentRegularLeads = 80;
    private const int percentVipLeads = 20;
    private const int percentAll = 100;
    private const string passwordLead = "Password_ENVIRONMENT";
    private const string secret = "SecretPassword_ENVIRONMENT";
    private const string provider = "crm.ru";

    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    private static Faker<Lead> GetLeadGenerator(LeadStatus status)
    {
        var (hash, salt) = PasswordsService.HashPassword(Environment.GetEnvironmentVariable(passwordLead), Environment.GetEnvironmentVariable(secret));
        var counter = 0;
        
        return new Faker<Lead>()
            .RuleFor(e => e.Id, _ => Guid.NewGuid())
            .RuleFor(e => e.Name, f => f.Name.FirstName())
            .RuleFor(e => e.Mail, (f, e) => f.Internet.Email(e.Name, RandomString(4), provider,(counter++).ToString()).ToLower())
            .RuleFor(e => e.Phone, f => f.Phone.PhoneNumberFormat())
            .RuleFor(e => e.Address, f => f.Address.StreetAddress())
            .RuleFor(e => e.BirthDate, f => f.Date.BetweenDateOnly(new DateOnly(1950, 1, 1), new DateOnly(2005, 1, 1)))
            .RuleFor(e => e.Status, _ => status)
            .RuleFor(e => e.Password, _ => hash)
            .RuleFor(e => e.Salt, _ => salt);
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
            Accounts.AddRange(lead.Accounts);
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
