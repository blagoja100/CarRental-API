using System;
using System.Linq;
using CarRental.Data;
using CarRental.Domain.Parameters;
using CarRental.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static CarRental.Domain.Constants;

namespace CarRental.Tests.ServiceTests
{
	[TestClass]
	public class RezervationServiceTests : BaseTestInitialization
	{
		[TestInitialize]
		public override void InitializeTest()
		{
			this.SetupRezervationService();
		}
		[TestMethod]
		public void CreateRezervationTest()
		{
			var createRezervationParameters = new RezervationCreationParameters
			{
				PickUpDate = DateTime.Today.AddDays(1), // set to Today for simplicity.
				ReturnDate = DateTime.Today.AddDays(5),
				CarPlateNumber = "CA1234AC",
				CarType = CarTypeEnum.Family,
				ClientAccount = new ClientAccountCreationParams
				{
					Email = "client_rez_1@mail.com",
					FullName = "Client_Rez_1",
					Phone = "+12345",
				},
			};

			using (var context = new CarRentalDbContext(this.dbContextOptions))
			{
				var clientAccountService = new ClientAccountService(context);
				var rezervationService = new RezervationService(context, clientAccountService);

				var rezervationModel = rezervationService.CreateBooking(createRezervationParameters);

				// Sanity check
				Assert.IsTrue(rezervationModel.RezervationId > 0);
				Assert.AreEqual(rezervationModel.PickUpDate, createRezervationParameters.PickUpDate);
				Assert.AreEqual(rezervationModel.ReturnDate, createRezervationParameters.ReturnDate);
				Assert.AreEqual(rezervationModel.CarPlateNumber, createRezervationParameters.CarPlateNumber);
				Assert.AreEqual(rezervationModel.CarType, createRezervationParameters.CarType);
				Assert.AreEqual(rezervationModel.IsCancelled, false);
				Assert.AreEqual(rezervationModel.IsPickedUp, false);
				Assert.AreEqual(rezervationModel.IsReturned, false);

				// Fee calculation.
				var carType = CarTypes.GetCarType(createRezervationParameters.CarType);

				// Test the actual calculations in the car type class.
				var rentalFee = carType.RentalRateFee * (decimal)(createRezervationParameters.ReturnDate - createRezervationParameters.PickUpDate).TotalHours;
				var depositFee = rentalFee * (carType.DepositFeePercentage / 100);

				Assert.AreEqual(rezervationModel.RentaltFee, rentalFee);
				Assert.AreEqual(rezervationModel.DepositFee, depositFee);
				Assert.IsNull(rezervationModel.CancelationFeeRate);				
				
				// validate thrown exceptions
				try
				{
					rezervationService.CreateBooking(null);
					Assert.Fail();
				}
				catch(ArgumentNullException)
				{
				}
				catch(Exception)
				{
					Assert.Fail();
				}

				try
				{
					rezervationService.CreateBooking(new RezervationCreationParameters());
					Assert.Fail();
				}
				catch (InvalidOperationException e)
				{
					Assert.AreEqual("Invalid parameters.", e.Message);
				}
				catch (Exception)
				{
					Assert.Fail();
				}
			}
		}

		[TestMethod]
		public void PickUpCarTest()
		{
			using (var context = new CarRentalDbContext(this.dbContextOptions))
			{
				var dbRezervation = context.Rezervations.First();
				Assert.IsFalse(dbRezervation.IsPickedUp);

				var clientAccountService = new ClientAccountService(context);
				var rezervationService = new RezervationService(context, clientAccountService);

				var isPickedUp = rezervationService.PickUpCar(dbRezervation.RezervationId);

				Assert.IsTrue(isPickedUp);

				dbRezervation = context.Rezervations.First();
				Assert.IsTrue(dbRezervation.IsPickedUp);
			}
		}

		[TestMethod]
		public void ReturnCarTest()
		{
			using (var context = new CarRentalDbContext(this.dbContextOptions))
			{
				var dbRezervation = context.Rezervations.First(x => x.IsPickedUp && !x.IsReturned);

				var clientAccountService = new ClientAccountService(context);
				var rezervationService = new RezervationService(context, clientAccountService);

				var isReturned = rezervationService.ReturnCar(dbRezervation.RezervationId);

				Assert.IsTrue(isReturned);

				dbRezervation = context.Rezervations.Single(x => x.RezervationId == dbRezervation.RezervationId);
				Assert.IsTrue(dbRezervation.IsReturned);
			}
		}

		[TestMethod]
		public void CancelRezervationTest()
		{
			using (var context = new CarRentalDbContext(this.dbContextOptions))
			{
				var dbRezervation = context.Rezervations.First(x => !x.IsPickedUp && !x.IsReturned);

				var clientAccountService = new ClientAccountService(context);
				var rezervationService = new RezervationService(context, clientAccountService);

				var cancelationFeeRate = 2.00m;

				var isCancelled = rezervationService.CancelRezervation(dbRezervation.RezervationId, cancelationFeeRate);

				Assert.IsTrue(isCancelled);

				var carType = CarTypes.GetCarType((CarTypeEnum)dbRezervation.CarType);

				// Test the actual calculations in the car type class.
				var cancellationFee = carType.CancellationFee * cancelationFeeRate;				

				dbRezervation = context.Rezervations.Single(x => x.RezervationId == dbRezervation.RezervationId);
				Assert.IsTrue(dbRezervation.IsCancelled);
				Assert.AreEqual(dbRezervation.CancellationFee, cancellationFee);
				Assert.AreEqual(dbRezervation.CancelationFeeRate, cancelationFeeRate);
			}
		}
	}
}
