using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace LoanTaxCalculator.Entities
{
    public class User : IdentityUser
    {
        public ICollection<PeriodicTax> PeriodicTaxes { get; set; }
    }
}
