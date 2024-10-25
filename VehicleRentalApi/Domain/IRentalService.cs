using VehicleRentalApi.Contracts;

namespace VehicleRentalApi.Domain
{
	public interface IRentalService
	{
		Task<IEnumerable<VehicleAvailabilityResponse>> GetVehiclesAsync(VehicleAvailabilityQuery query);
		Task ReserveVehicleAsync(VehicleReservationRequest reservation);
	}
}
