namespace Contacts.Api.Exceptions;

public class CustomConflictException(string errorMessage)
: Exception(errorMessage) { } 