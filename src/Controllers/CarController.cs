using CarRentalApi.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CarController : ControllerBase
	{
		[HttpGet]
		public IEnumerable<string> Get([FromQuery] CarAvailabilityQuery carAvailabilityQuery)
		{
			return new string[] { "value1", "value2" };
		}

		[HttpPost]
		public void Post([FromBody] CarReservation carReservation)
		{
		}
	}
}
