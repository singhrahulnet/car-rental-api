using CarRentalApi.Contracts;
using FluentValidation.Results;

namespace CarRentalApi.Domain
{
	public interface IValidator
	{
		ValidationResult Validate(CarReservation carReservation);
	}
}
