﻿namespace Domain.Entities;

public class Contact
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string PhoneNumber { get; set; }

    public User User { get; set; }
}
