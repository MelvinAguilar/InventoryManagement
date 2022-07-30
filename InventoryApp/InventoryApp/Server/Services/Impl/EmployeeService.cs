using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Server.Services.Impl
{
    public class EmployeeService : IEmployeeService
    {
        public readonly inventory_managementContext _context;

        public EmployeeService(inventory_managementContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all employees
        /// </summary>
        /// <returns>List of employees wrapped in a response</returns>
        public async Task<ServerResponse<IEnumerable<Employee>>> GetAllEmployees()
        {
            var response = new ServerResponse<IEnumerable<Employee>>();
            var employees = await _context.Employees.ToListAsync();

            if (employees == null)
            {
                response.Success = false;
                response.Message = "No employees found";
            }
            else
            {
                response.Data = employees;
            }

            return response;
        }

        /// <summary>
        /// Get employee by id
        /// </summary>
        /// <param name="id">Employee id</param>
        /// <returns>Employee wrapped in a response</returns>
        public async Task<ServerResponse<Employee>> GetEmployeeById(int id)
        {
            var response = new ServerResponse<Employee>();
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                response.Success = false;
                response.Message = "Employee not found";
            }
            else
            {
                response.Data = employee;
            }

            return response;
        }

        /// <summary>
        /// Add new employee into database
        /// </summary>
        /// <param name="employee">Employee to add</param>
        /// <returns>Added employee wrapped in a response</returns>
        public async Task<ServerResponse<Employee>> AddEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return new ServerResponse<Employee> { Data = employee };
        }

        /// <summary>
        /// Update employee in database
        /// </summary>
        /// <param name="id">Employee Id</param>
        /// <param name="employee">Employee to update</param>
        /// <returns>Success or error message in server response</returns>
        public async Task<ServerResponse<bool>> UpdateEmployee(int id, Employee employee)
        {
            var response = new ServerResponse<bool>();
            if (id != employee.Id)
            {
                response.Success = false;
                response.Message = "Employee id mismatch";
            }
            else
            {
                try
                {
                    _context.Entry(employee).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    
                    response.Data = true;
                }
                catch (DbUpdateConcurrencyException e)
                {
                    response.Success = false;
                    
                    if (!EmployeeExists(employee.Id))
                        response.Message = "Employee not found";
                    else
                        response.Message = "Error updating employee: " + e.Message;
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
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
                
                response.Data = true;
            }

            return response;
        }

        /// <summary>
        /// Check if employee exists in database
        /// </summary>
        /// <param name="id">Employee Id</param>
        /// <returns>True if employee exists, false otherwise</returns>
        public bool EmployeeExists(int id) 
            => (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}