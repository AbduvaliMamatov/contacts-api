using AutoMapper;
using Contacts.Api.Models;
using Contacts.Api.Dtos;

namespace Contacts.Api.Services;

public class ContactProfile : Profile
{
    public ContactProfile()
    {
        CreateMap<ContactDto, Contact>();
        CreateMap<CreateContactDto, CreateContact>();
        CreateMap<UpdateContactDto, UpdateContact>();
        CreateMap<PatchContactDto, PatchContact>();

        CreateMap<Contact, ContactDto>();
        CreateMap<Contact, PatchContactDto>();
        CreateMap<CreateContact, Contact>();
        CreateMap<UpdateContact, Contact>();
        CreateMap<PatchContact, Contact>();

        CreateMap<Entities.Contact, Contact>();
        CreateMap<CreateContact, Entities.Contact>();
        CreateMap<UpdateContact, Entities.Contact>();
        CreateMap<PatchContact, Entities.Contact>();
    }
}