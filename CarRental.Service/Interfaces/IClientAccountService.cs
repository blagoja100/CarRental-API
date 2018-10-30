using CarRental.Domain.Models;
using CarRental.Domain.Parameters;

namespace CarRental.Service.Interfaces
{
	/// <summary>
	/// Handles the CRUD opperations on the Client account.
	/// </summary>
	public interface IClientAccountService
	{
		/// <summary>
		/// Get a client account for the specified identifier.
		/// </summary>
		/// <param name="clientId"></param>
		/// <returns></returns>
		ClientAccountModel Get(int clientId);

		/// <summary>
		/// Creates a new client account.
		/// </summary>
		/// <param name="parameters">Client account creation parameters. Contains all the client account information.</param>
		/// <returns></returns>
		ClientAccountModel Add(ClientAccountCreationParams parameters);

		/// <summary>
		/// Updates a client account.
		/// </summary>
		/// <param name="parameters">Client account modification parameters. Contains all the client account information as well as the client identifier.</param>
		/// <returns></returns>
		ClientAccountModel Update(ClientAccountModificationParams parameters);

		/// <summary>
		/// Calculates total fees (Rental rate fee, Cancellation fee,  Deposit fee) from all reservations for specified client.
		/// </summary>
		/// <param name="clientId">Client identifier.</param>
		/// <returns>Client balance model.</returns>
		ClientBalanceModel GetClientAccountBalance(int clientId);
	}
}