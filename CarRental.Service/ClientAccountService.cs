using CarRental.Data;
using CarRental.Data.Entities;
using CarRental.Domain.Models;
using CarRental.Domain.Parameters;
using CarRental.Service.Interfaces;
using CarRental.Service.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Service
{
	/// <summary>
	/// Handles the CRUD opperations on the Client account.
	/// </summary>
    public class ClientAccountService : IClientAccountService
    {
		private readonly CarRentalDbContext dbContext;

		/// <summary>
		/// Creates new instance of the Client account service.
		/// </summary>
		/// <param name="context"></param>
		public ClientAccountService(CarRentalDbContext context)
		{
			this.dbContext = context;
		}
		/// <summary>
		/// Get a client account for the specified identifier.
		/// </summary>
		/// <param name="clientId"></param>
		/// <returns></returns>
		public ClientAccountModel Get(int clientId)
		{			
			if(clientId < 1)
			{
				return null;
			}

			var dbClientAccount = this.dbContext.ClientAccounts.SingleOrDefault(x => x.ClientId == clientId);

			if(dbClientAccount == null)
			{
				return null;
			}

			return dbClientAccount.ToModel();
		}

		/// <summary>
		/// Creates a new client account.
		/// </summary>
		/// <param name="parameters">Client account creation parameters. Contains all the client account information.</param>
		/// <returns>Client account model.</returns>
		public ClientAccountModel Add(ClientAccountCreationParams parameters)
		{
			if (parameters == null)
			{
				throw new ArgumentNullException(nameof(parameters));
			}

			if (string.IsNullOrWhiteSpace(parameters.FullName) || string.IsNullOrWhiteSpace(parameters.Email) || string.IsNullOrWhiteSpace(parameters.Phone))
			{
				throw new InvalidOperationException("Invalid parameters.");
			}

			var dbClientAccount = new ClientAccount
			{
				Email = parameters.Email,
				FullName = parameters.FullName,
				Phone = parameters.Phone,
			};

			this.dbContext.ClientAccounts.Add(dbClientAccount);
			this.dbContext.SaveChanges();

			return dbClientAccount.ToModel();
		}

		/// <summary>
		/// Updates a client account.
		/// </summary>
		/// <param name="parameters">Client account modification parameters. Contains all the client account information as well as the client identifier.</param>
		/// <returns>Client account model.</returns>
		public ClientAccountModel Update(ClientAccountModificationParams parameters)
		{
			if (parameters == null)
			{
				throw new ArgumentNullException(nameof(parameters));
			}

			if (parameters.ClientId < 1 || string.IsNullOrWhiteSpace(parameters.FullName) || string.IsNullOrWhiteSpace(parameters.Email) || string.IsNullOrWhiteSpace(parameters.Phone))
			{
				throw new InvalidOperationException("Invalid parameters.");
			}

			var dbClientAccount = this.dbContext.ClientAccounts.SingleOrDefault(x => x.ClientId == parameters.ClientId);
			if(dbClientAccount == null)
			{
				throw new InvalidOperationException("Client account not found.");
			}

			dbClientAccount.Email = parameters.Email;
			dbClientAccount.FullName = parameters.FullName;
			dbClientAccount.Phone = parameters.Phone;
								
			this.dbContext.SaveChanges();

			return dbClientAccount.ToModel();			
		}

		/// <summary>
		/// Calculates total fees (Rental rate fee, Cancellation fee,  Deposit fee) from all reservations for specified client.
		/// </summary>
		/// <param name="clientId">Client identifier.</param>
		/// <returns>Client balance model.</returns>
		public ClientBalanceModel GetClientAccountBalance(int clientId)
		{
			if (clientId < 1)
			{
				throw new InvalidOperationException("Invalid parameters.");
			}

			var dbClientAccount = this.dbContext.ClientAccounts.SingleOrDefault(x => x.ClientId == clientId);
			if (dbClientAccount == null)
			{
				throw new InvalidOperationException("Client account not found.");
			}

			var clientBalanceModel = new ClientBalanceModel
			{
				ClientAccount = dbClientAccount.ToModel(),
			};

			var dbClientRezervations = this.dbContext.Rezervations.Where(x => x.ClientId == clientId).ToList();

			if(!dbClientRezervations.Any())
			{
				return clientBalanceModel;
			}

			clientBalanceModel.TotalRentalFee = dbClientRezervations.Where(x => x.IsReturned).Sum(x => x.RentaltFee);
			clientBalanceModel.TotalCancellationFee = dbClientRezervations.Where(x => x.CancellationFee.HasValue && x.IsCancelled).Sum(x => x.CancellationFee.Value);
			clientBalanceModel.TotalFees = clientBalanceModel.TotalCancellationFee + clientBalanceModel.TotalRentalFee;

			return clientBalanceModel;
		}
	}
}
