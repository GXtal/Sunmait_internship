namespace Domain.Entities;

public class User
{
    public int Id { get; set; }

    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public int RoleId { get; set; }

    public string Name { get; set; }

    public string Surname { get; set; }

    public ICollection<Address> Addresses { get; set; }

    public ICollection<Contact> Contacts { get; set; }

    public ICollection<Order> Orders { get; set; }

    public ICollection<Review> Reviews { get; set; }

    public ICollection<ReservedProduct> ReservedProducts { get; set; }

    public Role Role { get; set; }
}
