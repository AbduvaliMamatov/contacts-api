namespace Contacts.Api.Exceptions;

public class CustomBadRequestException(string errorMessage)
: Exception(errorMessage) { } 