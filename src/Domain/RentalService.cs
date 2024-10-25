using CarRentalApi.Contracts;

namespace CarRentalApi.Domain
{
	internal class RentalService : IRentalService
	{
		public Task<IEnumerable<CarAvailabilityResponse>> GetCarsAsync(CarAvailabilityQuery query)
		{
			throw new NotImplementedException();
		}

		public Task ReserveCarAsync(CarReservation reservation)
		{
			throw new NotImplementedException();
		}
	}
}
