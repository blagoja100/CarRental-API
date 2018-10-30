using CarRental.Api.Attributes;
using CarRental.Domain.Models;
using CarRental.Domain.Parameters;
using CarRental.Service.Interfaces;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CarRental.Api.Controllers
{
	/// <summary>
	/// Controller responsible for the car reservations.
	/// </summary>
	/// <remarks>
	/// Contains endpoins for the creation of rezervation, car pick-up and return, reservations cancelations and rezervation browsing.
	/// </remarks>
	[InvalidParametersException]
	[NotFoundException]
	public class RezervationController : ApiController
	{
		private readonly IRezervationService rezervationService;

		/// <summary>
		/// Creates a new instance of the rezervation controller.
		/// </summary>
		/// <param name="rezervationService"></param>
		public RezervationController(IRezervationService rezervationService)
		{
			this.rezervationService = rezervationService;
		}

		/// <summary>
		/// Books a rezervation for the specified client account.
		/// </summary>
		/// <remarks>
		///	Creates new reservation with following information. The client account information is mandatory.
		/// You can provide a client account identifier, in which case the rezervation will be added to an existing account.
		/// Or, you can provide a new client account information, in which case the client account will be created automatically.
		///
		/// When creating a reservation a you must specify a car type.
		/// The api supports the following car types:
		///		- Standard:
		///			RentalRateFee = 10.00,
		///			CancellationFee = 5.00,
		///			DepositFeePercentage = 10%
		///		- Family:
		///			RentalRateFee = 12.00,
		///			CancellationFee = 7.00,
		///			DepositFeePercentage = 12%
		///		- Prestiege:
		///			RentalRateFee = 50.00,
		///			CancellationFee = 25.00,
		///			DepositFeePercentage = 70%
		/// </remarks>
		/// <param name="parameters">Reservation creation parameters.</param>
		/// <returns>Reservation information.</returns>
		/// <response code="400">In case of invalid parameters.</response>
		[HttpPost]
		public RezervationModel BookRezervation(RezervationCreationParameters parameters) => this.rezervationService.CreateBooking(parameters);

		/// <summary>
		/// Marks the reservation as picked up.
		/// </summary>
		/// <remarks>
		///	When the client picks up the car, the reservation is marked as picked up.
		/// </remarks>
		/// <param name="parameters">Reservation retireval parameters. The reservation identifier must be provided.</param>
		/// <returns>TRUE if the reservation is marked as picked up successfully.</returns>
		/// <response code="400">In case of invalid parameters.</response>
		[HttpPost]
		public bool PickUpCar(RezervationRetrievalParams parameters) => this.rezervationService.PickUpCar(parameters.RezervationId);

		/// <summary>
		/// Marks the reservation as returned up.
		/// </summary>
		/// <remarks>
		///	When the client returns the car, the reservation is marked as returned.
		///	The rental fee is also adjusted in case the car is returned later that the initial return date.
		/// </remarks>
		/// <param name="parameters">Reservation retireval parameters. The reservation identifier must be provided.</param>
		/// <returns>TRUE if the reservation is marked as returned successfully.</returns>
		/// <response code="400">In case of invalid parameters.</response>
		[HttpPost]
		public bool ReturnCar(RezervationRetrievalParams parameters) => this.rezervationService.ReturnCar(parameters.RezervationId);

		/// <summary>
		/// Cancels the reservation.
		/// </summary>
		/// <remarks>
		///	Marks the reservation as cancelled. The cancelation fee is calculated.
		///	In order to for the cancelation fee to be configurable for each car type a cancelation fee rate can be provided to adjust the cancellation fee.
		/// </remarks>
		/// <param name="parameters">Reservation retireval parameters. The reservation identifier must be provided.</param>
		/// <returns>TRUE if the reservation is marked as cancelled successfully.</returns>
		/// <response code="400">In case of invalid parameters.</response>
		[HttpPost]
		public bool CancelRezervation(RezervationCancellationParams parameters)
		{
			if (parameters == null)
			{
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "Invalid parameters." });
			}

			return this.rezervationService.CancelRezervation(parameters.RezervationId, parameters.CancellationFeeRate);
		}

		/// <summary>
		/// Gets a list of reservations filtered by specified fields.
		/// </summary>
		/// <remarks>
		///	Allows to fetch reservation according to specified options:
		///		- Booked / Canceled / Picked-up / Returned
		///		- Picked-up date-time from
		///		- Picked-up date-time to
		///		- Client full name or email or phone number
		///
		///	The collection can be paged by providing a startIndex and maximum number of items to retrieve.
		/// </remarks>
		/// <param name="parameters">Reservation browsing parameters. If a parameter value is not specified, the fields is not included in the search.</param>
		/// <returns>Collection of reservations.</returns>
		/// <response code="400">In case of invalid parameters.</response>
		[HttpPost]
		public RezervationCollectionModel GetAllRezervations(RezervationBrowsingParams parameters)
		{
			if (parameters == null)
			{
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "Invalid parameters." });
			}

			return this.rezervationService.FindRezervations(parameters);
		}
	}
}