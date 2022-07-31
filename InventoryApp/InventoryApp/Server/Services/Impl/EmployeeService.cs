using AutoMapper;
using InventoryApp.Server.Dtos.EmployeeDtos;
using Microsoft.EntityFrameworkCore;

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
            var newEmployee = _mapper.Map<Employee>(employee);
            _context.Employees.Add(newEmployee);
            await _context.SaveChangesAsync();

            return new ServerResponse<GetEmployeeDto> { Data = _mapper.Map<GetEmployeeDto>(newEmployee) };
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
            }
            else
            {
                try
                {
                    // TODO: Update only a part of the employee
                    var updatedEmployee = _mapper.Map<Employee>(employee);
                    _context.Entry(updatedEmployee).State = EntityState.Modified;
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