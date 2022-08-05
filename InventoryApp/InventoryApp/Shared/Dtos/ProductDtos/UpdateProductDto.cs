using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryApp.Shared.Dtos.ProductDtos
{
    /// <summary>
    /// Dto for updating product
    /// </summary>
    public partial class UpdateProductDto : AddProductDto
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
    }
}