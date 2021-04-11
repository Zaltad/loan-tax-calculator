using System.Collections.Generic;

namespace LoanTaxCalculator.Dtos.Requests
{
    public class PeriodicTaxesResponse
    {
        public List<PeriodicTaxResponse> PeriodicTaxes { get; set; }
    }
}