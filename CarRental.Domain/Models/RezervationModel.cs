using System;
using static CarRental.Domain.Constants;

namespace CarRental.Domain.Models
{
	/// <summary>
	/// Reprezents the car rental rezervation model.
	/// </summary>
	public class RezervationModel
	{
		/// <summary>
		/// Rezervation identifier.
		/// </summary>
		public int RezervationId { get; set; }

		/// <summary>
		/// Client account model.
		/// </summary>
		public ClientAccountModel ClientAccount { get; set; }

		/// <summary>
		/// Car type, Standard, Family, Prestiege.
		/// </summary>
		public CarTypeEnum CarType { get; set; }

		/// <summary>
		/// Car plate number.
		/// </summary>
		public string CarPlateNumber { get; set; }

		/// <summary>
		/// Date of car pick up.
		/// </summary>
		public DateTime? PickUpDate { get; set; }

		/// <summary>
		/// Date of car return.
		/// </summary>
		public DateTime? ReturnDate { get; set; }

		/// <summary>
		/// Deposit fee. It's a percentage of the rental rate fee.
		/// </summary>
		public decimal DepositFee { get; set; }

		/// <summary>
		/// Cancelation fee. It is the price of one or more hours, but not more than total amount of reservation.
		/// </summary>
		public decimal? CancellationFee { get; set; }

		/// <summary>
		/// Rate under which the cancelation fee is
		/// </summary>
		public decimal? CancelationFeeRate { get; set; }

		/// <summary>
		/// Rental fee.
		/// </summary>
		public decimal RentaltFee { get; set; }

		/// <summary>
		/// If TRUE indicates that the car has been picked up.
		/// </summary>
		public bool IsPickedUp { get; set; }

		/// <summary>
		/// If TRUE indicates that the rezervations is cancelled.
		/// </summary>
		public bool IsCancelled { get; set; }

		/// <summary>
		/// If TRUE indicates that the car is returned.
		/// </summary>
		public bool IsReturned { get; set; }
	}
}