using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Server.Dtos.CategoryDtos
{
    /// <summary>
    /// Dto for adding new category into database
    /// </summary>
    public partial class AddCategoryDto
    {
        [Column("name")]
        [StringLength(250)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        [Column("description", TypeName = "text")]
        public string Description { get; set; } = null!;

    }
}