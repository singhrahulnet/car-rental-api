using VehicleRentalApi.DataModel;

namespace VehicleRentalApi.DataAccess
{
	internal interface IVehicleRepository
	{
		Task<IEnumerable<Vehicle>> GetAvailableVehiclesAsync();
		Task AddVehicleToInventoryAsync(Vehicle vehicle);
		Task<IEnumerable<VehicleReservation>> GetVehiclesReservationsAsync();
		Task AddVehicleReservationAsync(VehicleReservation reservation);
		Task ExecuteInTransactionAsync(Func<Task> operation);
	}
}