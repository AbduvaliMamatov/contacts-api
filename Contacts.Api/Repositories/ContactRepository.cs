using Contacts.Api.Entities;
using Contacts.Api.Exceptions;
using Contacts.Api.Repositories.Abstractions;

namespace Contacts.Api.Repositories;

public class ContactRepository : IContactRepository
{
    private Dictionary<string, Contact> contacts = [];
    int index = 1;

    public async ValueTask DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var contact = await GetSingleAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Contact with id {id} not found.");

        contacts.Remove(contact.PhoneNumber!);
    }

    public ValueTask<bool> ExistsAsync(string phoneNumber, CancellationToken cancellationToken = default)
        => ValueTask.FromResult(contacts.ContainsKey(phoneNumber));

    public ValueTask<IEnumerable<Contact>> GetAllAsync(CancellationToken cancellationToken = default)
        => ValueTask.FromResult(contacts.Values.AsEnumerable());

    public async ValueTask<Contact> GetSingleAsync(int id, CancellationToken cancellationToken = default)
        => await GetSingleOrDefaultAsync(id, cancellationToken)
            ?? throw new CustomNotFoundException($"Contact with id {id} not found.");

    public ValueTask<Contact?> GetSingleOrDefaultAsync(int id, CancellationToken cancellationToken = default)
        => ValueTask.FromResult(contacts.Values.FirstOrDefault(c => c.Id == id));

    public ValueTask<Contact> InsertAsync(Contact contact, CancellationToken cancellationToken = default)
    {
        if (contacts.TryAdd(contact.PhoneNumber!, contact))
        {
            contact.Id = index++;
            return ValueTask.FromResult(contact);
        }
        else
            throw new CustomConflictException($"Title musb be unique!");
    }

    public async ValueTask<Contact> UpdateAsync(int id, Contact contact, CancellationToken cancellationToken = default)
    {
        var found = await GetSingleAsync(id, cancellationToken);
        contact.Id = found.Id;

        if (found.PhoneNumber != contact.PhoneNumber)
        {
            if (contacts.TryAdd(contact.PhoneNumber!, contact))
                contacts.Remove(found.PhoneNumber!);
            else
                throw new CustomConflictException($"Phone number must be unique!");
        }
        else
            contacts[contact.PhoneNumber!] = contact;

        return contact;
    }

    public async ValueTask<Contact> PatchAsync(int id, Contact contact, CancellationToken cancellationToken = default)
    {
        var found = await GetSingleAsync(id, cancellationToken);
        
        var patchedContact = new Contact
        {
            Id = found.Id,
            FirstName = contact.FirstName ?? found.FirstName,
            LastName = contact.LastName ?? found.LastName,
            Email = contact.Email ?? found.Email,
            PhoneNumber = contact.PhoneNumber ?? found.PhoneNumber,
            Address = contact.Address ?? found.Address,
            CreatedAt = found.CreatedAt,
            UpdatedAt = DateTimeOffset.Now
        };

        if (found.PhoneNumber != patchedContact.PhoneNumber)
        {
            if (contacts.TryAdd(patchedContact.PhoneNumber!, patchedContact))
                contacts.Remove(found.PhoneNumber!);
            else
                throw new CustomConflictException($"Phone number must be unique!");
        }
        else
            contacts[patchedContact.PhoneNumber!] = patchedContact;

        return patchedContact;
    }
}
