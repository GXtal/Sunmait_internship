namespace Application.Exceptions;

public class BrandInUseException : Exception
{
    public BrandInUseException()
        : base()
    {
    }

    public BrandInUseException(string message)
        : base(message)
    {
    }
}
