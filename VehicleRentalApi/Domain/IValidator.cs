using VehicleRentalApi.Contracts;
using FluentValidation.Results;

namespace VehicleRentalApi.Domain
{
	public interface IValidator
	{
		ValidationResult Validate(VehicleReservationRequest vehicleReservationRequest);
	}
}
