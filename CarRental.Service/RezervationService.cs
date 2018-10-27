﻿using CarRental.Data;
using CarRental.Data.Entities;
using CarRental.Domain;
using CarRental.Domain.Models;
using CarRental.Domain.Parameters;
using CarRental.Service.Interfaces;
using CarRental.Service.Mappers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarRental.Domain.Constants;

namespace CarRental.Service
{
	/// <summary>
	/// Handels the logic for rezervation CRUD, booking a rezervation, rezervation cancellation, car pickup and car return.
	/// </summary>
	public class RezervationService : IRezervationService
	{
		private readonly CarRentalDbContext dbContext;
		private readonly IClientAccountService clientAccountService;

		/// <summary>
		/// Creates a new instance of the Rezervation service.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="clientAccountService"></param>
		public RezervationService(CarRentalDbContext context, IClientAccountService clientAccountService)
		{
			this.dbContext = context;
			this.clientAccountService = clientAccountService;
		}

		/// <summary>
		/// Creates a booking for the client account.
		/// </summary>
		/// <param name="parameters">Rezervation creation parameters. Hold information on the client account and the rezervation.</param>
		/// <returns></returns>
		public RezervationModel CreateBooking(RezervationCreationParameters parameters)
		{
			if(parameters == null)
			{
				throw new ArgumentNullException(nameof(parameters));
			}

			if(parameters.ClientAccount == null
				|| !Enum.IsDefined(typeof(CarTypeEnum), parameters.CarType)
				|| string.IsNullOrWhiteSpace(parameters.CarPlateNumber)
				|| parameters.PickUpDate > DateTime.Now
				|| parameters.ReturnDate > parameters.PickUpDate)
			{
				throw new InvalidOperationException("Invalid parameters.");
			}

			//create a client account
			var clientAccount = this.clientAccountService.Add(parameters.ClientAccount);

			var dbRezervation = new Rezervation()
			{
				ClientId = clientAccount.ClientId,
				CarPlateNumber = parameters.CarPlateNumber,
			};
			
			//get the car type
			var carType = CarTypes.GetCarType(parameters.CarType);

			dbRezervation.RentaltFee = carType.GetRentalFee(parameters.PickUpDate, parameters.ReturnDate);
			dbRezervation.DepositFee = carType.GetDepositFee(dbRezervation.RentaltFee);

			this.dbContext.Rezervations.Add(dbRezervation);
			this.dbContext.SaveChanges();			

			return dbRezervation.ToModel(clientAccount);
		}

		/// <summary>
		/// Sets the flag which determines if the car is picked up.
		/// </summary>
		/// <param name="rezervationId">Rezervation identifier.</param>
		/// <returns>True if the operation complete successfully</returns>
		public bool PickUpCar(int rezervationId)
		{
			if(rezervationId < 1)
			{
				return false;
			}

			var dbRezervation = this.dbContext.Rezervations.SingleOrDefault(x => x.RezervationId == rezervationId);

			if(dbRezervation == null)
			{
				return false;
			}

			// TODO: Maybe we should calculate the rental fee again, but it's not defined in the use case.
			dbRezervation.IsPickedUp = true;
			this.dbContext.SaveChanges();

			return true;			
		}

		/// <summary>
		/// Sets the flag which indicates of the car is return and set rental fee if needed to be calculated.
		/// </summary>
		/// <param name="rezervationId"></param>
		/// <returns>True if the operation complete successfully</returns>
		public bool ReturnCar(int rezervationId)
		{
			if (rezervationId < 1)
			{
				return false;
			}

			var dbRezervation = this.dbContext.Rezervations.SingleOrDefault(x => x.RezervationId == rezervationId);

			if (dbRezervation == null)
			{
				return false;
			}

			if(dbRezervation.IsPickedUp && !dbRezervation.IsCancelled)
			{
				throw new InvalidOperationException("Error returning car.");
			}

			dbRezervation.IsReturned = true;

			// in case the return date is more that the one intially specified then recalucate the rental fee.
			if (dbRezervation.ReturnDate < DateTime.Now)
			{
				dbRezervation.RentaltFee = CarTypes.GetCarType((CarTypeEnum)dbRezervation.CarType).GetRentalFee(dbRezervation.PickUpDate, DateTime.Now);
			}
			dbRezervation.RentaltFee = dbRezervation.RentaltFee - dbRezervation.DepositFee;
			dbRezervation.CancelationFeeRate = 0.0m;
			dbRezervation.CancellationFee = 0.0m;
			this.dbContext.SaveChanges();

			return true;
		}

		/// <summary>
		/// Sets the flag which indicates of the rezervation is cancelled.
		/// </summary>
		/// <param name="rezervationId"></param>
		/// <returns>True if the operation complete successfully</returns>
		public bool CancelRezervation(int rezervationId, decimal cancelationFeeRate)
		{
			if (rezervationId < 1)
			{
				return false;
			}

			var dbRezervation = this.dbContext.Rezervations.SingleOrDefault(x => x.RezervationId == rezervationId);

			if (dbRezervation == null)
			{
				return false;
			}

			if(dbRezervation.IsReturned)
			{
				throw new InvalidOperationException("Error returning car.");
			}

			dbRezervation.IsCancelled = true;

			dbRezervation.CancellationFee = CarTypes.GetCarType((CarTypeEnum)dbRezervation.CarType).GetCancellationFee(cancelationFeeRate);
			dbRezervation.CancelationFeeRate = cancelationFeeRate;
			
			// we set this to 0 since the car is not rented.
			dbRezervation.RentaltFee = 0.0m;
			dbRezervation.DepositFee = 0.0m;

			this.dbContext.SaveChanges();

			return true;
		}

		/// <summary>
		/// Allows to fetch reservation according to specified options:
		/// Booked / Canceled / Picked-up / Returned
		/// Picked-up date-time from
		/// Picked-up date-time to
		/// Client full name or email or phone number
		/// </summary>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public ICollection<RezervationModel> FindRezervations(RezervationBrowsingParams parameters)
		{
			if(parameters == null)
			{
				throw new ArgumentNullException(nameof(parameters));
			}

			var dbRezervations = this.dbContext.Rezervations.AsQueryable();

			if(!string.IsNullOrWhiteSpace(parameters.ClientEmail))
			{
				dbRezervations = dbRezervations.Include(x => x.ClientAccount).Where(x => x.ClientAccount.Email == parameters.ClientEmail);
			}

			if (!string.IsNullOrWhiteSpace(parameters.ClientFullName))
			{
				dbRezervations = dbRezervations.Include(x => x.ClientAccount).Where(x => x.ClientAccount.Email == parameters.ClientFullName);
			}

			if (!string.IsNullOrWhiteSpace(parameters.ClientPhone))
			{
				dbRezervations = dbRezervations.Include(x => x.ClientAccount).Where(x => x.ClientAccount.Email == parameters.ClientPhone);
			}

			if(parameters.PickUpDateFrom.HasValue)
			{
				dbRezervations = dbRezervations.Where(x => x.PickUpDate >= parameters.PickUpDateFrom);

			}

			if (parameters.PickUpDateTo.HasValue)
			{
				dbRezervations = dbRezervations.Where(x => x.PickUpDate <= parameters.PickUpDateTo);
			}

			if(parameters.IsBooked)
			{
				dbRezervations = dbRezervations.Where(x => !x.IsCancelled && !x.IsPickedUp && !x.IsReturned);
			}

			if (parameters.IsReturned)
			{
				dbRezervations = dbRezervations.Where(x => x.IsReturned);
			}

			if (parameters.IsPickedUp)
			{
				dbRezervations = dbRezervations.Where(x => x.IsPickedUp && !x.IsPickedUp && !x.IsCancelled);
			}

			if (parameters.IsCancelled)
			{
				dbRezervations = dbRezervations.Where(x => x.IsCancelled);
			}

			return dbRezervations.ToList().Select(x => x.ToModel()).ToList();
		}
	}
}