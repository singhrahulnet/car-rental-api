using VehicleRentalApi.DataModel;

namespace VehicleRentalApi.Contracts
{
	public class VehicleReservationRequest
	{
		public DateTime PickupOn { get; set; }
		public DateTime ReturnOn { get; set; }		
		public VehicleType VehicleType { get; set; }
	}
}
