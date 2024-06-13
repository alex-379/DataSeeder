using DataSeeder.Enums;

namespace DataSeeder.Models;

public class Account
{
    public Guid Id { get; init; }
    public Currency Currency { get; init; }
    public AccountStatus Status { get; init; }
    public Guid LeadId { get; init; }
}