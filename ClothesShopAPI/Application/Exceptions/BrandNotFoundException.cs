namespace Application.Exceptions;

public class BrandNotFoundException : Exception
{
    public BrandNotFoundException()
        : base()
    {
    }

    public BrandNotFoundException(string message)
        : base(message)
    {
    }
}
