using Microsoft.EntityFrameworkCore;

namespace Contacts.Api.Data;
using Contacts.Api.Entities;

public class ContactContext(DbContextOptions<ContactContext> options) : DbContext(options)
{
    public DbSet<Contact> Contacts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContactContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}