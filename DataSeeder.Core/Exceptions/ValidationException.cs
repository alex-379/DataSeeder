namespace DataSeeder.Core.Exceptions;

public class ValidationException(string message= "Validation failed") : Exception(message)
{
}
