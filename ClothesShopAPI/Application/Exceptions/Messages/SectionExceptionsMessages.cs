namespace Application.Exceptions.Messages;

public class SectionExceptionsMessages
{
    public const string SectionNotFound = "Section with id={0} is not found";
    public const string SectionNameExists = "Section with name=\"{0}\" already exists";
    public const string SectionIsUsed = "Can't remove section with id={0}. This section has linked categories";
}
