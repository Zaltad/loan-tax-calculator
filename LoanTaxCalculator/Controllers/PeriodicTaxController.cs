using ClosedXML.Extensions;
using LoanTaxCalculator.Dtos.Requests;
using LoanTaxCalculator.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LoanTaxCalculator.Controllers
{
    [ApiController]
    [Route("periodic-tax")]
    [Authorize]
    public class PeriodicTaxController : ControllerBase
    {
        private readonly PeriodicTaxService _periodicTaxService;

        public PeriodicTaxController(PeriodicTaxService periodicTaxService)
        {
            _periodicTaxService = periodicTaxService;
        }

        [HttpPut]
        public async Task<IActionResult> CreatePeriodicTax([FromBody] CreatePeriodicTaxRequest request)
        {
            var userId = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;
            await _periodicTaxService.CreatePeriodicTaxAsync(userId, request);
            return NoContent(); //
        }

        [HttpGet]
        public async Task<PeriodicTaxesResponse> GetPeriodicTaxesForUser([FromQuery] int? taxTypeId, [FromQuery] DateTime? fromMonth, [FromQuery] DateTime? toMonth)
        {
            var userId = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;
            return new PeriodicTaxesResponse
            {
                PeriodicTaxes = await _periodicTaxService.GetPeriodicTaxesForUserAsync(userId, taxTypeId, fromMonth, toMonth)
            };
        }

        [HttpGet("summary")]
        public async Task<HttpResponseMessage> GetPeriodicTaxesForUser()
        {
            var userId = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;
            var workbook = await _periodicTaxService.GetPeriodicTaxesSummaryForUserAsync(userId);
            return workbook.Deliver("Loan tax summary.xlsx");
        }
    }
}
