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
				.Where(VehicleTypeMatchesWithQuery(query));

			var overlappingReservations = (await _vehicleRepository.GetVehiclesReservationsAsync())
				.Where(RequestedDateOverlapsWithExistingReservations(query))
				.GroupBy(r => r.VehicleType)
				.Select(g => new { VehicleType = g.Key, ReservedCount = g.Count() });


			var availabilityQuery = matchingVehicles
				.GroupBy(v => v.VehicleType)
				.Select(g =>
				{
					var reservedCount = overlappingReservations
						.Where(res => res.VehicleType == g.Key)
						.Select(res => res.ReservedCount)
						.FirstOrDefault();

					return new VehicleAvailabilityResponse
					{
						VehicleType = g.Key,
						NumbersAvailable = g.Count() - reservedCount
					};
				});

			return availabilityQuery.Where(v => v.NumbersAvailable > 0);
		}

		private static Func<VehicleReservation, bool> RequestedDateOverlapsWithExistingReservations(VehicleAvailabilityQuery query)
		{
			return existingReservations => (query.PickupOn >= existingReservations.PickupOn && query.PickupOn < existingReservations.ReturnOn) ||
										(query.ReturnOn > existingReservations.PickupOn && query.ReturnOn <= existingReservations.ReturnOn) ||
										(query.PickupOn <= existingReservations.PickupOn && query.ReturnOn >= existingReservations.ReturnOn);
		}

		private static Func<Vehicle, bool> VehicleTypeMatchesWithQuery(VehicleAvailabilityQuery query)
		{
			return vehicles => query.VehicleTypes == null || query.VehicleTypes.Contains(vehicles.VehicleType);
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
