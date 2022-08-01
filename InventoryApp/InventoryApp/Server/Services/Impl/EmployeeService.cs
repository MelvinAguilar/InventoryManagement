using AutoMapper;
using InventoryApp.Server.Dtos.EmployeeDtos;
using Microsoft.EntityFrameworkCore;

/*
    TODO: Add authentication to this service
*/

namespace InventoryApp.Server.Services.Impl
{
    public class EmployeeService : IEmployeeService
    {
        public readonly inventory_managementContext _context;
        public readonly IMapper _mapper;

        public EmployeeService(inventory_managementContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all employees
        /// </summary>
        /// <returns>List of employees wrapped in a response</returns>
        public async Task<ServerResponse<IEnumerable<GetEmployeeDto>>> GetAllEmployees()
        {
            var response = new ServerResponse<IEnumerable<GetEmployeeDto>>();
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
        /// <returns>Employee wrapped in a response</returns>
        public async Task<ServerResponse<GetEmployeeDto>> GetEmployeeById(int id)
        {
            var response = new ServerResponse<GetEmployeeDto>();
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
        /// Add new employee into database
        /// </summary>
        /// <param name="employee">Employee to add</param>
        /// <returns>Added employee wrapped in a response</returns>
        public async Task<ServerResponse<GetEmployeeDto>> AddEmployee(AddEmployeeDto employee)
        {
            var response = new ServerResponse<GetEmployeeDto>();
            // Try catch block to catch any errors that may occur while inserting into database
            try
            {
                var newEmployee = _mapper.Map<Employee>(employee);
                _context.Employees.Add(newEmployee);
                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<GetEmployeeDto>(newEmployee);
            }
            catch (DbUpdateException ex)
            {
                // If enter here, it means that the email or phone number is already taken by another employee
                response.Success = false;
                if (EmployeeExists(employee.Email, employee.PhoneNumber))
                    response.Message = "Employee wich email or phone number " + employee.Email + " or " + employee.PhoneNumber + " already exists";
                else
                    response.Message = "Error adding employee: " + ex.Message;
            }

            return response;
        }

        /// <summary>
        /// Update employee in database
        /// </summary>
        /// <param name="id">Employee Id</param>
        /// <param name="employee">Employee to update</param>
        /// <returns>Success or error message in server response</returns>
        public async Task<ServerResponse<bool>> UpdateEmployee(int id, UpdateEmployeeDto employee)
        {
            var response = new ServerResponse<bool>();
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
                    // Update only a few properties of the category in database
                    // I dont want to update the DateCreated property
                    // that's why I don't use: "_context.Entry(existingCategory).State = EntityState.Modified;"
                    _context.Employees.Attach(existingEmployee);
                    existingEmployee.FirstName = employee.FirstName;
                    existingEmployee.LastName = employee.LastName;
                    existingEmployee.Email = employee.Email;
                    existingEmployee.PhoneNumber = employee.PhoneNumber;
                    existingEmployee.Avatar = employee.Avatar;
                    //existingEmployee.Password = employee.Password; //Warning
                    existingEmployee.DateModified = DateTime.Now;

                    await _context.SaveChangesAsync();
                    response.Data = true;
                }
                catch (DbUpdateException ex)
                {
                    // If enter here, it means that the email or phone number is already taken by another employee
                    response.Success = false;
                    if (EmployeeExists(employee.Email, employee.PhoneNumber))
                        response.Message = "Employee wich email " + employee.Email + " or phone number " + employee.PhoneNumber + " already exists";
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
        /// <returns>Success or error message in server response</returns>
        public async Task<ServerResponse<bool>> DeleteEmployee(int id)
        {
            var response = new ServerResponse<bool>();
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                response.Success = false;
                response.Message = "Employee not found";
            }
            else
            {
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
    }
}