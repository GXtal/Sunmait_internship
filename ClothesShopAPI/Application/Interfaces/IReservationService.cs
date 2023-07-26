using Domain.Entities;

namespace Application.Interfaces;

public interface IReservationService
{
    public Task<IEnumerable<Product>> DeleteExpiredReservations();
}
