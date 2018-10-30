namespace CarRental.Domain.Parameters
{
	/// <summary>
	/// Rezervation creation parameters.
	/// </summary>
	public class RezervationCreationParameters : RezervationBaseParameters
	{
		/// <summary>
		/// Client account model.
		/// </summary>
		public ClientAccountCreationParams ClientAccount { get; set; }

		/// <summary>
		/// Client account identifier.
		/// </summary>
		public int ClientId { get; set; }
	}
}