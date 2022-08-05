using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryApp.Shared.Dtos.ProviderDtos
{
    /// <summary>
    /// Dto for updating provider
    /// </summary>
    public partial class UpdateProviderDto : AddProviderDto
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
    }
}