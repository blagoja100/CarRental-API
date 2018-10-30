using CarRental.Data;
using CarRental.Data.Entities;
using CarRental.Domain.Models;
using CarRental.Domain.Parameters;
using CarRental.Service.Exceptions;
using CarRental.Service.Interfaces;
using CarRental.Service.Mappers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
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
			this.ValidateBookingParameters(parameters);

			//try to get a client account first
			var clientAccount = this.dbContext.ClientAccounts.SingleOrDefault(x => x.ClientId == parameters.ClientId)?.ToModel();

			if (clientAccount == null && parameters.ClientAccount != null)
			{
				clientAccount = this.clientAccountService.Add(parameters.ClientAccount);
			}
			else if (parameters.ClientAccount == null && clientAccount == null)
			{
				throw new NotFoundException("Client account not found.");
			}

			var dbRezervation = new Rezervation()
			{
				ClientId = clientAccount.ClientId,
				CarPlateNumber = parameters.CarPlateNumber,
				PickUpDate = parameters.PickUpDate,
				ReturnDate = parameters.ReturnDate,
			};

			//get the car type
			var carType = CarTypes.GetCarType(parameters.CarType);

			dbRezervation.CarType = (int)carType.Type;
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
			var dbRezervation = this.GetRezervation(rezervationId);

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
			var dbRezervation = this.GetRezervation(rezervationId);

			if (!dbRezervation.IsPickedUp || dbRezervation.IsCancelled)
			{
				throw new InvalidOperationException("Car has not been picked up yet, or rezervation cancelled.");
			}

			dbRezervation.IsReturned = true;

			// in case the return date is more that the one intially specified then recalucate the rental fee.
			if (dbRezervation.ReturnDate < DateTime.Now)
			{
				dbRezervation.RentaltFee = CarTypes.GetCarType((CarTypeEnum)dbRezervation.CarType).GetRentalFee(dbRezervation.PickUpDate, DateTime.Now);
			}
			dbRezervation.RentaltFee = dbRezervation.RentaltFee;
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
			var dbRezervation = this.GetRezervation(rezervationId);

			if (dbRezervation.IsReturned)
			{
				throw new InvalidOperationException("Car already returned.");
			}

			dbRezervation.IsCancelled = true;

			var cancelationFee = CarTypes.GetCarType((CarTypeEnum)dbRezervation.CarType).GetCancellationFee(cancelationFeeRate);

			// cancelation fee cannot be bigger than the rental fee.
			dbRezervation.CancellationFee = cancelationFee > dbRezervation.RentaltFee ? dbRezervation.RentaltFee : cancelationFee;
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
		public RezervationCollectionModel FindRezervations(RezervationBrowsingParams parameters)
		{
			if (parameters == null)
			{
				throw new InvalidParameterException(nameof(parameters));
			}

			var dbRezervations = this.dbContext.Rezervations.AsQueryable();

			if (!string.IsNullOrWhiteSpace(parameters.ClientEmail))
			{
				dbRezervations = dbRezervations.Include(x => x.ClientAccount).Where(x => x.ClientAccount.Email == parameters.ClientEmail);
			}

			if (!string.IsNullOrWhiteSpace(parameters.ClientFullName))
			{
				dbRezervations = dbRezervations.Include(x => x.ClientAccount).Where(x => x.ClientAccount.FullName == parameters.ClientFullName);
			}

			if (!string.IsNullOrWhiteSpace(parameters.ClientPhone))
			{
				dbRezervations = dbRezervations.Include(x => x.ClientAccount).Where(x => x.ClientAccount.Phone == parameters.ClientPhone);
			}

			if (parameters.PickUpDateFrom.HasValue)
			{
				dbRezervations = dbRezervations.Where(x => x.PickUpDate >= parameters.PickUpDateFrom);
			}

			if (parameters.PickUpDateTo.HasValue)
			{
				dbRezervations = dbRezervations.Where(x => x.PickUpDate <= parameters.PickUpDateTo);
			}

			if (parameters.IsBooked)
			{
				dbRezervations = dbRezervations.Where(x => !x.IsCancelled && !x.IsPickedUp && !x.IsReturned);
			}

			if (parameters.IsReturned)
			{
				dbRezervations = dbRezervations.Where(x => x.IsReturned);
			}

			if (parameters.IsPickedUp)
			{
				dbRezervations = dbRezervations.Where(x => x.IsPickedUp && !x.IsCancelled);
			}

			if (parameters.IsCancelled)
			{
				dbRezervations = dbRezervations.Where(x => x.IsCancelled);
			}

			return new RezervationCollectionModel(dbRezervations.ToList().Select(x => x.ToModel()).ToList(), parameters.StartIndex, parameters.MaxItems);
		}

		private void ValidateBookingParameters(RezervationCreationParameters parameters)
		{
			if (parameters == null)
			{
				throw new InvalidParameterException("Parameters not provided.");
			}

			if (!Enum.IsDefined(typeof(CarTypeEnum), parameters.CarType))
			{
				throw new InvalidParameterException(nameof(parameters.CarType));
			}

			if (string.IsNullOrWhiteSpace(parameters.CarPlateNumber))
			{
				throw new InvalidParameterException(nameof(parameters.CarPlateNumber));
			}

			if (parameters.PickUpDate < DateTime.Now)
			{
				throw new InvalidParameterException($"{nameof(parameters.PickUpDate)} must be set after the current time.");
			}

			if (parameters.ReturnDate < parameters.PickUpDate)
			{
				throw new InvalidParameterException($"{nameof(parameters.ReturnDate)} must be set after {nameof(parameters.PickUpDate)}");
			}

			if (parameters.ClientAccount == null && parameters.ClientId < 1)
			{
				throw new InvalidParameterException("Client account not provided");
			}
		}

		private Rezervation GetRezervation(int rezervationId)
		{
			if (rezervationId < 1)
			{
				throw new InvalidParameterException(nameof(rezervationId));
			}

			var dbRezervation = this.dbContext.Rezervations.SingleOrDefault(x => x.RezervationId == rezervationId);

			if (dbRezervation == null)
			{
				throw new NotFoundException("Rezervation not found.");
			}

			return dbRezervation;
		}
	}
}