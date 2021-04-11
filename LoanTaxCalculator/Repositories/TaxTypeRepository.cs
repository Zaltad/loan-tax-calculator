using AutoMapper;
using AutoMapper.QueryableExtensions;
using LoanTaxCalculator.Dtos;
using LoanTaxCalculator.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoanTaxCalculator.Repositories
{
    public class TaxTypeRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfigurationProvider _configurationProvider;

        public TaxTypeRepository(AppDbContext appDbContext, IConfigurationProvider configurationProvider)
        {
            _dbContext = appDbContext;
            _configurationProvider = configurationProvider;
        }

        public async Task CreateTaxTypeAsync(TaxType taxType)
        {
            _dbContext.TaxTypes.Add(taxType);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<TaxTypeDto>> GetAllTaxTypesAsync()
        {
            return await _dbContext.TaxTypes.ProjectTo<TaxTypeDto>(_configurationProvider).ToListAsync();
        }

        public async Task<TaxType> GetTaxTypeByIdAsync(int id)
        {
            return await _dbContext.TaxTypes.SingleAsync(taxType => taxType.Id == id);
        }
    }
}
