using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryApp.Server.Dtos.SupplyDtos
{
    /// <summary>
    /// Dto for getting supply
    /// </summary>
    public partial class GetSupplyDto
    {
        public GetSupplyDto()
        {
            SupplyDetails = new HashSet<SupplyDetail>();
        }

        [Column("id")]
        public int Id { get; set; }
        [Column("total_amount", TypeName = "money")]
        public decimal TotalAmount { get; set; }
        [Column("payment", TypeName = "money")]
        public decimal Payment { get; set; }
        [Column("date_supplied", TypeName = "datetime")]
        public DateTime DateSupplied { get; set; }
        [Column("id_employee")]
        public int IdEmployee { get; set; }
        [Column("id_provider")]
        public int IdProvider { get; set; }

        [ForeignKey("IdEmployee")]
        [InverseProperty("Supplies")]
        public virtual Employee IdEmployeeNavigation { get; set; } = null!;
        [ForeignKey("IdProvider")]
        [InverseProperty("Supplies")]
        public virtual Provider IdProviderNavigation { get; set; } = null!;
        [InverseProperty("IdSupplyNavigation")]
        public virtual ICollection<SupplyDetail> SupplyDetails { get; set; }
    }
}