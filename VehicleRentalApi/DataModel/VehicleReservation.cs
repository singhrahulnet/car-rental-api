namespace VehicleRentalApi.DataModel
{
	internal class VehicleReservation
	{
		public int Id { get; set; }
		public DateTime PickupOn { get; set; }
		public DateTime ReturnOn { get; set; }
        public VehicleType VehicleType { get; set; }
    }
}
