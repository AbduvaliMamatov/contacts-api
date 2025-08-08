namespace Contacts.Api.Entities;

public interface IHasTimeStamp
{
    DateTimeOffset CreatedAt { get; set; }
    DateTimeOffset UpdatedAt { get; set; }
}