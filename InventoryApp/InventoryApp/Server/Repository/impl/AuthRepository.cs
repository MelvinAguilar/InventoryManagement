using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using AutoMapper;
using InventoryApp.Server.Dtos.EmployeeDtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace InventoryApp.Server.Repository.impl
{
    public class AuthRepository : IAuthRepository
    {
        private readonly inventory_managementContext _context;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public AuthRepository(inventory_managementContext context, IConfiguration config, IMapper mapper)
        {
            _context = context;
            _config = config;
            _mapper = mapper;
        }
        
        /// <summary>
        /// Login user and generate token
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="password">Password</param>
        /// <returns>Token wrapped in a response</returns>
        public async Task<ServiceResponse<string>> Login(string email, string password)
        {
            var response = new ServiceResponse<string>();
            var employee = await _context.Employees
                .Include(e => e.IdRoleNavigation)
                .FirstOrDefaultAsync(e => e.Email.ToLower() == email.ToLower());

            if (employee == null)
            {
                response.Success = false;
                response.Message = "Employee not found";
            }
            else if (!VerifyPasswordHash(password, employee.PasswordHash, employee.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Wrong password";
            }
            else // Check if employee was verified goes here
            {
                response.Data = GenerateToken(employee);
            }

            return response;
        }


        /// <summary>
        /// Register new employee into database
        /// </summary>
        /// <param name="employeeRequest">Employee object</param>
        /// <param name="password">Password</param>
        /// <returns>Employee id wrapped in a response</returns>
        public async Task<ServiceResponse<int>> Register(AddEmployeeDto employeeRequest, string password)
        {
            var response = new ServiceResponse<int>();
            if (await EmployeeExists(employeeRequest.Email, employeeRequest.PhoneNumber))
            {
                response.Success = false;
                response.Message = "Employee whit this email or phone number already exists";
                return response;
            }
            
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            var employee = _mapper.Map<Employee>(employeeRequest);
            employee.PasswordHash = passwordHash;
            employee.PasswordSalt = passwordSalt;
        
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            response.Data = employee.Id;
            return response;
        }

         /// <summary>
        /// Send email to employee with toker for forgot password
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>True if email was sent, false otherwise</returns>
        public async Task<ServiceResponse<bool>> ForgotPassword(string email)
        {
            var response = new ServiceResponse<bool>();
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Email.ToLower() == email.ToLower());

            if (employee == null)
            {
                response.Success = false;
                response.Message = "Employee not found";
                return response;
            }

            employee.VerificationToken = CreateRandomToken();
            employee.ResetTokenExpires = DateTime.Now.AddMinutes(60);

            await _context.SaveChangesAsync();
            response.Data = true;

            // Send email with token
            // The logic of sending email will be implemented in the future

            return response;
        }

        /// <summary>
        /// Reset password after forgot password request
        /// </summary>
        /// <param name="request">Reset password request object</param>
        /// <returns>True if password was reset, false otherwise</returns>
        public async Task<ServiceResponse<bool>> ResetPassword(ResetPasswordRequest request)
        {
            var response = new ServiceResponse<bool>();
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.PasswordResetToken == request.Token);
            if (employee == null || employee.ResetTokenExpires < DateTime.Now)
            {
                response.Success = false;
                response.Message = "Token is invalid or expired";
                return response;
            }
            // Check if employee was verified goes here...

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            employee.PasswordHash = passwordHash;
            employee.PasswordSalt = passwordSalt;
            employee.PasswordResetToken = null;
            employee.ResetTokenExpires = null;

            await _context.SaveChangesAsync();
            response.Data = true;

            return response;
        }

        /// <summary>
        /// Change password for employee
        /// </summary>
        /// <param name="employeeId">Employee id</param>
        /// <param name="oldPassword">Old password</param>
        /// <param name="newPassword">New password</param>
        /// <returns>True if password was changed, false otherwise</returns>
        public async Task<ServiceResponse<bool>> ChangePassword(int employeeId, string oldPassword, string newPassword)
        {
            var response = new ServiceResponse<bool>();
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == employeeId);

            if (employee == null)
            {
                response.Success = false;
                response.Message = "Employee not found";
                return response;
            }
            else if (!VerifyPasswordHash(oldPassword, employee.PasswordHash, employee.PasswordSalt))
            {
                response.Success = false;
                response.Message = "The old password is incorrect, please try again";
                return response;
            }
            else
            {
                CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);
                employee.PasswordHash = passwordHash;
                employee.PasswordSalt = passwordSalt;
                await _context.SaveChangesAsync();
                response.Data = true;
            }
            return response;
        }

        /// <summary>
        /// Check if Employee exists
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="phoneNumber">Phone number</param>
        /// <returns>True if employee with the email exists, false otherwise</returns>
        public async Task<bool> EmployeeExists(string email, string phoneNumber)
        {
            if(await _context.Employees.AnyAsync(e => e.Email.ToLower() == email.ToLower() || e.PhoneNumber == phoneNumber))
                return true;

            return false;
        }

        /// <summary>
        /// Generate token for employee
        /// </summary>
        /// <param name="password">Employee password</param>
        /// <param name="passwordHash">Employee password hash</param>
        /// <param name="passwordSalt">Employee password salt</param>
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            // Using HMACSHA512 algorithm for creating password hash
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key; // Generate a key for password salt
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); // Generate a hash which takes password as bytes
            }
        }
        
        /// <summary>
        /// Verify password hash
        /// </summary>
        /// <param name="password">Password in plain text</param>
        /// <param name="passwordHash">Password hash in bytes</param>
        /// <param name="passworkdSalt">Password salt to be used for hashing and verifying password</param>
        /// <returns>True if password is correct, false otherwise</returns>
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            // Using HMACSHA512 algorithm for verifying password hash with passwordSalt as key
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); // Compute hash of password
                return computedHash.SequenceEqual(passwordHash); // Compare computed hash with password hash
            }
        }

        /// <summary>
        /// Generate token for employee
        /// </summary>
        /// <param name="employee">Employee object</param>
        /// <returns>Token</returns>
        private string GenerateToken(Employee employee)
        {
            // Create claims for token with role
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString()),
                new Claim(ClaimTypes.Email, employee.Email),
                new Claim(ClaimTypes.Role, employee.IdRoleNavigation.Name)
            };

            var key = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value)
            );
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1), // Token expires in 1 day
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }  

        /// <summary>
        /// Create Random Token
        /// </summary>
        /// <returns>Random token</returns>
        private string CreateRandomToken()
            => Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

    }
}