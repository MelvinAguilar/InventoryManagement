using InventoryApp.Server.Dtos.EmployeeDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [Authorize (Roles = "Administrator")]
        [HttpPost("register")]
        public async Task<ActionResult<ServerResponse<int>>> Register(AddEmployeeDto employee)
        {
            return HandleResponse(await _authRepository.Register(employee, employee.Password));
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<ServerResponse<string>>> Login(EmployeeLoginDto request)
        {
            return HandleResponse(await _authRepository.Login(request.Email, request.Password));
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<ActionResult<ServerResponse<bool>>> ForgotPassword(string email)
        {
            return HandleResponse(await _authRepository.ForgotPassword(email));
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<ActionResult<ServerResponse<bool>>> ResetPassword(ResetPasswordRequest request)
        {
            return HandleResponse(await _authRepository.ResetPassword(request));
        }

        private ActionResult<ServerResponse<T>> HandleResponse<T> (ServerResponse<T> response)
        {
            return (response.Success) ? Ok(response) : BadRequest(response); 
        }   
    }
}