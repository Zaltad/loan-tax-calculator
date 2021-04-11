using System;

namespace LoanTaxCalculator.Dtos.Requests
{
    public class CreatePeriodicTaxRequest
    {
        public int TaxTypeId { get; set; }
        public DateTime ForMonth { get; set; }
        public decimal? Cost { get; set; }
        public TaxMeasurementDto Measurement { get; set; }
    }
}