using System.Security.Cryptography;
using DataSeeder.Models;
using Bogus;
using DataSeeder.Enums;

namespace DataSeeder;

public class DataGenerator
{
    private static readonly Random _random = new();
    public static readonly List<Lead> Leads = [];
    private static readonly Currency[] _allowedCurrenciesForRegularLead = new[] { Currency.Rub, Currency.Usd, Currency.Eur };
    private static readonly Currency[] _allowedCurrenciesForVipLead = ((Currency[])Enum.GetValues(typeof(Currency)))
        .Where(c => c != Currency.Unknown)
        .ToArray();

    private const int numberOfLeads = 400;
    private const int percentRegularLeads = 80;
    private const int percentVipLeads = 20;
    private const int percentAll = 100;
    private const int keySize = 64;
    private const string passwordLead = "Password_ENVIRONMENT";
    private const string secret = "SecretPassword_ENVIRONMENT";
    private const string provider = "crm.ru";
    private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    private static string GenerateRandomString(int length) => new string(Enumerable.Repeat(chars, length)
            .Select(s => s[_random.Next(s.Length)]).ToArray());

    private static string GenerateRandomHash() => Convert.ToHexString(RandomNumberGenerator.GetBytes(keySize));
    
    private static Faker<Lead> GetLeadGenerator(LeadStatus status)
    {
        var counter = 0;
        var randomHash = GenerateRandomHash();
        var randomSalt = GenerateRandomHash();
        var lastNameMail = GenerateRandomString(2);
        
        return new Faker<Lead>()
            .RuleFor(e => e.Id, _ => Guid.NewGuid())
            .RuleFor(e => e.Name, f => f.Name.FirstName())
            .RuleFor(e => e.Mail, (f, e) => f.Internet.Email(e.Name, lastNameMail, provider,(counter++).ToString()).ToLower())
            .RuleFor(e => e.Phone, f => f.Phone.PhoneNumberFormat())
            .RuleFor(e => e.Address, f => f.Address.StreetAddress())
            .RuleFor(e => e.BirthDate, f => f.Date.BetweenDateOnly(new DateOnly(1950, 1, 1), new DateOnly(2005, 1, 1)))
            .RuleFor(e => e.Status, _ => status)
            .RuleFor(e => e.Password, _ => randomHash)
            .RuleFor(e => e.Salt, _ => randomSalt);
    }
    
    public static void InitBogusData()
    {
        GeneratedLeads(numberOfLeads * percentRegularLeads/percentAll);
        GeneratedLeads(numberOfLeads * percentVipLeads/percentAll, LeadStatus.Vip);

        foreach (var lead in Leads)
        {
            lead.Accounts = lead.Status != LeadStatus.Vip
                ? GeneratedAccountsForLead(lead, _allowedCurrenciesForRegularLead)
                : GeneratedAccountsForLead(lead, _allowedCurrenciesForVipLead, 1,8);
        }
    }

    private static void GeneratedLeads(int numberOfLeadsWithStatus, LeadStatus status = LeadStatus.Regular)
    {
        var leadGenerator = GetLeadGenerator(status);
        var generatedLeads = leadGenerator.Generate(numberOfLeadsWithStatus);
        Leads.AddRange(generatedLeads);
    }

    private static List<Account> GeneratedAccountsForLead(Lead lead, Currency[] allowedCurrencies, int minAccounts = 1, int maxAccounts = 3)
    {
        var rnd = new Random();
        var numAccounts = rnd.Next(minAccounts - 1, maxAccounts);
        var accountsForLead = new List<Account>();
        var usedCurrencies = new HashSet<Currency>();

        var account = new Account
        {
            Id = Guid.NewGuid(),
            Currency = Currency.Rub,
            LeadId = lead.Id,
            Status = AccountStatus.Active
        };
        accountsForLead.Add(account);
        usedCurrencies.Add(Currency.Rub);

        for (var i = 0; i < numAccounts - 1; i++)
        {
            Currency currency;
            if (usedCurrencies.Count < allowedCurrencies.Length)
            {
                do
                {
                    currency = allowedCurrencies[rnd.Next(0, allowedCurrencies.Length)];
                } while (usedCurrencies.Contains(currency));
                usedCurrencies.Add(currency);
            }
            else
            {
                currency = (Currency)usedCurrencies.ToArray()[rnd.Next(1, usedCurrencies.Count)];
            }

            account = new Account
            {
                Id = Guid.NewGuid(),
                Currency = currency,
                LeadId = lead.Id,
                Status = AccountStatus.Active
            };
            accountsForLead.Add(account);
        }

        return accountsForLead;
    }

}
