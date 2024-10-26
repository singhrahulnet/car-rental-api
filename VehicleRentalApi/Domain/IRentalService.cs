using VehicleRentalApi.Contracts;

namespace VehicleRentalApi.Domain
{
	public interface IRentalService
	{
		Task<IEnumerable<VehicleAvailabilityResponse>> GetAvailableVehiclesAsync(VehicleAvailabilityQuery query);
		Task ReserveVehicleAsync(VehicleReservationRequest reservation);
	}
}
