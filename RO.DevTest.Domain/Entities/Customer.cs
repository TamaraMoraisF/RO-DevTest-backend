using System.ComponentModel.DataAnnotations.Schema;

namespace RO.DevTest.Domain.Entities;

[Table("AspNetCustomers")]
public class Customer
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}