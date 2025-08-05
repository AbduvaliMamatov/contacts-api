namespace Contacts.Api.Exceptions;

public class CustomNotFoundException(string errorMessage)
: Exception(errorMessage) { }