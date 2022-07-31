using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryApp.Server.Dtos.CustomerDtos
{
    /// <summary>
    /// Dto for getting customer
    /// </summary>
    public partial class GetCustomerDto
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("first_name")]
        public string FirstName { get; set; } = null!;
        [Column("last_name")]
        public string LastName { get; set; } = null!;
        [Column("phone_number")]
        public string PhoneNumber { get; set; } = null!;
    }
}