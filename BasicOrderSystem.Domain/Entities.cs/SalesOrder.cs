using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BasicOrderSystem.Shared;

namespace BasicOrderSystem.Domain.Entities.cs;

public class SalesOrder
{
    [Key]
    public int Id { get; set; }
    public int OrderId { get; set; }
    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string BillingAddress { get; set; }
    public string BillingZipcode { get; set; }
    public string BillingCity { get; set; }
    public string ShippingAddress { get; set; }
    public string ShippingZipcode { get; set; }
    public string ShippingCity { get; set; }
    
    public int CreatedById { get; set; }
    public User CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? LastModifiedById { get; set; }
    public User LastModifiedBy { get; set; }
    public DateTime? LastModifiedAt { get; set; }
    
    public OrderStatus OrderStatus { get; set; }
    
    public string OrderLineDescription { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal OrderLineTotal { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal OrderTotal { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal OrderTax { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal OrderDiscount { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal ShippingTotal { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal SubTotal { get; set; }
}