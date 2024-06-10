using DataSeeder.Enums;

namespace DataSeeder.Models;

public class Lead
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Mail { get; init; }
    public string Phone { get; init; }
    public string Address { get; init; }
    public DateOnly BirthDate { get; init; }
    public LeadStatus Status { get; init; }
    public string Password { get; init; }
    public string Salt { get; init; }
    public string RefreshToken { get; init; }
    public DateTime RefreshTokenExpiryTime { get; init; }
    public bool IsDeleted { get; init; }
    public List<Account> Accounts { get; init; }
}