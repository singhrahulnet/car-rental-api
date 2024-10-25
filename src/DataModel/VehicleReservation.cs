namespace VehicleRentalApi.DataModel
{
	internal class VehicleReservation
	{
		public int Id { get; set; }
		public int VehicleId { get; set; }
		public DateTime PickupOn { get; set; }
		public DateTime ReturnOn { get; }
	}
}
