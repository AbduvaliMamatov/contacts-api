using Microsoft.AspNetCore.Mvc;
using Contacts.Api.Services;
using Contacts.Api.Dtos;
using AutoMapper;
using Contacts.Api.Models;
using Contacts.Api.Services.Abstractions;
using Microsoft.AspNetCore.JsonPatch;

namespace Contacts.Api.Controllers;

[ApiController, Route("api/[controller]")]
public class ContactsController(
    IContactService service,
    IMapper mapper) : Controller
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateContactDto dto,
        CancellationToken cancellationToken = default
        )
    {
        var model = mapper.Map<CreateContact>(dto);
        var created = await service.CreateContactAsync(model, cancellationToken);
        return Ok(mapper.Map<ContactDto>(created));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var contacts = await service.GetAllAsync(cancellationToken);
        return Ok(mapper.Map<IEnumerable<ContactDto>>(contacts));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken = default)
    {
        var contact = await service.GetSingleAsync(id, cancellationToken);
        return Ok(mapper.Map<ContactDto>(contact));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateContactDto dto,
        CancellationToken cancellationToken = default)
    {
        var model = mapper.Map<UpdateContact>(dto);
        var updated = await service.UpdateContactAsync(id, model, cancellationToken);
        return Ok(mapper.Map<ContactDto>(updated));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
    {
        await service.DeleteAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(
        int id,
        [FromBody] JsonPatchDocument<PatchContactDto> dto,
        CancellationToken cancellationToken = default)
    {
        var contact = await service.GetSingleAsync(id, cancellationToken);
        var patchedDto = mapper.Map<PatchContactDto>(contact);

        dto.ApplyTo(patchedDto);   
        var patchedContact = mapper.Map<PatchContact>(patchedDto);
        var updated = await service.PatchAsync(id, patchedContact, cancellationToken);

        return Ok(mapper.Map<ContactDto>(updated));
    }
}