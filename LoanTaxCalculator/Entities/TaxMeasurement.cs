namespace LoanTaxCalculator.Entities
{
    public class TaxMeasurement
    {
        public int Id { get; set; }

        public int PeriodicTaxId { get; set; } //?
        public PeriodicTax PeriodicTax { get; set; }

        public decimal UnitPrice { get; set; }
        public double NowIs { get; set; } //decimal
    }
}
