using System;
using System.Collections.Generic;
using ScooterRental.Entities;
using ScooterRental.Exceptions;
using ScooterRental.Interfaces;

namespace ScooterRental.Services
{
    public class RentalService : IRentalService
    {
        private readonly IScooterRepository _scooterRepository;
        private readonly IRentalRepository _rentalRepository;

        public RentalService(
            IScooterRepository scooterRepository, 
            IRentalRepository rentalRepository)
        {
            _scooterRepository = scooterRepository;
            _rentalRepository = rentalRepository;
        }

        /// <inheritdoc />
        public void StartRentalByScooterId(string scooterId)
        {
            var scooter = _scooterRepository.GetById(scooterId);

            if (scooter == null)
            {
                throw new ScooterNotFoundException(scooterId);
            }

            if (scooter.IsRented)
            {
                throw new InvalidScooterOperationException(scooterId);
            }

            var previousRental = _rentalRepository.GetLastRentalByScooterId(scooterId);
            if (previousRental != null && !previousRental.RentalEnd.HasValue)
            {
                throw new InvalidScooterOperationException(scooterId);
            }

            scooter.IsRented = true;
            var rental = new Rental
            {
                ScooterId = scooterId,
                RentalStart = DateTime.UtcNow,
                RentalEnd = null
            };

            _rentalRepository.CreateOrUpdate(rental);
            _scooterRepository.CreateOrUpdate(scooter);
        }

        /// <inheritdoc />
        public Rental EndRentalByScooterId(string scooterId)
        {
            var scooter = _scooterRepository.GetById(scooterId);

            if (scooter == null)
            {
                throw new ScooterNotFoundException(scooterId);
                //throw
            }

            if (!scooter.IsRented)
            {
                throw new InvalidScooterOperationException(scooterId);
            }
            
            var rental = _rentalRepository.GetLastRentalByScooterId(scooterId);
            if (rental == null || rental.RentalEnd.HasValue)
            {
                throw new InvalidScooterOperationException(scooterId);
            }
            
            rental.RentalEnd = DateTime.UtcNow;
            scooter.IsRented = false;

            _rentalRepository.CreateOrUpdate(rental);
            _scooterRepository.CreateOrUpdate(scooter);

            return rental;
        }

        /// <inheritdoc />
        public IList<Rental> GetRentalsByScooterId(string scooterId)
        {
            var scooter = _scooterRepository.GetById(scooterId);

            if (scooter == null)
            {
                throw new ScooterNotFoundException(scooterId);
            }
            
            return _rentalRepository.GetRentalsByScooterId(scooterId);
        }
    }
}