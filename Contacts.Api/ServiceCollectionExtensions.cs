using Contacts.Api.Filters;

namespace Contacts.Api;

public static class ServiceCollectionExtensions
{
    public static IMvcBuilder AddFluentValidationAsyncAutoValidation(this IMvcBuilder builder)
    {
        return builder.AddMvcOptions(o =>
        {
            o.Filters.Add<AsyncAutoValidation>(AsyncAutoValidation.OrderLowerThanModelStateInvalidFilter);
        });
    }
}