using System.Security.Claims;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Server.Services.Impl
{
    public class EmployeeService : IEmployeeService
    {
        private readonly inventory_managementContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EmployeeService(inventory_managementContext context, IMapper mapper, 
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Get all employees
        /// </summary>
        /// <returns>List of employees wrapped in a service response</returns>
        public async Task<ServiceResponse<IEnumerable<GetEmployeeDto>>> GetAllEmployees()
        {
            var response = new ServiceResponse<IEnumerable<GetEmployeeDto>>();
            var employees = await _context.Employees.ToListAsync();

            if (employees == null)
            {
                response.Success = false;
                response.Message = "No employees found";
            }
            else
            {
                response.Data = _mapper.Map<IEnumerable<GetEmployeeDto>>(employees);
            }

            return response;
        }

        /// <summary>
        /// Get employee by id
        /// </summary>
        /// <param name="id">Employee id</param>
        /// <returns>Employee wrapped in a service response</returns>
        public async Task<ServiceResponse<GetEmployeeDto>> GetEmployeeById(int id)
        {
            var response = new ServiceResponse<GetEmployeeDto>();
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                response.Success = false;
                response.Message = "Employee not found";
            }
            else
            {
                response.Data = _mapper.Map<GetEmployeeDto>(employee);
            }

            return response;
        }

        /// <summary>
        /// Update employee in database
        /// </summary>
        /// <param name="id">Employee Id</param>
        /// <param name="employee">Employee to update</param>
        /// <returns>Success or error message in service response</returns>
        /// <remarks>This method not update password</remarks>
        /// <remarks>This will only update the employee if the user is the same as the one who created the employee</remarks>
        public async Task<ServiceResponse<bool>> UpdateEmployee(int id, UpdateEmployeeDto employee)
        {
            var response = new ServiceResponse<bool>();
            if (id != employee.Id)
            {
                response.Success = false;
                response.Message = "Employee id mismatch";
                return response;
            }
           
            var existingEmployee = await _context.Employees.FindAsync(id);
            if (existingEmployee == null)
            {
                response.Success = false;
                response.Message = "Employee not found";
            } 
            else 
            {
                try
                {
                    // Update only a few properties of the employee in database
                    // I dont want to update the DateCreated and password property
                    // that's why I don't use: "_context.Entry(entity).State = EntityState.Modified;"
                    _context.Employees.Attach(existingEmployee);
                    existingEmployee.FirstName = employee.FirstName;
                    existingEmployee.LastName = employee.LastName;
                    existingEmployee.Email = employee.Email;
                    existingEmployee.PhoneNumber = employee.PhoneNumber;
                    existingEmployee.Avatar = employee.Avatar;
                    existingEmployee.DateModified = DateTime.Now;

                    await _context.SaveChangesAsync();
                    response.Data = true;
                }
                catch (DbUpdateException ex)
                {
                    // If enter here, it means that the email or phone number is already taken by another employee
                    response.Success = false;
                    if (EmployeeExists(employee.Email, employee.PhoneNumber))
                        response.Message = "Employee wich email " + employee.Email + " or phone number "
                            + employee.PhoneNumber + " already exists";
                    else
                        response.Message = "Error updating employee: " + ex.Message;
                }
            }           

            return response;
        }

        /// <summary>
        /// Delete employee from database
        /// </summary>
        /// <param name="id">Employee Id</param>
        /// <returns>Success or error message in service response</returns>
        /// <remarks>The admin cant delete himself or another admins</remarks>
        public async Task<ServiceResponse<bool>> DeleteEmployee(int id)
        {
            var response = new ServiceResponse<bool>();
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                response.Success = false;
                response.Message = "Employee not found";
            }
            else if (employee.Id == GetAuthenticatedEmployeeId())
            {
                response.Success = false;
                response.Message = "You can't delete yourself";
            }
            else if (employee.IdRoleNavigation.Name.Equals("Admin"))
            {
                response.Success = false;
                response.Message = "You can't delete an admin";
            } else {
                // Try catch block for deleting employee with related entities
                try
                {
                    _context.Employees.Remove(employee);
                    await _context.SaveChangesAsync();

                    response.Success = true;
                }
                catch (DbUpdateException)
                {
                    response.Success = false;
                    response.Message = "Error deleting employee: Employee has related purchase orders or supply orders";
                }
            }

            return response;
        }

        /// <summary>
        /// Check if employee exists in database
        /// </summary>
        /// <param name="id">Employee Id</param>
        /// <returns>True if employee exists, false otherwise</returns>
        private bool EmployeeExists(int id) 
            => (_context.Employees?.Any(e => e.Id == id)).GetValueOrDefault();

        /// <summary>
        /// Check if employee exists in database
        /// </summary>
        /// <param name="email">Employee email</param>
        /// <param name="phoneNumber">Employee phone number</param>
        /// <returns>True if employee exists, false otherwise</returns>
        public bool EmployeeExists(string email, string phoneNumber) 
            => (_context.Employees?.Any(e => e.Email == email || e.PhoneNumber == phoneNumber)).GetValueOrDefault();

        /// <summary>
        /// Get the ID of the authenticated employee
        /// </summary>
        /// <returns>Employee ID</returns>
        /* This method may be used in the future to auditory the administrator employee who submits request
           to change a normal employee */
        private int GetAuthenticatedEmployeeId()
        {
            if (_httpContextAccessor.HttpContext == null)
                throw new Exception("No HTTP context found");
            return int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        /// <summary>
        /// Get the role of the authenticated employee
        /// </summary>
        /// <returns>Employee role</returns>
        private string GetAuthenticatedEmployeeRole()
        {
            if (_httpContextAccessor.HttpContext == null)
                throw new Exception("No HTTP context found");
            return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        }
    }
}