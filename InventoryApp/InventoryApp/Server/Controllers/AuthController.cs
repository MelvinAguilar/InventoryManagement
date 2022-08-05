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
        public async Task<ActionResult<ServiceResponse<int>>> Register(AddEmployeeDto employee)
        {
            return HandleResponse(await _authRepository.Register(employee, employee.Password));
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(EmployeeLoginDto request)
        {
            return HandleResponse(await _authRepository.Login(request.Email, request.Password));
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<ActionResult<ServiceResponse<bool>>> ForgotPassword(string email)
        {
            return HandleResponse(await _authRepository.ForgotPassword(email));
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<ActionResult<ServiceResponse<bool>>> ResetPassword(ResetPasswordRequest request)
        {
            return HandleResponse(await _authRepository.ResetPassword(request));
        }

        private ActionResult<ServiceResponse<T>> HandleResponse<T> (ServiceResponse<T> response)
        {
            return (response.Success) ? Ok(response) : BadRequest(response); 
        }   
    }
}