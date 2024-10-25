using CarRentalApi.Contracts;
using FluentValidation.Results;

namespace CarRentalApi.Domain
{
	internal interface IValidator
	{
		public ValidationResult Validate(CarReservation carReservation);
	}
}
