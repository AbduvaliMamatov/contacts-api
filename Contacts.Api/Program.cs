using System.Text.Json.Serialization;
using FluentValidation;
using Contacts.Api.Validators;
using Contacts.Api;
using Contacts.Api.Services.Abstractions;
using Contacts.Api.Services;
using Contacts.Api.Dtos;
using Contacts.Api.Middlewares;
using Contacts.Api.Repositories.Abstractions;
using Contacts.Api.Repositories;
using Contacts.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
            options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            options.SerializerSettings.DateFormatString = "yyyy-MM-dd / HH:mm:ss";
    })
    .AddFluentValidationAsyncAutoValidation()
    .AddJsonOptions(jsonOptions =>
    {
        jsonOptions.JsonSerializerOptions.AllowTrailingCommas = true;
        jsonOptions.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IValidator<CreateContactDto>, CreateContactValidator>();
builder.Services.AddScoped<IValidator<UpdateContactDto>, UpdateContactValidator>();
builder.Services.AddScoped<IValidator<PatchContactDto>, PatchContactValidator>();

builder.Services.AddDbContext<IContactContext, ContactContext>(options => options
    .UseNpgsql(builder.Configuration.GetConnectionString("Contact"))
    .UseSnakeCaseNamingConvention());

builder.Services.AddHostedService<AutoApplyMigrationsService>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();

app.Run();

