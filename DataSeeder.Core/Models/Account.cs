using DataSeeder.Core.Enums;

namespace DataSeeder.Core.Models;

public class Account
{
    public Guid Id { get; init; }
    public Currency Currency { get; set; }
    public AccountStatus Status { get; init; }
    public Guid LeadId { get; init; }
}