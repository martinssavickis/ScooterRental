using System;
using ScooterRental.Interfaces;

namespace ScooterRental.Services
{
    public class RentalCalculator : IRentalCalculator
    {
        //would be nice to pass as configuration, DI container reads from configuration and passes as argument maybe?
        private const decimal DailyCap = 20;

        /// <inheritdoc />
        public decimal CalculateScooterRentalPrice(DateTime start, DateTime? end, decimal pricePerMinute)
        {
            var effectiveEnd = end ?? DateTime.UtcNow;

            if (start > effectiveEnd)
            {
                throw new ArgumentException("Start date is later than end date");
            }
            
            decimal sum = 0;
            var nextDay = start.Date.AddDays(1);
            
            var currentStart = start;
            var currentEnd = effectiveEnd > nextDay ? nextDay : effectiveEnd;
            while (currentStart < currentEnd)
            {
                //calculate price for day
                var timeSpan = currentEnd - currentStart;
                var dayPrice = (int)timeSpan.TotalMinutes * pricePerMinute;
                sum += dayPrice < DailyCap ? dayPrice : DailyCap;

                //advance to next day
                currentStart = nextDay;
                nextDay = nextDay.AddDays(1);
                currentEnd = effectiveEnd > nextDay ? nextDay : effectiveEnd;
            }
            return sum;
        }
    }
}