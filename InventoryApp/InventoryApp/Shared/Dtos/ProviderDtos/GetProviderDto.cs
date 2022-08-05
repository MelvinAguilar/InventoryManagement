using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryApp.Shared.Dtos.ProviderDtos
{
    /// <summary>
    /// Dto for getting provider
    /// </summary>
    public partial class GetProviderDto
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; } = null!;
        [Column("phone_number")]
        public string PhoneNumber { get; set; } = null!;
        [Column("address")]
        public string Address { get; set; } = null!;
        [Column("description", TypeName = "text")]
        public string? Description { get; set; }
    }
}