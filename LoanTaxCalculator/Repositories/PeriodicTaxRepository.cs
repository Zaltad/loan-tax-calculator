using AutoMapper;
using AutoMapper.QueryableExtensions;
using LoanTaxCalculator.Dtos.Requests;
using LoanTaxCalculator.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanTaxCalculator.Repositories
{
    public class PeriodicTaxRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfigurationProvider _configurationProvider;

        public PeriodicTaxRepository(AppDbContext appDbContext, IConfigurationProvider configurationProvider)
        {
            _dbContext = appDbContext;
            _configurationProvider = configurationProvider;
        }

        public async Task CreatePeriodicTaxAsync(PeriodicTax periodicTax)
        {
            _dbContext.PeriodicTaxes.Add(periodicTax);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<PeriodicTaxResponse>> GetPeriodicTaxesForUserAsync(string userId, int? taxTypeId, DateTime? fromMonth, DateTime? toMonth)
        {
            var periodicTaxes = _dbContext.PeriodicTaxes.Where(periodicTax => periodicTax.UserId == userId);
            if (taxTypeId != null)
            {
                periodicTaxes = periodicTaxes.Where(periodicTax => periodicTax.TaxTypeId == taxTypeId);
            }
            if (fromMonth != null)
            {
                periodicTaxes = periodicTaxes.Where(periodicTax => periodicTax.ForMonth >= fromMonth);
            }
            if (toMonth != null)
            {
                periodicTaxes = periodicTaxes.Where(periodicTax => periodicTax.ForMonth <= toMonth);
            }
            return await periodicTaxes
                .Include(periodicTax => periodicTax.TaxType)
                .Include(periodicTax => periodicTax.Measurement)
                .OrderBy(periodicTax => periodicTax.ForMonth)
                .ProjectTo<PeriodicTaxResponse>(_configurationProvider).ToListAsync();
        }
    }
}
