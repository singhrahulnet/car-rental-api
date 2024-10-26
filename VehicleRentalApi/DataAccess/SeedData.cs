using VehicleRentalApi.DataModel;

namespace VehicleRentalApi.DataAccess
{
	internal static class SeedData
	{
		public static Vehicle[] Generate()
		{
			return [
				new Vehicle { Id = 1, VehicleType = VehicleType.SEDAN },
				new Vehicle { Id = 2, VehicleType = VehicleType.COMPACT },
				new Vehicle { Id = 3, VehicleType = VehicleType.SUV },
				new Vehicle { Id = 4, VehicleType = VehicleType.SUV }
				];
		}
	}
}
