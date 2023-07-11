using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces;
public interface IViewersCountService
{
    public Task<int> GetViewersCount(int productId);

    public Task AddWatchingUser(int userId, int productId);

    public Task RemoveWatchingUser(int userId, int productId);
}
