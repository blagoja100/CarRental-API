using System;
using System.Collections.Generic;
using System.Linq;
using CarRental.Data;
using CarRental.Data.Entities;
using CarRental.Domain.Parameters;
using CarRental.Service;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CarRental.Tests.ClientAccountTests
{
	[TestClass]
	public class ClientAccountServiceTest
	{
		private SqliteConnection connection;
		private DbContextOptions<CarRentalDbContext> dbContextOptions;

		[TestInitialize]
		public void Setup()
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

		[TestCleanup]
		public void Finish()
		{
			this.connection.Close();
		}

		[TestMethod]
		public void AddClientAccountTest()
		{
			using (var context = new CarRentalDbContext(this.dbContextOptions))
			{				
				var service = new ClientAccountService(context);
				ClientAccountCreationParams parameters = new ClientAccountCreationParams
				{
					Email = "Add_Client@mail.com",
					FullName = "Add Client",
					Phone = "+12345",
				};

				var clientAccountModel = service.Add(parameters);
				
				Assert.AreEqual(parameters.Email, clientAccountModel.Email);
				Assert.AreEqual(parameters.Phone, clientAccountModel.Phone);
				Assert.AreEqual(parameters.FullName, clientAccountModel.FullName);
				Assert.IsTrue(clientAccountModel.ClientId > 0);
			}
		}

		[TestMethod]
		public void UpdateClientAccountTest()
		{
			using (var context = new CarRentalDbContext(this.dbContextOptions))
			{
				var service = new ClientAccountService(context);
				ClientAccountModificationParams parameters = new ClientAccountModificationParams
				{
					ClientId = 1,
					Email = "Update_Client@mail.com",
					FullName = "Update Client",
					Phone = "+12345",
				};

				var clientAccountModel = service.Update(parameters);

				Assert.AreEqual(parameters.Email, clientAccountModel.Email);
				Assert.AreEqual(parameters.Phone, clientAccountModel.Phone);
				Assert.AreEqual(parameters.FullName, clientAccountModel.FullName);
				Assert.AreEqual(parameters.ClientId, clientAccountModel.ClientId);
			}
		}

		[TestMethod]
		public void GetClientAccountTest()
		{			
			using (var context = new CarRentalDbContext(this.dbContextOptions))
			{
				var clientAccount = context.ClientAccounts.First();
				var service = new ClientAccountService(context);
				var clientAccountModel = service.Get(clientAccount.ClientId);

				Assert.AreEqual(clientAccount.ClientId, clientAccountModel.ClientId);
				Assert.AreEqual(clientAccount.Email, clientAccountModel.Email);
				Assert.AreEqual(clientAccount.Phone, clientAccountModel.Phone);
				Assert.AreEqual(clientAccount.FullName, clientAccountModel.FullName);
			}
		}
	}
}
