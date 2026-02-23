namespace ShoppingList.Shared.DTOs;

public class ShoppingListItemDto
{
    public int ShoppingListItemId { get; set; }
    
    public bool IsPicked { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
}