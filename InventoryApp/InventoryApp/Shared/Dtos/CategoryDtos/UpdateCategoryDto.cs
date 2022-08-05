using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryApp.Shared.Dtos.CategoryDtos
{
    /// <summary>
    /// Dto for updating category
    /// </summary>
    public partial class UpdateCategoryDto : AddCategoryDto
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
    }
}