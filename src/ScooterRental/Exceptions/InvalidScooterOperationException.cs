using System;

namespace ScooterRental.Exceptions
{
    public class InvalidScooterOperationException : Exception
    {
        public InvalidScooterOperationException(string id) : base($"Attempting invalid operation on scooter with ID: {id}")
        {
            
        }
    }
}