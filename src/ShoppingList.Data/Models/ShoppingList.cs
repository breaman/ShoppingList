using ShoppingList.Shared.DTOs;

namespace ShoppingList.Data.Models;

public class ShoppingList : FingerPrintEntityBase
{
    public string Name { get; set; } = string.Empty;
    public ICollection<ShoppingListItem> Items { get; set; } = [];
    
    public ShoppingListDto ToDto()
    {
        return new ShoppingListDto
        {
            ShoppingListId = Id,
            Name = Name,
            CreatedOn = CreatedOn
        };
    }
}