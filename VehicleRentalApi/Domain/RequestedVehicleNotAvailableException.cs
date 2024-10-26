namespace VehicleRentalApi.Domain
{
	[Serializable]
	internal class RequestedVehicleNotAvailableException : Exception
	{
		public RequestedVehicleNotAvailableException() { }
		public RequestedVehicleNotAvailableException(string message) : base(message) { }
		public RequestedVehicleNotAvailableException(string message, Exception innerException) : base(message, innerException) { }
	}
}