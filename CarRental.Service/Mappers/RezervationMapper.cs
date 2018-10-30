using CarRental.Data.Entities;
using CarRental.Domain.Models;
using static CarRental.Domain.Constants;

namespace CarRental.Service.Mappers
{
	/// <summary>
	/// Maps the rezervation model to the rezervation entity.
	/// </summary>
	public static class RezervationMapper
	{
		public static RezervationModel ToModel(this Rezervation dbRezervation)
		{
			return new RezervationModel
			{
				CancelationFeeRate = dbRezervation.CancelationFeeRate,
				CancellationFee = dbRezervation.CancellationFee,
				CarPlateNumber = dbRezervation.CarPlateNumber,
				CarType = (CarTypeEnum)dbRezervation.CarType,
				DepositFee = dbRezervation.DepositFee,
				IsCancelled = dbRezervation.IsCancelled,
				IsPickedUp = dbRezervation.IsPickedUp,
				IsReturned = dbRezervation.IsReturned,
				PickUpDate = dbRezervation.PickUpDate,
				RentaltFee = dbRezervation.RentaltFee,
				ReturnDate = dbRezervation.ReturnDate,
				RezervationId = dbRezervation.RezervationId,
			};
		}

		public static RezervationModel ToModel(this Rezervation dbRezervation, ClientAccountModel clientAccountModel)
		{
			var model = dbRezervation.ToModel();
			model.ClientAccount = clientAccountModel;

			return model;
		}
	}
}