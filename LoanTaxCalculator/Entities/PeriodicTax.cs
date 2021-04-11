using System;

namespace LoanTaxCalculator.Entities
{
    public class PeriodicTax
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public int TaxTypeId { get; set; }
        public TaxType TaxType { get; set; }

        public DateTime ForMonth { get; set; } //duration, measuredAt
        public decimal? Cost { get; set; }

        public TaxMeasurement Measurement { get; set; } //null
    }
}
