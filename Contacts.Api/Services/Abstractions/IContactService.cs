using Contacts.Api.Models;
namespace Contacts.Api.Services.Abstractions;

public interface IContactService
{
    ValueTask<Contact> CreateContactAsync(CreateContact contact, CancellationToken cancellationToken = default);
    ValueTask<IEnumerable<Contact>> GetAllAsync(CancellationToken cancellationToken = default);
    ValueTask<Contact?> GetSingleOrDefaultAsync(int id, CancellationToken cancellationToken = default);
    ValueTask<Contact> GetSingleAsync(int id, CancellationToken cancellationToken = default);
    ValueTask<Contact> UpdateContactAsync(int id, UpdateContact contact, CancellationToken cancellationToken = default);
    ValueTask<bool> ExistsAsync(string phoneNumber, CancellationToken cancellationToken = default);
    ValueTask DeleteAsync(int id, CancellationToken cancellationToken = default);
    ValueTask<Contact> PatchAsync(int id, PatchContact contact, CancellationToken cancellationToken = default);

}