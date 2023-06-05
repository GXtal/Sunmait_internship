﻿using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IUserRepository
{
    public Task<User> GetUserById(int userId);
}
