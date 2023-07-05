namespace Application.Interfaces;

public interface IReservationService
{
    public Task<IEnumerable<int>> DeleteExpiredReservations();
}
