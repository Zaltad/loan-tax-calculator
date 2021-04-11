using LoanTaxCalculator.Dtos.Requests;
using LoanTaxCalculator.Error;
using LoanTaxCalculator.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LoanTaxCalculator.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateModel model)
        {
            var loginResult = await _userService.AuthenticateAsync(model.Username, model.Password);

            if (!loginResult.Succeeded)
            {
                return BadRequest(new ErrorResponse { ErrorMessage = "Username or password is incorrect" });
            }

            return Ok(await _userService.GetTokenAsync(model.Username));
        }

        [HttpPut("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerModel)
        {
            var identityResult = await _userService.RegisterAsync(registerModel);

            if (identityResult.Succeeded)
            {
                return Ok();
            }

            return BadRequest(identityResult.Errors);
        }

        [HttpPost("{id}/role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserRole([FromRoute] string id, [FromBody] UpdateUserRoleRequest request)
        {
            await _userService.UpdateUserRoleAsync(id, request.Role);
            return Ok(); //
        }
    }
}
