using VehicleRentalApi.Contracts;
using FluentValidation;

namespace VehicleRentalApi.Domain
{
	internal class ValidatorForReservationRequest : AbstractValidator<VehicleReservationRequest>
	{
		public ValidatorForReservationRequest()
		{
			RuleFor(vehicleReservation => vehicleReservation.VehicleType)
				.NotNull()
				.NotEmpty()
				.WithMessage("Vehicle type is required to reserve a vehicle.");

			RuleFor(vehicleReservation => vehicleReservation.PickupOn)
				.NotEmpty()
				.WithMessage("The pick up date must be specified");

			RuleFor(vehicleReservation => vehicleReservation.ReturnOn)
				.NotEmpty()
				.WithMessage("The return date must be specified")
				.GreaterThanOrEqualTo(reservation => reservation.PickupOn)
				.WithMessage("The return date must be on or later than pick up date.");
		}
	}
	internal class ValidatorForAvailabilityQuery : AbstractValidator<VehicleAvailabilityQuery>
	{
		public ValidatorForAvailabilityQuery()
		{
			RuleFor(query => query.PickupOn)
				.NotEmpty()
				.WithMessage("The pick up date must be specified");

			RuleFor(query => query.ReturnOn)
				.NotEmpty()
				.WithMessage("The return date must be specified")
				.GreaterThanOrEqualTo(query => query.PickupOn)
				.WithMessage("The return date must be on or later than pick up date.");
		}
	}

	public interface IValidatorResolver
	{
		IValidator<T> GetValidator<T>();
	}

	internal class ValidatorResolver : IValidatorResolver
	{
		private readonly Dictionary<Type, IValidator> _validators;

		public ValidatorResolver(ValidatorForReservationRequest reservationValidator,
								 ValidatorForAvailabilityQuery availabilityValidator)
		{
			_validators = new Dictionary<Type, IValidator>
		{
			{ typeof(VehicleReservationRequest), reservationValidator },
			{ typeof(VehicleAvailabilityQuery), availabilityValidator }
		};
		}

		public IValidator<T> GetValidator<T>()
		{
			return _validators[typeof(T)] as IValidator<T>;
		}
	}
}
