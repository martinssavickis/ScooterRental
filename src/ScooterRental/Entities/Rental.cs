using System;

namespace ScooterRental.Entities
{
    public class Rental
    {
        /// <summary>
        /// Id of the rental
        /// </summary>
        public int RentalId { get; set; } 
        
        /// <summary>
        /// Id of the scooter
        /// </summary>
        public string ScooterId { get; set; }
        
        /// <summary>
        /// Rental start time
        /// </summary>
        public DateTime RentalStart { get; set; }
        
        /// <summary>
        /// Rental end time
        /// </summary>
        public DateTime? RentalEnd { get; set; }
    }
}