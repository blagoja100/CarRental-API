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
	}
}
