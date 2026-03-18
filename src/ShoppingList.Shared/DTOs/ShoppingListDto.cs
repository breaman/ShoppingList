namespace ShoppingList.Shared.DTOs;

public class ShoppingListDto
{
    public int ShoppingListId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime? CreatedOn { get; set; }
    public int TotalItems { get; set; }
    public int RemainingItems { get; set; }
}