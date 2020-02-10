using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using ScooterRental.Entities;
using ScooterRental.Interfaces;
using ScooterRental.Services;
using Xunit;

namespace ScooterRental.Tests
{
    public class RentalCompanyTests
    {
        private readonly Mock<IScooterService> _scooterServiceMock = new Mock<IScooterService>();
        private readonly Mock<IRentalService> _rentalServiceMock = new Mock<IRentalService>();
        private readonly Mock<IRentalCalculator> _rentalCalculatorMock = new Mock<IRentalCalculator>();

        private readonly RentalCompany _rentalCompany;

        public RentalCompanyTests()
        {
            _rentalCompany = new RentalCompany("test", _rentalServiceMock.Object, _scooterServiceMock.Object, _rentalCalculatorMock.Object );
        }

        [Fact]
        public void StartRent_WhenCalled_CallsRentalService()
        {
            _rentalCompany.StartRent("id");
            
            _rentalServiceMock.Verify(x => x.StartRentalByScooterId("id"));
        }

        [Fact]
        public void EndRent_WhenCalled_CallsScooterRentalCalcServices()
        {
            _scooterServiceMock.Setup(x => x.GetScooterById("id")).Returns(new Scooter("id", 1));
            _rentalServiceMock.Setup(x => x.EndRentalByScooterId("id")).Returns(new Rental());
            _rentalCalculatorMock.Setup(x =>
                    x.CalculateScooterRentalPrice(It.IsAny<DateTime>(), It.IsAny<DateTime?>(), It.IsAny<decimal>()))
                .Returns(5);

            _rentalCompany.EndRent("id");
            
            _scooterServiceMock.Verify(x => x.GetScooterById("id"));
            _rentalServiceMock.Verify(x => x.EndRentalByScooterId("id"));
            _rentalCalculatorMock.Verify(x => x.CalculateScooterRentalPrice(It.IsAny<DateTime>(), It.IsAny<DateTime?>(), 1));
        }

        [Fact]
        public void CalculateIncome_GivenYear_FiltersOtherRentals()
        {
            var scooters = new List<Scooter>
            {
                new Scooter("id1", 1)
            };
            var rentals = new List<Rental>
            {
                new Rental {RentalStart = new DateTime(2020, 1, 1)},
                new Rental {RentalStart = new DateTime(2020, 1,1)},
                new Rental {RentalStart = new DateTime(2019, 12, 31)}
            };
            _scooterServiceMock.Setup(x => x.GetScooters()).Returns(scooters);
            _rentalServiceMock.Setup(x => x.GetRentalsByScooterId("id1")).Returns(rentals);

            _rentalCompany.CalculateIncome(2020, true);
            
            _rentalCalculatorMock.Verify(x => x.CalculateScooterRentalPrice(It.IsAny<DateTime>(), It.IsAny<DateTime?>(), 1), Times.Exactly(2));
        }
        
        [Fact]
        public void CalculateIncome_NotIncludeNotCompleted_FiltersOtherRentals()
        {
            var scooters = new List<Scooter>
            {
                new Scooter("id1", 1)
            };
            var rentals = new List<Rental>
            {
                new Rental {RentalStart = new DateTime(2020, 1, 1), RentalEnd = new DateTime(2020,1,2)},
                new Rental {RentalStart = new DateTime(2020, 1,1)},
                new Rental {RentalStart = new DateTime(2019, 12, 31), RentalEnd = new DateTime(2020,1,2)}
            };
            _scooterServiceMock.Setup(x => x.GetScooters()).Returns(scooters);
            _rentalServiceMock.Setup(x => x.GetRentalsByScooterId("id1")).Returns(rentals);

            _rentalCompany.CalculateIncome(null, false);
            
            _rentalCalculatorMock.Verify(x => x.CalculateScooterRentalPrice(It.IsAny<DateTime>(), It.IsAny<DateTime?>(), 1), Times.Exactly(2));
        }
        
        [Fact]
        public void CalculateIncome_GivenScooterRentals_SumsRentalPrices()
        {
            var scooters = new List<Scooter>
            {
                new Scooter("id1", 1),
                new Scooter("id2", 2)
                
            };
            var rentals1 = new List<Rental>
            {
                new Rental {RentalStart = new DateTime(2020, 1, 1), RentalEnd = new DateTime(2020,1,2)},
                new Rental {RentalStart = new DateTime(2020, 1,1)},
                new Rental {RentalStart = new DateTime(2019, 12, 31), RentalEnd = new DateTime(2020,1,2)}
            };
            var rentals2 = new List<Rental>
            {
                new Rental {RentalStart = new DateTime(2020, 1, 1), RentalEnd = new DateTime(2020,1,2)},
                new Rental {RentalStart = new DateTime(2020, 1,1)},
            };
            
            _scooterServiceMock.Setup(x => x.GetScooters()).Returns(scooters);
            _rentalServiceMock.Setup(x => x.GetRentalsByScooterId("id1")).Returns(rentals1);
            _rentalServiceMock.Setup(x => x.GetRentalsByScooterId("id2")).Returns(rentals2);
           
            _rentalCalculatorMock
                .Setup(x => x.CalculateScooterRentalPrice(It.IsAny<DateTime>(), It.IsAny<DateTime?>(), 1)).Returns(10);
            _rentalCalculatorMock
                .Setup(x => x.CalculateScooterRentalPrice(It.IsAny<DateTime>(), It.IsAny<DateTime?>(), 2)).Returns(20);
            
            var result = _rentalCompany.CalculateIncome(null, false);

            result.Should().Be(40);
        }

    }
}