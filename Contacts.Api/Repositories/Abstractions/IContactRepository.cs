namespace Contacts.Api.Repositories.Abstractions;

using Contacts.Api.Entities;
public interface IContactRepository
{
    ValueTask<Contact> InsertAsync(Contact contact, CancellationToken cancellationToken = default);

    ValueTask<IEnumerable<Contact>> GetAllAsync(CancellationToken cancellationToken = default);

    ValueTask<Contact?> GetSingleOrDefaultAsync(int id, CancellationToken cancellationToken = default);

    ValueTask<Contact> GetSingleAsync(int id, CancellationToken cancellationToken = default);

    ValueTask DeleteAsync(int id, CancellationToken cancellationToken = default);

    ValueTask<Contact> UpdateAsync(Contact contact, CancellationToken cancellationToken = default);

    ValueTask<bool> ExistsAsync(string title, CancellationToken cancellationToken = default);

    ValueTask<Contact> PatchAsync(int id, Contact contact, CancellationToken cancellationToken = default);
}