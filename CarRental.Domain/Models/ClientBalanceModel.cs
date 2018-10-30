namespace CarRental.Domain.Models
{
	/// <summary>
	/// Hold information on client's total fees (Rental rate fee, Cancellation fee,  Deposit fee) from all reservations for specified client.
	/// </summary>
	public class ClientBalanceModel
	{
		/// <summary>
		/// Client account of the returned balance.
		/// </summary>
		public ClientAccountModel ClientAccount { get; set; }

		/// <summary>
		/// Total rental fee from all the rezervations.
		/// </summary>
		public decimal TotalRentalFee { get; set; }

		/// <summary>
		/// Total cancellation fee from all the rezervations.
		/// </summary>
		public decimal TotalCancellationFee { get; set; }

		/// <summary>
		/// Total fees payed by the client.
		/// </summary>
		public decimal TotalFees { get; set; }
	}
}