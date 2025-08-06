using Contacts.Api.Data;
using Contacts.Api.Entities;
using Contacts.Api.Exceptions;
using Contacts.Api.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Contacts.Api.Repositories;

public class ContactRepository(ContactContext context) : IContactRepository
{
    public async ValueTask DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var effectedRows = await context.Contacts
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
        if (effectedRows < 1)
            throw new CustomNotFoundException($"Contact with id {id} not found");
    }

    public async ValueTask<bool> ExistsAsync(string phoneNumber, CancellationToken cancellationToken = default)
        => await context.Contacts.AnyAsync(q => q.PhoneNumber == phoneNumber, cancellationToken);

    public async ValueTask<IEnumerable<Contact>> GetAllAsync(CancellationToken cancellationToken = default)
        => await context.Contacts.ToListAsync(cancellationToken);

    public async ValueTask<Contact> GetSingleAsync(int id, CancellationToken cancellationToken = default)
        => await GetSingleOrDefaultAsync(id, cancellationToken)
            ?? throw new CustomNotFoundException($"Contact with id {id} not found.");

    public async ValueTask<Contact?> GetSingleOrDefaultAsync(int id, CancellationToken cancellationToken = default)
        => await context.Contacts.FindAsync([id], cancellationToken);

    public async ValueTask<Contact> InsertAsync(Contact contact, CancellationToken cancellationToken = default)
    {
        try
        {
            contact.CreatedAt = DateTimeOffset.UtcNow;
            contact.UpdatedAt = DateTimeOffset.UtcNow;
            var entry = context.Contacts.Add(contact);
            await context.SaveChangesAsync(cancellationToken);
            return entry.Entity;
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException { SqlState: "23505" })
        {
            throw new CustomConflictException("PhoneNumber must be unique!");
        }
    }

    public async ValueTask<Contact> UpdateAsync(Contact contact, CancellationToken cancellationToken = default)
    {
        try
        {
            contact.UpdatedAt = DateTimeOffset.UtcNow;
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException { SqlState: "23505" })
        {
            throw new CustomConflictException("PhoneNumber must be unique!");
        }

        return contact;
    }

    public async ValueTask<Contact> PatchAsync(int id, Contact contact, CancellationToken cancellationToken = default)
    {
        var found = await GetSingleAsync(id, cancellationToken);
        if (found is null)
            throw new CustomNotFoundException($"Contact with ID {id} not found.");

        found.FirstName = contact.FirstName ?? found.FirstName;
        found.LastName = contact.LastName ?? found.LastName;
        found.Email = contact.Email ?? found.Email;
        found.PhoneNumber = contact.PhoneNumber ?? found.PhoneNumber;
        found.Address = contact.Address ?? found.Address;
        found.UpdatedAt = DateTimeOffset.UtcNow;

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException { SqlState: "23505" })
        {
            throw new CustomConflictException("PhoneNumber must be unique!");
        }

        return found;
    }
}
