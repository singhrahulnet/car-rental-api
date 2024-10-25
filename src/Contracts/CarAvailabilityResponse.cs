using CarRentalApi.DataModel;

namespace CarRentalApi.Contracts
{
	public class CarAvailabilityResponse
	{
		public VehicleType VehicleType { get; set; }
		public int NumbersAvailable { get; set; }
	}
}
