namespace Application.Exceptions.Messages;

public class CartExceptionMessages
{
    public const string ReservedProductNotFound = "User with id={0} doesn't have product with id={1} in his cart";
    public const string CartIsEmpty = "User with id={0} doesn't have any products in his cart";
}
