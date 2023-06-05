using Application.Exceptions.Messages;
using Application.Exceptions;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;

namespace Application.Services;
public class StatusService : IStatusService
{
    private readonly IStatusRepository _statusRepository;

    public StatusService(IStatusRepository statusRepository)
    {
        _statusRepository = statusRepository;
    }

    public async Task<Status> GetStatus(int id)
    {
        var status = await _statusRepository.GetStatusById(id);
        if (status == null)
        {
            throw new NotFoundException(String.Format(StatusExceptionsMessages.StatusNotFound, id));
        }
        return status;
    }

    public async Task<IEnumerable<Status>> GetStatuses()
    {
        var statuses = await _statusRepository.GetStatuses();
        return statuses;
    }
}
