namespace Contacts.Api.Models;

public record CreateContact(
    string FirstName,
    string LastName,
    string? Email,
    string? PhoneNumber,
    string? Address);