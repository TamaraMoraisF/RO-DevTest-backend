﻿namespace RO.DevTest.Domain.Entities
{
    public class Sale
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid CustomerId { get; set; }

        public DateTime SaleDate { get; set; } = DateTime.UtcNow;

        public ICollection<SaleItem> Items { get; set; } = [];

        public decimal TotalAmount => Items.Sum(i => i.Quantity * i.UnitPrice);

        public Customer Customer { get; set; } = null!;
    }
}