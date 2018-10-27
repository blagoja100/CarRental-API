using CarRental.Domain.Models;
using CarRental.Domain.Parameters;
using CarRental.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CarRental.Api.Controllers
{
	/// <summary>
	/// Controller responsible for the client account.
	/// </summary>
	public class ClientAccountController : ApiController
    {
		private readonly IClientAccountService clientAccountService;

		/// <summary>
		/// Creates new instance of the client account controller.
		/// </summary>
		/// <param name="clientAccountService"></param>
		public ClientAccountController(IClientAccountService clientAccountService)
		{
			this.clientAccountService = clientAccountService;
		}

		[HttpPost]
		public ClientAccountModel AddClientAccount(ClientAccountCreationParams parameters)
		{
			return this.clientAccountService.Add(parameters);
		}

		[HttpPost]
		public ClientAccountModel UpdateClientAccount(ClientAccountModificationParams parameters)
		{
			return this.clientAccountService.Update(parameters);
		}

		[HttpPost]
		public ClientAccountModel GetClientAccount(ClientAccountRetrievalParams parameters)
		{
			return this.clientAccountService.Get(parameters.ClientId);
		}

		[HttpPost]
		public ClientBalanceModel GetClientBalance(ClientAccountRetrievalParams pameters)
		{
			return this.clientAccountService.GetClientAccountBalance(pameters.ClientId);
		}
	}
}
