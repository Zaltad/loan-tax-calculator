using System;

namespace LoanTaxCalculator.Entities
{
    public class PeriodicTaxMeasurementException : Exception
    {
        public PeriodicTaxMeasurementException(string message) : base(message)
        {
        }
    }
}
