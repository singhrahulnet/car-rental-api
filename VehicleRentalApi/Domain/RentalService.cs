using VehicleRentalApi.Contracts;

namespace VehicleRentalApi.Domain
{
	internal class RentalService : IRentalService
	{
		public Task<IEnumerable<VehicleAvailabilityResponse>> GetVehiclesAsync(VehicleAvailabilityQuery query)
		{
			throw new NotImplementedException();
		}

		public Task ReserveVehicleAsync(VehicleReservationRequest reservation)
		{
			throw new NotImplementedException();
		}
	}
}
