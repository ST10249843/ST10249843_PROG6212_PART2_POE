using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace CMCS.Models
{
    public class ViewClaim
    {
        public int Id { get; set; }

        [Required]
        public DateTime DateOfSubmission { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Hours Worked must be a positive number.")]
        public int HoursWorked { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Hourly Rate must be a positive number.")]
        public double HourlyRate { get; set; }

        public double TotalAmount { get; set; }

        [Required]
        public IFormFile SupportingDocument { get; set; }

        // Add the Status property
        public string Status { get; set; }
    }
}
