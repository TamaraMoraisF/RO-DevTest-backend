using System.ComponentModel.DataAnnotations.Schema;

namespace RO.DevTest.Domain.Entities
{
    [Table("AspNetSaleItems")] 
    public class SaleItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid SaleId { get; set; }

        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
    }
}
