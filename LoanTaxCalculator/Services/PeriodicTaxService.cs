using AutoMapper;
using ClosedXML.Excel;
using FluentValidation;
using LoanTaxCalculator.Dtos.Requests;
using LoanTaxCalculator.Entities;
using LoanTaxCalculator.Repositories;
using LoanTaxCalculator.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanTaxCalculator.Services
{
    public class PeriodicTaxService
    {
        private readonly PeriodicTaxRepository _periodicTaxRepository;
        private readonly TaxTypeRepository _taxTypeRepository;
        private readonly PeriodicTaxesSummaryExportService _periodicTaxesSummaryExportService;
        private readonly IMapper _mapper;

        public PeriodicTaxService(PeriodicTaxRepository periodicTaxRepository, TaxTypeRepository taxTypeRepository, IMapper mapper, PeriodicTaxesSummaryExportService periodicTaxesSummaryExportService)
        {
            _periodicTaxRepository = periodicTaxRepository;
            _taxTypeRepository = taxTypeRepository;
            _periodicTaxesSummaryExportService = periodicTaxesSummaryExportService;
            _mapper = mapper;
        }

        public async Task CreatePeriodicTaxAsync(string userId, CreatePeriodicTaxRequest request)
        {
            new CreatePeriodicTaxRequestValidator().ValidateAndThrow(request);
            await validatePeriodicTaxMeasurementAsync(request);
            var periodicTax = _mapper.Map<PeriodicTax>(request);
            periodicTax.UserId = userId;
            await _periodicTaxRepository.CreatePeriodicTaxAsync(periodicTax);
        }

        public async Task<List<PeriodicTaxResponse>> GetPeriodicTaxesForUserAsync(string userId, int? taxTypeId, DateTime? fromMonth, DateTime? toMonth)
        {
            return await _periodicTaxRepository.GetPeriodicTaxesForUserAsync(userId, taxTypeId, fromMonth, toMonth);
        }

        public async Task<XLWorkbook> GetPeriodicTaxesSummaryForUserAsync(string userId)
        {
            var periodicTaxes = await GetPeriodicTaxesForUserAsync(userId, null, null, null);
            var taxTypes = periodicTaxes
                .Select(periodicTax => periodicTax.TaxType)
                .GroupBy(taxType => taxType.Id)
                .Select(taxTypes => taxTypes.First())
                .ToList();
            var periods = periodicTaxes
                .Select(periodicTax => new DateTime(periodicTax.ForMonth.Year, periodicTax.ForMonth.Month, 1))
                .Distinct()
                .ToList();
            var periodicTaxesByTaxTypeAndMonth = periodicTaxes
                .GroupBy(periodicTax => new TaxTypeAndMonth
                {
                    TaxTypeId = periodicTax.TaxType.Id,
                    Month = new DateTime(periodicTax.ForMonth.Year, periodicTax.ForMonth.Month, 1)
                })
                .ToDictionary(periodicTaxes => periodicTaxes.Key, periodicTaxes => periodicTaxes.Single());
            return await Task.Run(() =>
            {
                return _periodicTaxesSummaryExportService.ExportPeriodicTaxesSummary(taxTypes, periods, periodicTaxesByTaxTypeAndMonth);
            });
        }

        private async Task validatePeriodicTaxMeasurementAsync(CreatePeriodicTaxRequest request)
        {
            var taxType = await _taxTypeRepository.GetTaxTypeByIdAsync(request.TaxTypeId);

            if ((taxType.UnitOfMeasurement != null && request.Measurement == null) ||
                (taxType.UnitOfMeasurement == null && request.Cost == null))
            {
                throw new PeriodicTaxMeasurementException("Tax type and periodic tax measurement units don't match");
            }
        }
    }

    public class TaxTypeAndMonth
    {
        public int TaxTypeId { get; set; }
        public DateTime Month { get; set; }
    }
}
