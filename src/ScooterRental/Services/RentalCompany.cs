using System.Linq;
using ScooterRental.Interfaces;

namespace ScooterRental.Services
{
    public class RentalCompany : IRentalCompany
    {
        private readonly IRentalService _rentalService;
        private readonly IScooterService _scooterService;
        private readonly IRentalCalculator _rentalCalculator;
        
        public string Name { get; }

        public RentalCompany(string name, 
            IRentalService rentalService, 
            IScooterService scooterService, 
            IRentalCalculator rentalCalculator)
        {
            Name = name;
            _rentalService = rentalService;
            _scooterService = scooterService;
            _rentalCalculator = rentalCalculator;
        }

        /// <inheritdoc />
        public decimal CalculateIncome(int? year, bool includeNotCompletedRentals)
        {
            var scooters = _scooterService.GetScooters();

            return scooters.Sum(scooter =>
            {
                var rentals = _rentalService.GetRentalsByScooterId(scooter.Id).AsEnumerable();

                if (!includeNotCompletedRentals)
                {
                    rentals = rentals.Where(x => x.RentalEnd.HasValue);
                }

                if (year.HasValue)
                {
                    //this would raise a question for product owner - what should we count towards given year?
                    //I chose start because it's easiest
                    rentals = rentals.Where(x => x.RentalStart.Year.Equals(year));
                }

                return rentals.Sum(x =>
                    _rentalCalculator.CalculateScooterRentalPrice(x.RentalStart, x.RentalEnd, scooter.PricePerMinute));
            });
        }

        /// <inheritdoc />
        public decimal EndRent(string id)
        {
            var rental = _rentalService.EndRentalByScooterId(id);
            var scooter = _scooterService.GetScooterById(id);
            return _rentalCalculator.CalculateScooterRentalPrice(rental.RentalStart, rental.RentalEnd, scooter.PricePerMinute);
        }

        /// <inheritdoc />
        public void StartRent(string id)
        {
            _rentalService.StartRentalByScooterId(id);
        }
    }
}
