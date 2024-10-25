using VehicleRentalApi.DataModel;

namespace VehicleRentalApi.Contracts
{
	public class VehicleAvailabilityResponse
	{
		public VehicleType VehicleType { get; set; }
		public int NumbersAvailable { get; set; }
	}
}
