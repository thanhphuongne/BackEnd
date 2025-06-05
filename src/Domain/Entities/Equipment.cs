namespace BackEnd.Domain.Entities;

public class Equipment : BaseAuditableEntity
{
    public int FieldId { get; set; }
    
    public string EquipmentName { get; set; } = null!;
    
    public string? Description { get; set; }
    
    public int Quantity { get; set; }
    
    public int AvailableQuantity { get; set; }
    
    public decimal RentalPricePerHour { get; set; }
    
    public string Condition { get; set; } = "Good"; // New, Good, Fair, Poor, Damaged
    
    public string? ImageUrl { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public Field Field { get; set; } = null!;
}
