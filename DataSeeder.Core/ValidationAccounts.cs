using DataSeeder.Core.Enums;
using DataSeeder.Core.Models;

namespace DataSeeder.Core;

public static class ValidationAccounts
{
    private const int numberAccountsRegular = 3;
    private const int numberAccountsVip = 8;

    public static bool CheckAccounts(List<Lead> leads)
    {
        if (CheckCountLeads(leads) && CheckDefaultRubAccount(leads) && CheckLeadsOnDuplicateCurrencies(leads))
        {
            return true;
        }

        return false;
    }

    private static bool CheckCountLeads(List<Lead> leads)
    {
        var leadsWithInvalidAccountsCount = leads.Where(l =>
        {
            int expectedAccountsCount = l.Status == LeadStatus.Vip ? numberAccountsVip : numberAccountsRegular;
            return l.Accounts.Count > expectedAccountsCount;
        }).ToList();

        if (leadsWithInvalidAccountsCount.Count != 0)
        {
            Console.WriteLine($"Leads with an unacceptable number of {leadsWithInvalidAccountsCount.Count} accounts were found:");
            foreach (var lead in leadsWithInvalidAccountsCount)
            {
                Console.WriteLine($"Lead ID: {lead.Id}, Accounts Count: {lead.Accounts.Count}");
            }

            return false;
        }

        return true;
    }

    private static bool CheckDefaultRubAccount(List<Lead> leads)
    {
        var leadsWithoutRubAccount = leads
            .Where(l => l.Accounts.All(a => a.Currency != Currency.Rub)).ToList();
        if (leadsWithoutRubAccount.Count != 0)
        {
            Console.WriteLine($"Leads without a ruble account of {leadsWithoutRubAccount.Count} pieces were found:");
            foreach (var lead in leadsWithoutRubAccount)
            {
                Console.WriteLine($"Lead ID: {lead.Id}");
            }

            return false;
        }

        return true;
    }

    private static bool CheckLeadsOnDuplicateCurrencies(List<Lead> leads)
    {
        var leadsWithDuplicateCurrencies = new List<Lead>();
        foreach (var lead in leads)
        {
            var remainingCurrencies = new List<Currency>();
            var hasDuplicateCurrencies = false;

            foreach (var account in lead.Accounts)
            {
                var currency = account.Currency;
                if (remainingCurrencies.Contains(currency))
                {
                    hasDuplicateCurrencies = true;
                    break;
                }
                else
                {
                    remainingCurrencies.Add(currency);
                }
            }

            if (hasDuplicateCurrencies)
            {
                leadsWithDuplicateCurrencies.Add(lead);
            }
        }

        if (leadsWithDuplicateCurrencies.Count != 0)
        {
            Console.WriteLine($"Leads with duplicate account currencies of {leadsWithDuplicateCurrencies.Count} pcs were found:");
            foreach (var lead in leadsWithDuplicateCurrencies)
            {
                Console.WriteLine($"Lead ID: {lead.Id}");
            }

            return false;
        }

        return true;
    }

}