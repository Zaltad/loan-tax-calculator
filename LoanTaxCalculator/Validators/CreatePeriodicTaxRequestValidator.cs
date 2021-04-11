using FluentValidation;
using LoanTaxCalculator.Dtos.Requests;
using System;

namespace LoanTaxCalculator.Validators
{
    public class CreatePeriodicTaxRequestValidator : AbstractValidator<CreatePeriodicTaxRequest>
    {
        public CreatePeriodicTaxRequestValidator()
        {
            RuleFor(periodicTax => periodicTax.ForMonth).LessThanOrEqualTo(DateTime.Now);
            RuleFor(periodicTax => periodicTax).Must(periodicTax => periodicTax.Cost == null ^ periodicTax.Measurement == null);
        }
    }
}
