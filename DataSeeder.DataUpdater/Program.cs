using DataSeeder.Core;
using DataSeeder.Core.Database;
using DataSeeder.Core.Enums;
using DataSeeder.Core.Exceptions;
using DataSeeder.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DataSeeder.DataUpdater;

public static class Program
{
    private static readonly Currency[] _allowedCurrenciesForRegularLead = [Currency.Rub, Currency.Usd, Currency.Eur];
    private static readonly Currency[] _allowedCurrenciesForVipLead = ((Currency[])Enum.GetValues(typeof(Currency)))
        .Where(c => c != Currency.Unknown)
        .ToArray();

    public static async Task Main()
    {
        var pageSize = 1000;
        var totalPages = 4000;
        var counter = 0;

        var accounts = new List<Account>();
        await using var context = new CrmContext();
        for (int pageIndex = 0; pageIndex < totalPages; pageIndex++)
        {
            var leads = await context.Leads
                .Include(l => l.Accounts)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            if (!ValidationAccounts.CheckAccounts(leads))
            {
                foreach (var lead in leads)
                {
                    lead.Accounts = lead.Status != LeadStatus.Vip
                        ? DataUpdater.GeneratedAccountsForLead(lead, _allowedCurrenciesForRegularLead)
                        : DataUpdater.GeneratedAccountsForLead(lead, _allowedCurrenciesForVipLead);
                    accounts.AddRange(lead.Accounts);
                }
            }

            if (!ValidationAccounts.CheckAccounts(leads))
            {
                throw new ValidationException();
            };
            context.Accounts.UpdateRange(accounts);
            await context.SaveChangesAsync();

            accounts.Clear();
            Console.WriteLine($"Iteration {counter} on {pageSize} leads");
            counter++;
        }
    }
}
