namespace Application.Exceptions;

public class ExistingBrandNameException : Exception
{
    public ExistingBrandNameException()
        : base()
    {
    }

    public ExistingBrandNameException(string message)
        : base(message)
    {
    }
}
