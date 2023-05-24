namespace Domain.Entities;

public class User
{
    public int Id { get; set; }

    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public int RoleId { get; set; }

    public string Name { get; set; }

    public string Surname { get; set; }

    public virtual ICollection<Address> Addresses { get; set; }

    public virtual ICollection<Contact> Contacts { get; set; }

    public virtual ICollection<Order> Orders { get; set; }

    public virtual ICollection<Review> Reviews { get; set; }

    public virtual Role Role { get; set; }
}
