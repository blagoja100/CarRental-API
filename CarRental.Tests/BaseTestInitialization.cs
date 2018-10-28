using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using CarRental.Data.Entities;
using CarRental.Data;
using System.Linq;

namespace CarRental.Tests
{
	[TestClass]
	public abstract class BaseTestInitialization
	{
		protected SqliteConnection connection;
		protected DbContextOptions<CarRentalDbContext> dbContextOptions;

		public BaseTestInitialization()
		{
			this.connection = new SqliteConnection("DataSource=:memory:");
			this.connection.Open();

			this.dbContextOptions = new DbContextOptionsBuilder<CarRentalDbContext>()
				.UseSqlite(connection).Options;

			// Create the schema in the database
			using (var context = new CarRentalDbContext(this.dbContextOptions))
			{
				context.Database.EnsureCreated();
			}
		}

		[TestInitialize]
		public abstract void InitializeTest();

		public void SetupClientService()
		{		
			// Add data to the database.
			using (var context = new CarRentalDbContext(this.dbContextOptions))
			{
				context.ClientAccounts.Add(new ClientAccount
				{
					Email = "client1@mail.com",
					FullName = "Client Name 1",
					Phone = "+11111",
				});

				context.ClientAccounts.Add(new ClientAccount
				{
					Email = "client2@mail.com",
					FullName = "Client Name 2",
					Phone = "+22222",
				});
				context.SaveChanges();
			}
		}

		public void SetupRezervationService()
		{
			this.SetupClientService();

			using (var context = new CarRentalDbContext(this.dbContextOptions))
			{
				var client1 = context.ClientAccounts.First();
				var client2 = context.ClientAccounts.Last();

				context.Rezervations.Add(new Rezervation
				{
					PickUpDate = DateTime.Today.AddDays(1), // set to Today for simplicity.
					ReturnDate = DateTime.Today.AddDays(5),
					CarPlateNumber = "CA1234AC",
					CarType = 2,
					RentaltFee = 1152.00m,
					DepositFee = 138.24m,					
					ClientId = client1.ClientId,
				});

				context.Rezervations.Add(new Rezervation
				{
					PickUpDate = DateTime.Today.AddDays(1), // set to Today for simplicity.
					ReturnDate = DateTime.Today.AddDays(5),
					CarPlateNumber = "CA1234AC",
					CarType = 2,
					RentaltFee = 1152.00m,
					DepositFee = 138.24m,
					IsPickedUp = true,
					IsReturned = true,
					ClientId = client1.ClientId,

				});

				context.Rezervations.Add(new Rezervation
				{
					PickUpDate = DateTime.Today.AddDays(1), // set to Today for simplicity.
					ReturnDate = DateTime.Today.AddDays(5),
					CarPlateNumber = "BT1234UC",
					CarType = 1,
					RentaltFee = 50.00m,
					DepositFee = 2.5m,
					IsPickedUp = true,
					ClientId = client2.ClientId,

				});

				context.Rezervations.Add(new Rezervation
				{
					PickUpDate = DateTime.Today.AddDays(1), // set to Today for simplicity.
					ReturnDate = DateTime.Today.AddDays(2),
					CarPlateNumber = "SD1234T",
					CarType = 3,
					RentaltFee = 50.00m,
					DepositFee = 3.5m,
					IsCancelled = true,
					CancelationFeeRate = 2,
					CancellationFee = 50.0m,
					ClientId = client2.ClientId,
				});

				context.Rezervations.Add(new Rezervation
				{
					PickUpDate = DateTime.Today.AddDays(1), // set to Today for simplicity.
					ReturnDate = DateTime.Today.AddDays(5),
					CarPlateNumber = "CA1234AC",
					CarType = 2,
					RentaltFee = 1152.00m,
					DepositFee = 138.24m,
					IsPickedUp = true,
					IsReturned = true,
					ClientId = client1.ClientId,

				});

				context.SaveChanges();
			}
		}

		[TestCleanup]
		public void Finish()
		{
			this.connection.Close();
		}
	}
}
