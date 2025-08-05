using AutoMapper;
using Contacts.Api.Models;
using Contacts.Api.Services.Abstractions;
using Contacts.Api.Dtos;
using Contacts.Api.Exceptions;
using Contacts.Api.Repositories.Abstractions;

namespace Contacts.Api.Services;

public class ContactService(IMapper mapper, IContactRepository repository) : IContactService
{
    public async ValueTask<Contact> CreateContactAsync(CreateContact contact, CancellationToken cancellationToken)
        => mapper.Map<Contact>(await repository.InsertAsync(mapper.Map<Entities.Contact>(contact), cancellationToken));

    public async ValueTask<IEnumerable<Contact>> GetAllAsync(CancellationToken cancellationToken)
        => (await repository.GetAllAsync(cancellationToken)).Select(mapper.Map<Contact>);

    public async ValueTask<Contact?> GetSingleOrDefaultAsync(int id, CancellationToken cancellationToken = default)
        => mapper.Map<Contact>(await repository.GetSingleOrDefaultAsync(id, cancellationToken));

    public async ValueTask<Contact> GetSingleAsync(int id, CancellationToken cancellationToken = default)
        => mapper.Map<Contact>(await repository.GetSingleAsync(id, cancellationToken));

    public ValueTask DeleteAsync(int id, CancellationToken cancellationToken = default)
        => repository.DeleteAsync(id, cancellationToken);

    public async ValueTask<Contact> UpdateContactAsync(int id, UpdateContact contact, CancellationToken cancellationToken = default)
        => mapper.Map<Contact>(await repository.UpdateAsync(id, mapper.Map<Entities.Contact>(contact), cancellationToken));

    public ValueTask<bool> ExistsAsync(string phoneNumber, CancellationToken cancellationToken = default)
        => repository.ExistsAsync(phoneNumber, cancellationToken);
    
    public async ValueTask<Contact> PatchAsync(int id, PatchContact contact, CancellationToken cancellationToken = default)
        => mapper.Map<Contact>(await repository.PatchAsync(id, mapper.Map<Entities.Contact>(contact), cancellationToken));
}