namespace CarRentalApi.DataModel
{
	internal class VehicleReservation
	{
		int Id { get; set; }
		int VehicleId { get; set; }
		DateTime PickupOn { get; set; }
		DateTime ReturnOn { get; }
	}
}
