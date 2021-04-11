using LoanTaxCalculator.Dtos.Requests;
using LoanTaxCalculator.Dtos.Responses;
using LoanTaxCalculator.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LoanTaxCalculator.Controllers
{
    [ApiController]
    [Route("tax-type")]
    public class TaxTypeController : ControllerBase
    {
        private readonly TaxTypeService _taxTypeService;

        public TaxTypeController(TaxTypeService taxTypeService)
        {
            _taxTypeService = taxTypeService;
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateTaxType([FromBody] CreateTaxTypeRequest request)
        {
            await _taxTypeService.CreateTaxTypeAsync(request);
            return NoContent(); //
        }

        [HttpGet]
        public async Task<TaxTypesResponse> GetAllTaxTypes()
        {
            return new TaxTypesResponse
            {
                TaxTypes = await _taxTypeService.GetAllTaxTypesAsync()
            };
        }
    }
}
