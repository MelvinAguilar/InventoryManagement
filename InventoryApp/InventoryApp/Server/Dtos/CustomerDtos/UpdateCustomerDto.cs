using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryApp.Server.Dtos.CustomerDtos
{
    /// <summary>
    /// Dto for updating customer
    /// </summary>
    public partial class UpdateCustomerDto : AddCustomerDto
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("date_modified", TypeName = "datetime")]
        public DateTime? DateModified { get; set; } = DateTime.Now;
    }
}