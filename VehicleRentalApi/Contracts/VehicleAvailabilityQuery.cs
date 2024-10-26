using VehicleRentalApi.DataModel;

namespace VehicleRentalApi.Contracts
{	
	public class VehicleAvailabilityQuery
	{
		public DateTime PickupOn { get; set; }
		public DateTime ReturnOn { get; set; }		
		public VehicleType[] VehicleTypes { get; set; }
	}
}
