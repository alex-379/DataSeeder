using DataSeeder.Enums;
using DataSeeder.Models;

namespace DataSeeder;

public static class ValidationAccounts
{
    public static void CheckAccounts(List<Lead> leads)
    {
        CheckCountLeads(leads);
        CheckDefaultRubAccount(leads);
        CheckLeadsOnDuplicateAccounts(leads);
    }
    
    private static void CheckCountLeads (List<Lead> leads)
    {
        var leadsWithInvalidAccountsCount = leads
            .Where(l => l.Accounts.Count is < 1 or > 8).ToList();
        if (leadsWithInvalidAccountsCount.Count != 0)
        {
            Console.WriteLine($"Обнаружены лиды с недопустимым количеством аккаунтов ({leadsWithInvalidAccountsCount.Count} шт):");
            foreach (var lead in leadsWithInvalidAccountsCount)
            {
                Console.WriteLine($"Lead ID: {lead.Id}, Accounts Count: {lead.Accounts.Count}");
            }
        }
        else
        {
            Console.WriteLine("Все лиды имеют допустимое количество аккаунтов (от 1 до 8).");
        }
    }

    private static void CheckDefaultRubAccount(List<Lead> leads)
    {
        var leadsWithoutRubAccount = leads
            .Where(l => l.Accounts.All(a => a.Currency != Currency.Rub)).ToList();
        if (leadsWithoutRubAccount.Count != 0)
        {
            Console.WriteLine($"Обнаружены лиды без рублевого аккаунта ({leadsWithoutRubAccount.Count} шт):");
            foreach (var lead in leadsWithoutRubAccount)
            {
                Console.WriteLine($"Lead ID: {lead.Id}");
            }
        }
        else
        {
            Console.WriteLine("Все лиды имеют рублевый аккаунт.");
        }
    }

    private static void CheckLeadsOnDuplicateAccounts(List<Lead> leads)
    {
        var leadsWithDuplicateAccounts = leads
            .Where(lead => lead.Accounts
                .GroupBy(a => a.Id)
                .Any(g => g.Count() > 1))
            .ToList();
        if (leadsWithDuplicateAccounts.Count != 0)
        {
            Console.WriteLine($"Обнаружены лиды с повторяющимися аккаунтами ({leadsWithDuplicateAccounts.Count} шт):");
            foreach (var lead in leadsWithDuplicateAccounts)
            {
                Console.WriteLine($"Lead ID: {lead.Id}");
            }
        }
        else
        {
            Console.WriteLine("Нет лидов с повторяющимися аккаунтами.");
        }
    }
}