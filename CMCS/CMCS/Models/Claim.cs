using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CMCS.Models
{
    public class Claim
    {
        public int ID { get; set; }
        [Required] 
        public string Name { get; set; }
        [Required] 
        public string Surname { get; set; }
        [Required]
        public string Module { get; set; }

        public string Status { get; set; } = "Pending";

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Hours worked must be a positive number.")]
        public double HoursWorked { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Hourly rate must be a positive number.")]
        public int HourlyRate { get; set; }

        [Display(Name = "Total Amount (R)")]
        public double TotalAmount { get; private set; } // Make it read-only

        public IFormFile SupportingDocument { get; set; }

        public DateTime DateOfSubmission { get; set; } = DateTime.Now;

        public void CalculateTotalAmount()
        {
            TotalAmount = HoursWorked * HourlyRate;
        }
    }


}
