using VehicleRentalApi.DataModel;

namespace VehicleRentalApi.DataAccess
{
	internal interface IVehicleRepository
	{
		Task AddVehicleToInventoryAsync(Vehicle vehicle);
		Task<IEnumerable<Vehicle>> GetVehiclesAsync();
		Task AddVehicleReservationAsync(VehicleReservation reservation);
	}
}