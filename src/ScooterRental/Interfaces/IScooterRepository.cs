using System;
using System.Collections.Generic;
using System.Text;
using ScooterRental.Entities;

namespace ScooterRental.Interfaces
{
    /// <summary>
    /// Repository for <see cref="Scooter"/> entities
    /// </summary>
    public interface IScooterRepository
    {
        /// <summary>
        /// Get All scooters
        /// </summary>
        /// <returns>All Scooters</returns>
        IList<Scooter> GetAll();

        /// <summary>
        /// Get Scooter by ID
        /// </summary>
        /// <param name="id">Scooter Id</param>
        /// <returns>Scooter</returns>
        Scooter GetById(string id);

        /// <summary>
        /// Create or Update Scooter
        /// </summary>
        /// <param name="scooter">Scooter to add or update</param>
        /// <returns>Scooter</returns>
        Scooter CreateOrUpdate(Scooter scooter);

        /// <summary>
        /// Remove scooter
        /// </summary>
        /// <param name="id">Scooter Id</param>
        void Remove(string id);
    }
}
