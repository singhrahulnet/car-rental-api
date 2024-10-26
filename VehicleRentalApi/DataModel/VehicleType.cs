using System.Text.Json.Serialization;

namespace VehicleRentalApi.DataModel
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum VehicleType
	{
		COMPACT = 1,
		SEDAN = 2,
		SUV = 3,
		VAN = 4
	}
}
