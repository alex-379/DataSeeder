using DataSeeder.Core.Enums;
using DataSeeder.Core.Models;

namespace DataSeeder.DataUpdater;

public static class DataUpdater
{
    public static List<Account> GeneratedAccountsForLead(Lead lead, Currency[] allowedCurrencies)
    {
        var rnd = new Random();
        var accounts = lead.Accounts
                       ?? [new Account
                       {
                           Id = Guid.NewGuid(),
                           Currency = Currency.Rub,
                           LeadId = lead.Id,
                           Status = AccountStatus.Active
                       }];
        var usedCurrencies = new HashSet<Currency>
        {
            Currency.Rub
        };

        accounts[0].Currency = Currency.Rub;

        for (var i = 1; i < accounts.Count; i++)
        {
            Currency currency;
            if (usedCurrencies.Count < allowedCurrencies.Length)
            {
                do
                {
                    currency = allowedCurrencies[rnd.Next(0, allowedCurrencies.Length)];
                }
                while (usedCurrencies.Contains(currency));

                usedCurrencies.Add(currency);
                accounts[i].Currency = currency;
            }
            else
            {
                currency = allowedCurrencies.Except(usedCurrencies).First();
                usedCurrencies.Add(currency);
                accounts[i].Currency = currency;
            }
        }

        return accounts;
    }
}
