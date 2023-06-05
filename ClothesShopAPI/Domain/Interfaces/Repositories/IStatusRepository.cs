using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IStatusRepository
{
    public Task<Status> GetStatusById(int id);

    public Task<IEnumerable<Status>> GetStatuses();

    public Task Save();
}
