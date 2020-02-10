using System;
using ScooterRental.Entities;

namespace ScooterRental.Interfaces
{
    public interface IRentalCalculator
    {
        /// <summary>
        /// Calculates rental price for given time period and rate
        /// </summary>
        /// <param name="start">Start time</param>
        /// <param name="end">End time</param>
        /// <param name="pricePerMinute">Price per minute</param>
        /// <returns>Price for rental period</returns>
        decimal CalculateScooterRentalPrice(DateTime start, DateTime? end, decimal pricePerMinute);
    }
}