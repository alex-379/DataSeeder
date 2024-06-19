using DataSeeder.Core;
using DataSeeder.Core.Database;
using DataSeeder.Core.Enums;
using DataSeeder.Core.Exceptions;
using DataSeeder.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DataSeeder.DataUpdater;

public static class Program
{
    private const int totalLeads = 4000000;
    private const int pageSize = 10000;
    private const int totalPages = totalLeads/pageSize;
    private static readonly Currency[] _allowedCurrenciesForRegularLead = [Currency.Rub, Currency.Usd, Currency.Eur];
    private static readonly Currency[] _allowedCurrenciesForVipLead = ((Currency[])Enum.GetValues(typeof(Currency)))
        .Where(c => c != Currency.Unknown)
        .ToArray();

    public static async Task Main()
    {
        var accounts = new List<Account>();
        await using var context = new CrmContext();
        for (var pageIndex = 0; pageIndex < totalPages; pageIndex++)
        {
            await CrmContext.ExecuteWithRetryAsync(async (cancellationToken) =>
            {
                var leads = await context.Leads
                    .Include(l => l.Accounts)
                    .Skip(pageSize * pageIndex)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken: cancellationToken);

                if (!ValidationAccounts.CheckAccounts(leads))
                {
                    foreach (var lead in leads)
                    {
                        if (lead.Accounts.Count == 0)
                        {
                            var account = new Account
                            {
                                Id = Guid.NewGuid(),
                                Currency = Currency.Rub,
                                LeadId = lead.Id,
                                Status = AccountStatus.Active
                            };
                            context.Accounts.Add(account);
                            await context.SaveChangesAsync(cancellationToken);
                        }

                        lead.Accounts = lead.Status != LeadStatus.Vip
                            ? DataUpdater.UpdateAccountsForLead(lead, _allowedCurrenciesForRegularLead)
                            : DataUpdater.UpdateAccountsForLead(lead, _allowedCurrenciesForVipLead);
                        accounts.AddRange(lead.Accounts);
                    }
                }

                if (!ValidationAccounts.CheckAccounts(leads))
                {
                    throw new ValidationException();
                }

                context.Accounts.UpdateRange(accounts);
                await context.SaveChangesAsync(cancellationToken);

                accounts.Clear();
                Console.WriteLine($"Iteration {pageIndex+1} on {pageSize} leads");
            });
        }
    }
}
