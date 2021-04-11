using AutoMapper;
using LoanTaxCalculator.Dtos;
using LoanTaxCalculator.Dtos.Requests;
using LoanTaxCalculator.Entities;
using LoanTaxCalculator.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoanTaxCalculator.Services
{
    public class TaxTypeService
    {
        private readonly TaxTypeRepository _taxTypeRepository;
        private readonly IMapper _mapper;

        public TaxTypeService(TaxTypeRepository taxTypeRepository, IMapper mapper)
        {
            _taxTypeRepository = taxTypeRepository;
            _mapper = mapper;
        }

        public async Task CreateTaxTypeAsync(CreateTaxTypeRequest request)
        {
            var taxType = _mapper.Map<TaxType>(request);
            await _taxTypeRepository.CreateTaxTypeAsync(taxType);
        }

        public async Task<List<TaxTypeDto>> GetAllTaxTypesAsync()
        {
            return await _taxTypeRepository.GetAllTaxTypesAsync();
        }
    }
}
