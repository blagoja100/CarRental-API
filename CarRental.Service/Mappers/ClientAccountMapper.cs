using CarRental.Data.Entities;
using CarRental.Domain.Models;

namespace CarRental.Service.Mappers
{
	/// <summary>
	/// Maps the properties from the ClientAccountModel to the ClientAccount entity
	/// </summary>
	public static class ClientAccountMapper
	{
		/// <summary>
		/// Creates a client account model from the entity.
		/// </summary>
		/// <param name="dbClientAccount"></param>
		/// <returns></returns>
		public static ClientAccountModel ToModel(this ClientAccount dbClientAccount)
		{
			return new ClientAccountModel
			{
				ClientId = dbClientAccount.ClientId,
				Email = dbClientAccount.Email,
				FullName = dbClientAccount.FullName,
				Phone = dbClientAccount.Phone,
			};
		}
	}
}