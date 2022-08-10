using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Shared.Dtos.CategoryDtos
{
    /// <summary>
    /// Dto for adding new category into database
    /// </summary>
    public partial class AddCategoryDto
    {
        [Required]
        [Column("name")]
        [StringLength(250)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        [Required]
        [Column("description", TypeName = "text")]
        public string Description { get; set; } = null!;

    }
}