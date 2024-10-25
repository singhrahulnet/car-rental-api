using CarRentalApi.Contracts;

namespace CarRentalApi.Domain
{
	public interface IRentalService
	{
		Task<IEnumerable<CarAvailabilityResponse>> GetCarsAsync(CarAvailabilityQuery query);
		Task ReserveCarAsync(CarReservation reservation);
	}
}
