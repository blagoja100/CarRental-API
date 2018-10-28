using CarRental.Domain.Models;
using CarRental.Domain.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Service.Interfaces
{
	/// <summary>
	/// Handels the logic for rezervation CRUD, booking a rezervation, rezervation cancellation, car pickup and car return.
	/// </summary>
	public interface IRezervationService
	{
		/// <summary>
		/// Creates a booking for the client account.
		/// </summary>
		/// <param name="parameters">Rezervation creation parameters. Hold information on the client account and the rezervation.</param>
		/// <returns>Rezervation model.</returns>
		RezervationModel CreateBooking(RezervationCreationParameters parameters);

		/// <summary>
		/// Sets the flag which determines if the car is picked up.
		/// </summary>
		/// <param name="rezervationId">Rezervation identifier.</param>
		/// <returns>True if the operation complete successfully</returns>
		bool PickUpCar(int rezervationId);

		/// <summary>
		/// Sets the flag which indicates of the car is return and set rental fee if needed to be calculated.
		/// </summary>
		/// <param name="rezervationId"></param>
		/// <returns>True if the operation complete successfully</returns>
		bool ReturnCar(int rezervationId);

		/// <summary>
		/// Sets the flag which indicates of the rezervation is cancelled.
		/// </summary>
		/// <param name="rezervationId"></param>
		/// <returns>True if the operation complete successfully</returns>
		bool CancelRezervation(int rezervationId, decimal cancelationFeeRate);

		/// <summary>
		/// Allows to fetch reservation according to specified options:
		/// Booked / Canceled / Picked-up / Returned
		/// Picked-up date-time from
		/// Picked-up date-time to
		/// Client full name or email or phone number
		/// </summary>
		/// <param name="parameters"></param>
		/// <returns></returns>
		RezervationCollectionModel FindRezervations(RezervationBrowsingParams parameters);
	}
}
