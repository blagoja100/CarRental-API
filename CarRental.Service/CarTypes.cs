using System.Collections.Generic;
using System.Linq;
using System.Data.Common;
using static CarRental.Domain.Constants;
using System;

namespace CarRental.Service
{
	/// <summary>
	/// Creates a specific car type.
	/// </summary>
	public static class CarTypes
	{
		private static Dictionary<CarTypeEnum, CarType> carTypes = new Dictionary<CarTypeEnum, CarType>();

		/// <summary>
		/// Creates a new instance of the car type factory.
		/// </summary>
		static CarTypes()
		{
			carTypes.Add(CarTypeEnum.Standard, new CarType { Type = CarTypeEnum.Standard, Name = "Standard Car Type", RentalRateFee = 10.00m, CancellationFee = 5.00m, DepositFeePercentage = 10m });
			carTypes.Add(CarTypeEnum.Family, new CarType { Type = CarTypeEnum.Family, Name = "Family Car Type", RentalRateFee = 12.00m, CancellationFee = 7.00m, DepositFeePercentage = 12m });
			carTypes.Add(CarTypeEnum.Prestiege, new CarType { Type = CarTypeEnum.Prestiege, Name = "Prestiege Car Type", RentalRateFee = 50.00m, CancellationFee = 25.00m, DepositFeePercentage = 70m });
		}

		public static CarType GetCarType(CarTypeEnum type) => carTypes[type];
	}

	/// <summary>
	/// Holds information on the car types.
	/// </summary>
	public class CarType
	{
		public CarTypeEnum Type { get; set; }
		public string Name { get; set; }
		public decimal RentalRateFee { get; set; }
		public decimal CancellationFee { get; set; }
		public decimal DepositFeePercentage { get; set; }

		/// <summary>
		/// Calculates the rental fee.
		/// </summary>
		/// <param name="pickUpDate"></param>
		/// <param name="returnDate"></param>
		/// <returns></returns>
		public decimal GetRentalFee(DateTime pickUpDate, DateTime returnDate)
		{
			return this.RentalRateFee * (decimal)(returnDate - pickUpDate).TotalHours;
		}

		/// <summary>
		/// Calculates the deposit fee for the deposit fee percentage.
		/// </summary>
		/// <param name="rentalFee"></param>
		/// <returns></returns>
		public decimal GetDepositFee(decimal rentalFee)
		{
			return rentalFee * (this.DepositFeePercentage / 100);
		}

		/// <summary>
		/// Calculates the cancelation fee based on the cancellation fee for the car type and the cancelation fee rate.
		/// </summary>
		/// <param name="cancelationFeeRate"></param>
		/// <returns></returns>
		public decimal GetCancellationFee(decimal cancelationFeeRate)
		{
			return cancelationFeeRate * this.CancellationFee;
		}
	}
}