using System.ComponentModel.DataAnnotations;

namespace LoanTaxCalculator.Dtos.Requests
{
    public class AuthenticateModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
