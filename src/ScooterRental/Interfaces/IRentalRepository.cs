using System.Collections.Generic;
using ScooterRental.Entities;

namespace ScooterRental.Interfaces
{
    public interface IRentalRepository
    {
        /// <summary>
        /// Get Rentals by scooter Id
        /// </summary>
        /// <param name="scooterId">Scooter Id</param>
        /// <returns>Rentals</returns>
        IList<Rental> GetRentalsByScooterId(string scooterId);

        /// <summary>
        /// Get last rental by scooter Id
        /// </summary>
        /// <param name="scooterId">Scooter Id</param>
        /// <returns>Scooter Rental</returns>
        Rental GetLastRentalByScooterId(string scooterId);

        /// <summary>
        /// Create or update rental
        /// </summary>
        /// <param name="rental"></param>
        /// <returns>Created rental</returns>
        Rental CreateOrUpdate(Rental rental);
    }
}