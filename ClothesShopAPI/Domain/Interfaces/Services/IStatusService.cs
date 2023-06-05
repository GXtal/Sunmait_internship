using Domain.Entities;

namespace Domain.Interfaces.Services;

public interface IStatusService
{
    public Task<Status> GetStatus(int id);

    public Task<IEnumerable<Status>> GetStatuses();
}
