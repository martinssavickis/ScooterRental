using System;
using FluentAssertions;
using ScooterRental.Services;
using Xunit;

namespace ScooterRental.Tests
{
    public class RentalCalculatorTests
    {
        private readonly RentalCalculator _rentalCalculator = new RentalCalculator();
        
        [Fact]
        public void CalculateScooterRentalPrice_InvalidDates_ThrowsArgumentException()
        {
            var start = new DateTime(2020, 1, 1, 17, 15, 15);
            var end = new DateTime(2020, 1, 1, 15, 15, 15);

            _rentalCalculator.Invoking(x => x.CalculateScooterRentalPrice(start, end, 1))
                .Should().Throw<ArgumentException>();

        }
        
        [Fact]
        public void CalculateScooterRentalPrice_SameDayUnderCap_CalculatesRentalPrice()
        {
            var start = new DateTime(2020, 1, 1, 17, 15, 15);
            var end = new DateTime(2020, 1, 1, 17, 25, 15);

            var result = _rentalCalculator.CalculateScooterRentalPrice(start, end, 1);

            result.Should().Be(10);
        }
        
        [Fact]
        public void CalculateScooterRentalPrice_SameDayUnderCap_CalculatesToFullMinutes()
        {
            var start = new DateTime(2020, 1, 1, 17, 15, 15);
            var end = new DateTime(2020, 1, 1, 17, 25, 55);

            var result = _rentalCalculator.CalculateScooterRentalPrice(start, end, 1);

            result.Should().Be(10);
        }
        
        [Fact]
        public void CalculateScooterRentalPrice_SameDayOverCap_CapsRentalPrice()
        {
            var start = new DateTime(2020, 1, 1, 17, 15, 15);
            var end = new DateTime(2020, 1, 1, 17, 55, 15);

            var result = _rentalCalculator.CalculateScooterRentalPrice(start, end, 1);

            result.Should().Be(20);
        }
        
        [Fact]
        public void CalculateScooterRentalPrice_FirstDayUnderNextOverCap_CapsRentalPriceCorrectly()
        {
            var start = new DateTime(2020, 1, 1, 23, 55, 0);
            var end = new DateTime(2020, 1, 2, 17, 55, 15);

            var result = _rentalCalculator.CalculateScooterRentalPrice(start, end, 1);

            result.Should().Be(25);
        }
        
        [Fact]
        public void CalculateScooterRentalPrice_FirstDayOverNextUnderCap_CapsRentalPriceCorrectly()
        {
            var start = new DateTime(2020, 1, 1, 17, 55, 0);
            var end = new DateTime(2020, 1, 2, 0, 10, 15);

            var result = _rentalCalculator.CalculateScooterRentalPrice(start, end, 1);

            result.Should().Be(30);
        }
        
        [Fact]
        public void CalculateScooterRentalPrice_FirstLastDayUnderCap_CapsRentalPriceCorrectly()
        {
            var start = new DateTime(2020, 1, 1, 23, 55, 0);
            var end = new DateTime(2020, 1, 4, 0, 10, 15);

            var result = _rentalCalculator.CalculateScooterRentalPrice(start, end, 1);

            result.Should().Be(55);
        }
        
        [Fact]
        public void CalculateScooterRentalPrice_AllDaysOverCap_CapsRentalPriceCorrectly()
        {
            var start = new DateTime(2020, 1, 1, 22, 55, 0);
            var end = new DateTime(2020, 1, 4, 22, 10, 15);

            var result = _rentalCalculator.CalculateScooterRentalPrice(start, end, 1);

            result.Should().Be(80);
        }
        
    }
}