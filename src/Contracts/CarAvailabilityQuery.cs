using CarRentalApi.DataModel;

namespace CarRentalApi.Contracts
{	
	public class CarAvailabilityQuery
	{
		public DateTime PickupOn { get; set; }
		public DateTime ReturnOn { get; set; }		
		public VehicleType[]? VehicleTypes { get; set; }
	}
}
