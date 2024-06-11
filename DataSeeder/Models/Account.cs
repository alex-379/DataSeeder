using System.Text.Json;
using DataSeeder.Enums;

namespace DataSeeder.Models;

public class Account
{
    public Guid Id { get; init; }
    public Currency Currency { get; init; }
    public AccountStatus Status { get; init; }
    public Guid LeadId { get; init; }
    
    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
    }
}