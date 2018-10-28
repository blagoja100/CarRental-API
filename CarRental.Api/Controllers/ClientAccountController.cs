﻿using CarRental.Domain.Models;
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
	/// <remarks>
	///  Contains endpoints for the creation, modification and retrieval of client accounts as weel as the endpoint for the client account balance retrieval.
	/// </remarks>
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

		/// <summary>
		/// Adds a client account.
		/// </summary>
		/// <remarks>
		///		Creates a new client account entry. This is part of the client account CRUD operations.
		/// </remarks>
		/// <param name="parameters">Client account creation parameters.</param>
		/// <returns>Client account.</returns>
		/// <response code="400">In case of invalid parameters.</response>
		[HttpPost]
		public ClientAccountModel AddClientAccount(ClientAccountCreationParams parameters)
		{
			if(parameters == null || string.IsNullOrWhiteSpace(parameters.FullName) || string.IsNullOrWhiteSpace(parameters.Email) || string.IsNullOrWhiteSpace(parameters.Phone))
			{
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "Invalid parameters." });
			}

			return this.clientAccountService.Add(parameters);
		}

		/// <summary>
		/// Updates the client account.
		/// </summary>
		/// <remarks>
		///		Updates an existing client account entry. This is part of the client account CRUD operations.
		///		The Client identifier must be provided.
		/// </remarks>
		/// <param name="parameters">Client account modification parameters.</param>
		/// <returns>Client account.</returns>
		/// <response code="400">In case of invalid parameters.</response>
		[HttpPost]
		public ClientAccountModel UpdateClientAccount(ClientAccountModificationParams parameters)
		{
			if (parameters == null || parameters.ClientId < 1 || string.IsNullOrWhiteSpace(parameters.FullName) || string.IsNullOrWhiteSpace(parameters.Email) || string.IsNullOrWhiteSpace(parameters.Phone))
			{
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "Invalid parameters." });
			}

			return this.clientAccountService.Update(parameters);
		}

		/// <summary>
		/// Gets the client account.
		/// </summary>
		/// <remarks>
		///		Gets the client account information.
		///		The Client identifier must be provided.
		/// </remarks>
		/// <param name="parameters">Client account retrieval parameters.</param>
		/// <returns>Client account.</returns>
		/// <response code="400">In case of invalid parameters.</response>
		[HttpPost]
		public ClientAccountModel GetClientAccount(ClientAccountRetrievalParams parameters)
		{
			if (parameters == null || parameters.ClientId < 1)
			{
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "Invalid parameters." });
			}

			return this.clientAccountService.Get(parameters.ClientId);
		}

		/// <summary>
		/// Gets the client account ballance.
		/// </summary>
		/// <remarks>
		///		Returns total fees (Rental rate fee, Cancellation fee,  Deposit fee) from all reservations for specified client.
		/// </remarks>
		/// <param name="parameters">Client account retrieval parameters.</param>
		/// <returns>Client account.</returns>
		/// <response code="400">In case of invalid parameters.</response>
		[HttpPost]
		public ClientBalanceModel GetClientBalance(ClientAccountRetrievalParams parameters)
		{
			if (parameters == null || parameters.ClientId < 1)
			{
				throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "Invalid parameters." });
			}

			return this.clientAccountService.GetClientAccountBalance(parameters.ClientId);
		}
	}
}
