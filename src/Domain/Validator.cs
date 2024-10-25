using CarRentalApi.Contracts;
using FluentValidation;

namespace CarRentalApi.Domain
{
	internal class Validator : AbstractValidator<CarReservation>, IValidator
	{
		public Validator()
		{
			RuleFor(carReservation => carReservation.VehicleType)
				.NotNull()
				.WithMessage("Vehicle type is required to reserve a car.");

			RuleFor(carReservation => carReservation)
				.Must(HaveReturnDateLaterThanPickupDate())
				.WithMessage("The return date must be on or later than pick up date.");
		}

		private Func<CarReservation, bool> HaveReturnDateLaterThanPickupDate()
		{
			return careReservation =>
				careReservation.ReturnOn >= careReservation.PickupOn;
		}
	}
}
