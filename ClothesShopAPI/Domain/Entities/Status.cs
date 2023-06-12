﻿namespace Domain.Entities;
public class Status
{
    public int Id { get; set; }

    public string Name { get; set; }

    public ICollection<OrderHistory> OrderHistories { get; set; }

    public ICollection<Order> Orders { get; set; }
}
