namespace Application.Exceptions.Messages;
public class CategoryExceptionsMessages
{
    public const string CategoryNotFound = "Category with id={0} is not found";
    public const string CategoryNameExists = "Category with name=\"{0}\" already exists";
    public const string CategoryHasChildren = "Can't remove category with id={0}. This category has child categories";
    public const string CategoryHasLinksToSections = "Can't remove category with id={0}. This category has links with sections";
    public const string CategoryIsUsed = "Can't remove category with id={0}. This category is used by products";
}
