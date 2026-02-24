using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SecureAppDemo.Data.Entities;
using SecureAppDemo.Dtos;
using SecureAppDemo.Exceptions;
using SecureAppDemo.Models.Request;
using SecureAppDemo.Models.Response;
using SecureAppDemo.Services;

namespace SecureAppDemo.Controllers
{
    [Authorize]
    [ApiController]
    [EnableRateLimiting("Fixed")]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [Authorize(Roles = "Admin,DataWarehouseOperator")]
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] SearchUsersRequestModel request)
        {
            try
            {
                var requestDto = new SearchUsersRequestDto
                {
                    Name = request.Name,
                    Email = request.Email,
                    ExternalId = request.ExternalId,
                };

                var users = await userService.GetUsersAsync(requestDto).ConfigureAwait(false);

                var dtos = users.Select(u => new UserResponse
                {
                    Username = u.Username,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    ExternalId = u.ExternalId,
                }).ToList();

                return Ok(dtos);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving the users");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newUser = new CreateUserDto // ocultamiento de la contraseña en el dto de respuesta, se utiliza un dto de request para recibir la contraseña y un dto de response para devolver los datos del usuario sin la contraseña
                {
                    Username = request.Username,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Password = request.Password,
                    RoleNames = request.RoleNames,
                };

                var createdUser = await userService.CreateUserAsync(newUser, request.RoleNames).ConfigureAwait(false);

                var userResponse = new UserResponse
                {
                    Username = createdUser.Username,
                    FirstName = createdUser.FirstName,
                    LastName = createdUser.LastName,
                    Email = createdUser.Email,
                    ExternalId = createdUser.ExternalId,
                };

                return CreatedAtAction(nameof(GetUsers), new { }, userResponse);
            }
            catch (BadRequestResponseException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while creating the user");
            }
        }
    }
}