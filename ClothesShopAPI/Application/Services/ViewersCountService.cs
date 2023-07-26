using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;
public class ViewersCountService : IViewersCountService
{
    Dictionary<int, List<int>> currentViewers = new Dictionary<int, List<int>>();

    public async Task AddWatchingUser(int userId, int productId)
    {
        var viewers = currentViewers.GetValueOrDefault(productId);
        if (viewers == null)
        {
            viewers = new List<int>();
            currentViewers.Add(productId, viewers);
        }

        if (!viewers.Contains(userId))
        {
            viewers.Add(userId);
        }
    }

    public async Task<int> GetViewersCount(int productId)
    {
        var viewers = currentViewers.GetValueOrDefault(productId);
        if (viewers == null)
        {
            viewers = new List<int>();
            currentViewers.Add(productId, viewers);
        }

        return viewers.Count;
    }

    public async Task RemoveWatchingUser(int userId, int productId)
    {
        var viewers = currentViewers.GetValueOrDefault(productId);
        if (viewers == null)
        {
            viewers = new List<int>();
            currentViewers.Add(productId, viewers);
        }

        if (viewers.Contains(userId))
        {
            viewers.Remove(userId);
        }
    }
}
