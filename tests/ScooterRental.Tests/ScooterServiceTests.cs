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
    public class ScooterServiceTests
    {
        private readonly Mock<IScooterRepository> _scooterRepositoryMock = new Mock<IScooterRepository>();
        
        private readonly IScooterService _scooterService;
        
        public ScooterServiceTests()
        {
            _scooterService = new ScooterService(_scooterRepositoryMock.Object);
        }
        
        [Fact]
        public void AddScooter_ScooterExists_ThrowsException()
        {
            _scooterRepositoryMock.Setup(x => x.GetById(It.IsAny<string>())).Returns(new Scooter("id", 1));

            _scooterService.Invoking(x => x.AddScooter("id", 1)).Should().Throw<DuplicateScooterException>();
        }

        [Fact]
        public void AddScooter_NewScooter_CallsRepositoryWithCorrectScooter()
        {
            _scooterService.AddScooter("id", 1);
            
            _scooterRepositoryMock.Verify(x => x.CreateOrUpdate(
                It.Is<Scooter>(y => y.Id.Equals("id") && y.PricePerMinute == 1)));
        }
        
        [Fact]
        public void GetScooterById_ScooterNotFound_ThrowsException()
        {
            _scooterService.Invoking(x => x.GetScooterById("id")).Should().Throw<ScooterNotFoundException>();
        }
        
        [Fact]
        public void GetScooterById_WithValidId_ReturnsScooter()
        {
            var scooter = new Scooter("id", 1);
            _scooterRepositoryMock.Setup(x => x.GetById("id")).Returns(scooter);

            var result = _scooterService.GetScooterById("id");

            _scooterRepositoryMock.Verify(x => x.GetById("id"));
            result.Should().Be(scooter);
        }
        
        [Fact]
        public void GetScooters_WhenCalled_ReturnsAllScooters()
        {
            var scooters = new List<Scooter>
            {
                new Scooter("id1", 1),
                new Scooter("id2", 2)
            };

            _scooterRepositoryMock.Setup(x => x.GetAll()).Returns(scooters);

            var result = _scooterService.GetScooters();

            _scooterRepositoryMock.Verify(x => x.GetAll());
            result.Should().BeSameAs(scooters);
        }
        
        [Fact]
        public void RemoveScooter_NotFound_ThrowsNotFoundException()
        {
            _scooterService.Invoking(x => x.RemoveScooter("id")).Should().Throw<ScooterNotFoundException>();
        }
        
        [Fact]
        public void RemoveScooter_ScooterIsRented_ThrowsInvalidOperationException()
        {
            var scooter = new Scooter("id", 1) {IsRented = true};
            _scooterRepositoryMock.Setup(x => x.GetById("id")).Returns(scooter);

            _scooterService.Invoking(x => x.RemoveScooter("id")).Should().Throw<InvalidScooterOperationException>();
        }
        
        [Fact]
        public void RemoveScooter_ScooterNotRented_CallsRemove()
        {
            var scooter = new Scooter("id", 1);
            _scooterRepositoryMock.Setup(x => x.GetById("id")).Returns(scooter);

            _scooterService.RemoveScooter("id");
            
            _scooterRepositoryMock.Verify(x => x.Remove("id"));
        }
    }
}
