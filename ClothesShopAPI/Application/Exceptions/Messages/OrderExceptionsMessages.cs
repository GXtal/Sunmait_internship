namespace Application.Exceptions.Messages;

public class OrderExceptionsMessages
{
    public const string OrderNotFound = "Order with id={0} is not found";
    public const string OrderUnchangeable = "Cant add products to order with id={0}, because its status does not allow it";
}
