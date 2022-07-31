using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryApp.Server.Dtos.EmployeeDtos
{
    /// <summary>
    /// Dto for updating employee
    /// </summary>
    public partial class UpdateEmployeeDto : AddEmployeeDto
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("date_modified", TypeName = "datetime")]
        public DateTime? DateModified { get; set; } = DateTime.Now;
    }
}