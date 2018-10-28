using System;
using System.Collections.Generic;
using System.Linq;
using CarRental.Data;
using CarRental.Domain.Parameters;
using CarRental.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CarRental.Tests.ServiceTests
{
	[TestClass]
	public class ClientAccountServiceTests : BaseTestInitialization
	{
		[TestInitialize]
		public override void InitializeTest()
		{
			this.SetupClientService();
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

				try
				{
					service.Add(null);
					Assert.Fail();
				}
				catch (ArgumentNullException)
				{
				}
				catch
				{
					Assert.Fail();
				}

				try
				{
					service.Add(new ClientAccountCreationParams());
					Assert.Fail();
				}
				catch (InvalidOperationException)
				{
				}
				catch
				{
					Assert.Fail();
				}
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

				try
				{
					service.Update(null);
					Assert.Fail();
				}
				catch (ArgumentNullException)
				{
				}
				catch
				{
					Assert.Fail();
				}

				try
				{
					service.Update(new ClientAccountModificationParams());
					Assert.Fail();
				}
				catch (InvalidOperationException)
				{
				}
				catch
				{
					Assert.Fail();
				}
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
	
				clientAccountModel = service.Get(-1);
				Assert.IsNull(clientAccountModel);				
			}
		}
	}
}
