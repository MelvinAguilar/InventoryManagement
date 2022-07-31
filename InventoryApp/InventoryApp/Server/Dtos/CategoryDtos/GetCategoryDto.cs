using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryApp.Server.Dtos.CategoryDtos
{
    /// <summary>
    /// Dto for getting category
    /// </summary>
    public partial class GetCategoryDto
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; } = null!;
        [Column("description", TypeName = "text")]
        public string Description { get; set; } = null!;
    }
}