﻿namespace Domain.Entities;

public class Address
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string FullAddress { get; set; }

    public User User { get; set; }
}
