using VehicleRentalApi.Contracts;
using VehicleRentalApi.DataAccess;
using VehicleRentalApi.DataModel;

namespace VehicleRentalApi.Domain
{
	internal class RentalService : IRentalService
	{
		private readonly IVehicleRepository _vehicleRepository;
		public RentalService(IVehicleRepository vehicleRepository)
		{
			_vehicleRepository = vehicleRepository;
		}

		public async Task<IEnumerable<VehicleAvailabilityResponse>> GetAvailableVehiclesAsync(VehicleAvailabilityQuery query)
		{
			var matchingVehicles = (await _vehicleRepository.GetAvailableVehiclesAsync())
				.Where(VehicleTypeMatchesQuery(query));

			var overlappingReservationsByVehicleType = (await _vehicleRepository.GetVehiclesReservationsAsync())				
				.Where(DatesOverlapWithExistingReservations(query))
				.GroupBy(r => r.VehicleType)
				.Select(g => new { VehicleType = g.Key, ReservedCount = g.Count() })
				.ToDictionary(g => g.VehicleType, g => g.ReservedCount);

			var availabilityResults = SubtractReservedCountFromAvailableCount(matchingVehicles, overlappingReservationsByVehicleType);

			return availabilityResults.Where(v => v.NumbersAvailable > 0);
		}

		private static Func<Vehicle, bool> VehicleTypeMatchesQuery(VehicleAvailabilityQuery query)
		{
			return vehicle => query.VehicleTypes == null || query.VehicleTypes.Contains(vehicle.VehicleType);
		}

		private static Func<VehicleReservation, bool> DatesOverlapWithExistingReservations(VehicleAvailabilityQuery query)
		{
			return existingReservations => query.PickupOn < existingReservations.ReturnOn && query.ReturnOn > existingReservations.PickupOn;			
		}

		private static IEnumerable<VehicleAvailabilityResponse> SubtractReservedCountFromAvailableCount(IEnumerable<Vehicle> matchingVehicles, Dictionary<VehicleType, int> overlappingReservationsByVehicleType)
		{
			return matchingVehicles
				.GroupBy(v => v.VehicleType)
				.Select(g =>
				{
					var reservedCount = overlappingReservationsByVehicleType.TryGetValue(g.Key, out var count) ? count : 0;

					return new VehicleAvailabilityResponse
					{
						VehicleType = g.Key,
						NumbersAvailable = g.Count() - reservedCount
					};
				});
		}

		public async Task ReserveVehicleAsync(VehicleReservationRequest reservation)
		{
			await _vehicleRepository.ExecuteInTransactionAsync(async () =>
			{
				var query = new VehicleAvailabilityQuery
				{
					PickupOn = reservation.PickupOn,
					ReturnOn = reservation.ReturnOn,
					VehicleTypes = [reservation.VehicleType]
				};

				var availableVehicles = await GetAvailableVehiclesAsync(query);

				if (!availableVehicles.Any())
				{
					throw new RequestedVehicleNotAvailableException("The requested vehicle is not available for the specified dates.");
				}

				var newReservation = new VehicleReservation
				{
					PickupOn = reservation.PickupOn,
					ReturnOn = reservation.ReturnOn,
					VehicleType = reservation.VehicleType
				};

				await _vehicleRepository.AddVehicleReservationAsync(newReservation);
			});
		}
	}
}
