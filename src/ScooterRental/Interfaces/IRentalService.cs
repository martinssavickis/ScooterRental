using System.Collections.Generic;
using ScooterRental.Entities;

namespace ScooterRental.Interfaces
{
    public interface IRentalService
    {
        /// <summary>
        /// Start Rental 
        /// </summary>
        /// <param name="scooterId">Scooter Id</param>
        void StartRentalByScooterId(string scooterId);
        
        /// <summary>
        /// End Rental
        /// </summary>
        /// <param name="id">Scooter Id</param>
        /// <returns>Finished Rental</returns>
        Rental EndRentalByScooterId(string id);

        /// <summary>
        /// Get all rentals for scooter
        /// </summary>
        /// <param name="scooterId">Scooter Id</param>
        /// <returns>Rentals</returns>
        IList<Rental> GetRentalsByScooterId(string scooterId);
    }
}