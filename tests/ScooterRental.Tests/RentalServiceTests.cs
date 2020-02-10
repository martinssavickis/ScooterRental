using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using ScooterRental.Entities;
using ScooterRental.Exceptions;
using ScooterRental.Interfaces;
using ScooterRental.Services;
using Xunit;

namespace ScooterRental.Tests
{
    public class RentalServiceTests
    {
        private readonly Mock<IRentalRepository> _rentalRepositoryMock = new Mock<IRentalRepository>();
        private readonly Mock<IScooterRepository> _scooterRepositoryMock = new Mock<IScooterRepository>();
        
        private readonly RentalService _rentalService;

        public RentalServiceTests()
        {
            _rentalService = new RentalService(_scooterRepositoryMock.Object, _rentalRepositoryMock.Object);
        }
        
        [Fact]
        public void StartRentalByScooterId_ScooterNotFound_ThrowsScooterNotFoundException()
        {
            _rentalService.Invoking(x => x.StartRentalByScooterId("id")).Should().Throw<ScooterNotFoundException>();
        }
        
        [Fact]
        public void StartRentalByScooterId_ScooterAlreadyRented_ThrowsInvalidScooterOperationException()
        {
            var scooter = new Scooter("id", 1) {IsRented = true};
            _scooterRepositoryMock.Setup(x => x.GetById("id")).Returns(scooter);

            _rentalService.Invoking(x => x.StartRentalByScooterId("id"))
                .Should().Throw<InvalidScooterOperationException>();
        }
        
        [Fact]
        public void StartRentalByScooterId_RentalNotFinished_ThrowsScooterNotFoundException()
        {
            var scooter = new Scooter("id", 1);
            var rental = new Rental();
            _scooterRepositoryMock.Setup(x => x.GetById("id")).Returns(scooter);
            _rentalRepositoryMock.Setup(x => x.GetLastRentalByScooterId("id")).Returns(rental);
            
            _rentalService.Invoking(x => x.StartRentalByScooterId("id"))
                .Should().Throw<InvalidScooterOperationException>();
        }
        
        [Fact]
        public void StartRentalByScooterId_ScooterAvailable_CreatesRentalUpdatesScooter()
        {
            var scooter = new Scooter("id", 1);
            var rental = new Rental {RentalEnd = DateTime.UtcNow.Date};
            _scooterRepositoryMock.Setup(x => x.GetById("id")).Returns(scooter);
            _rentalRepositoryMock.Setup(x => x.GetLastRentalByScooterId("id")).Returns(rental);
            
            _rentalService.StartRentalByScooterId("id");
            
            _rentalRepositoryMock.Verify(x => 
                x.CreateOrUpdate(It.Is<Rental>(y => !y.RentalEnd.HasValue)));
            _scooterRepositoryMock.Verify(x => x.CreateOrUpdate(It.Is<Scooter>(y => y.IsRented)));
        }
        
        [Fact]
        public void EndRentalByScooterId_ScooterNotFound_ThrowsScooterNotFoundException()
        {
            _rentalService.Invoking(x => x.EndRentalByScooterId("id")).Should().Throw<ScooterNotFoundException>();
        }
        
        [Fact]
        public void EndRentalByScooterId_ScooterNotRented_ThrowsInvalidScooterOperationException()
        {
            var scooter = new Scooter("id", 1);
            _scooterRepositoryMock.Setup(x => x.GetById("id")).Returns(scooter);

            _rentalService.Invoking(x => x.EndRentalByScooterId("id"))
                .Should().Throw<InvalidScooterOperationException>();
        }
        
        [Fact]
        public void EndRentalByScooterId_RentalNotStarted_ThrowsScooterNotFoundException()
        {
            var scooter = new Scooter("id", 1);
            var rental = new Rental{RentalEnd = DateTime.UtcNow.Date};
            _scooterRepositoryMock.Setup(x => x.GetById("id")).Returns(scooter);
            _rentalRepositoryMock.Setup(x => x.GetLastRentalByScooterId("id")).Returns(rental);
            
            _rentalService.Invoking(x => x.EndRentalByScooterId("id"))
                .Should().Throw<InvalidScooterOperationException>();
        }
        
        [Fact]
        public void EndRentalByScooterId_ScooterRented_UpdatesScooterAndRental()
        {
            var scooter = new Scooter("id", 1) {IsRented = true};
            var rental = new Rental();
            _scooterRepositoryMock.Setup(x => x.GetById("id")).Returns(scooter);
            _rentalRepositoryMock.Setup(x => x.GetLastRentalByScooterId("id")).Returns(rental);
            
            _rentalService.EndRentalByScooterId("id");
            
            _rentalRepositoryMock.Verify(x => x.CreateOrUpdate(It.Is<Rental>(y => y.RentalEnd.HasValue)));
            _scooterRepositoryMock.Verify(x => x.CreateOrUpdate(It.Is<Scooter>(y => !y.IsRented)));
        }
        
        [Fact]
        public void GetRentalsByScooterId_ScooterNotFound_ThrowsScooterNotFoundException()
        {
            _rentalService.Invoking(x => x.GetRentalsByScooterId("id")).Should().Throw<ScooterNotFoundException>();
        }
        
        [Fact]
        public void GetRentalsByScooterId_ScooterExists_ReturnsRentalsFromRepository()
        {
            var scooter = new Scooter("id", 1);
            var rentals = new List<Rental>
            {
                new Rental(),
                new Rental()
            };
            _rentalRepositoryMock.Setup(x => x.GetRentalsByScooterId("id")).Returns(rentals);
            _scooterRepositoryMock.Setup(x => x.GetById("id")).Returns(scooter);

            var result = _rentalService.GetRentalsByScooterId("id");

            result.Should().BeSameAs(rentals);
        }
    }
}