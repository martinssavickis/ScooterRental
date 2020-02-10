using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScooterRental.Entities;
using ScooterRental.Exceptions;
using ScooterRental.Interfaces;

namespace ScooterRental.Services
{
    public class ScooterService : IScooterService
    {
        private readonly IScooterRepository _scooterRepository;

        public ScooterService(IScooterRepository scooterRepository)
        {
            _scooterRepository = scooterRepository;
        }

        /// <inheritdoc />
        public void AddScooter(string id, decimal pricePerMinute)
        {
            var scooter = _scooterRepository.GetById(id);
            if (scooter != null)
            {
                throw new DuplicateScooterException(id);
            }
            
            scooter = new Scooter(id, pricePerMinute);

            _scooterRepository.CreateOrUpdate(scooter);
        }

        /// <inheritdoc />
        public Scooter GetScooterById(string scooterId)
        {
            var scooter = _scooterRepository.GetById(scooterId);

            if (scooter == null)
            {
                throw new ScooterNotFoundException(scooterId);
            }
            
            return scooter;
        }

        /// <inheritdoc />
        public IList<Scooter> GetScooters()
        {
            return _scooterRepository.GetAll();
        }

        /// <inheritdoc />
        public void RemoveScooter(string id)
        {
            var scooter = _scooterRepository.GetById(id);
            if (scooter == null)
            {
                throw new ScooterNotFoundException(id);
            }

            if (scooter.IsRented)
            {
                throw new InvalidScooterOperationException(id);
            }

            _scooterRepository.Remove(id);
        }
    }
}
