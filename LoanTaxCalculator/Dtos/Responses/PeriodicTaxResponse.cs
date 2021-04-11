using System;

namespace LoanTaxCalculator.Dtos.Requests
{
    public class PeriodicTaxResponse
    {
        public TaxTypeDto TaxType { get; set; }
        public DateTime ForMonth { get; set; }
        public decimal? Cost { get; set; }
        public TaxMeasurementDto Measurement { get; set; }
    }
}