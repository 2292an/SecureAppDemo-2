using Microsoft.AspNetCore.Mvc;
using SecureAppDemo.Dtos;
using SecureAppDemo.Models.Request;
using SecureAppDemo.Models.Response;
using SecureAppDemo.Services;

namespace Controllers
{
    [ApiController]
    [Route("api/authorization")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthorizationService authorizationService;

        public AuthorizationController(IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
        }

        [HttpPost("authorize")]
        public async Task<IActionResult> Authorize([FromBody] AuthorizationRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var requestDto = new AuthorizationRequestDto
                {
                    Username = request.Username,
                    Password = request.Password,
                };

                var result = await authorizationService.AuthorizeAsync(requestDto).ConfigureAwait(false);

                if (result == null)
                {
                    return Unauthorized("Invalid username or password");
                }

                return Ok(new AuthorizationResponse
                {
                    BearerToken = result.BearerToken,
                    ExpiresIn = result.ExpiresIn,
                });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while authorizing the user");
            }
        }
    }
}
