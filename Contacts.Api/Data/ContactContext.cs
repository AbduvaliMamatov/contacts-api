using Microsoft.EntityFrameworkCore;

namespace Contacts.Api.Data;
using Contacts.Api.Entities;

public class ContactContext(DbContextOptions<ContactContext> options) 
: DbContext(options), IContactContext
{
    public DbSet<Contact> Contacts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContactContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyTimeStampChanges();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyTimeStampChanges()
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if(entry is { State: EntityState.Added, Entity: IHasTimeStamp added })
            {
                added.CreatedAt = DateTimeOffset.UtcNow;
                added.UpdatedAt = DateTimeOffset.UtcNow;
            }
            else if (entry is { State: EntityState.Modified, Entity: IHasTimeStamp updated })
                updated.UpdatedAt = DateTimeOffset.UtcNow;
        }
    }
}