using AutoMapper;
using LoanTaxCalculator.Dtos;
using LoanTaxCalculator.Dtos.Requests;
using LoanTaxCalculator.Entities;

namespace LoanTaxCalculator
{
    public class LoanTaxCalculatorProfile : Profile
    {
        public LoanTaxCalculatorProfile()
        {
            CreateMap<CreateTaxTypeRequest, TaxType>();
            CreateMap<CreatePeriodicTaxRequest, PeriodicTax>();
            CreateMap<TaxMeasurementDto, TaxMeasurement>();
            CreateMap<RegisterRequest, User>();

            CreateMap<TaxType, TaxTypeDto>();
            CreateMap<PeriodicTax, PeriodicTaxResponse>();
            CreateMap<TaxMeasurement, TaxMeasurementDto>();
        }
    }
}
