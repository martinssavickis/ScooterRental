using System;

namespace ScooterRental.Exceptions
{
    public class ScooterNotFoundException : Exception
    {
        public ScooterNotFoundException(string id) : base($"Scooter with ID: {id} not found")
        {
            
        }
    }
}