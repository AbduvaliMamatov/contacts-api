using Contacts.Api.Dtos;
using FluentValidation;

namespace Contacts.Api.Validators;

public class PatchContactValidator : AbstractValidator<PatchContactDto>
{
    public PatchContactValidator()
    {
        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(100)
            .WithMessage("FirstName is required and must be at least 2 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(100)
            .WithMessage("LastName is required and must be at least 2 characters.");

        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => x.Email is { Length: > 0 })
            .WithMessage("Email address is invalid.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Length(13)
            .Must(phone => phone is { Length: > 0 } && 
                        phone.StartsWith("+998") && 
                        phone[0] == '+' && 
                        phone.Skip(1).All(char.IsDigit))
            .WithMessage("PhoneNumber must start with '+998' and contain only digits after '+'.");
    }   
}