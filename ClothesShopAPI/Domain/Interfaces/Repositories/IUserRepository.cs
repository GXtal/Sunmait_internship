﻿using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IUserRepository
{
    public Task<User> AddUser(User user);

    public Task<User> GetUserByEmail(string email);

    public Task<User> GetUserById(int id);

    public Task UpdateUser(User user);

    public Task Save();
}
