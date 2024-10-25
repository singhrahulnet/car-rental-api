using VehicleRentalApi.Contracts;
using FluentValidation;

namespace VehicleRentalApi.Domain
{
	internal class Validator : AbstractValidator<VehicleReservationRequest>, IValidator
	{
		public Validator()
		{
			RuleFor(vehicleReservation => vehicleReservation.VehicleType)
				.NotNull()
				.WithMessage("Vehicle type is required to reserve a vehicle.");

			RuleFor(vehicleReservation => vehicleReservation)
				.Must(HaveReturnDateLaterThanPickupDate())
				.WithMessage("The return date must be on or later than pick up date.");
		}

		private Func<VehicleReservationRequest, bool> HaveReturnDateLaterThanPickupDate()
		{
			return vehicleReservation =>
				vehicleReservation.ReturnOn >= vehicleReservation.PickupOn;
		}
	}
}
