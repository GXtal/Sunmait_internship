namespace Application.Exceptions.Messages;

public class BrandExceptionsMessages
{
    public const string BrandNotFound = "Brand with id={0} is not found";
    public const string BrandNameExists = "Brand with name=\"{0}\" already exists";
    public const string BrandIsUsed = "Can't remove brand with id={0}. This brand is used by products";
}
