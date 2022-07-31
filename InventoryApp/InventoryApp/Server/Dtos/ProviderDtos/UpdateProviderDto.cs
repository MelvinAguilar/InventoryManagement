using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryApp.Server.Dtos.ProviderDtos
{
    /// <summary>
    /// Dto for updating provider
    /// </summary>
    public partial class UpdateProviderDto : AddProviderDto
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("date_modified", TypeName = "datetime")]
        public DateTime? DateModified { get; set; } = DateTime.Now;
    }
}