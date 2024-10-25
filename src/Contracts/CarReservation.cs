using CarRentalApi.DataModel;

namespace CarRentalApi.Contracts
{
	public class CarReservation
	{
		public DateTime PickupOn { get; set; }
		public DateTime ReturnOn { get; set; }		
		public VehicleType VehicleType { get; set; }
	}
}
