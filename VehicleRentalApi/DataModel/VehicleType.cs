using System.Text.Json.Serialization;

namespace VehicleRentalApi.DataModel
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum VehicleType
	{
		COMPACT,
		SEDAN,
		SUV,
		VAN
	}
}
