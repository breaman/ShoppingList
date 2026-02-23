using ShoppingList.Shared.DTOs;

namespace ShoppingList.Data.Models;

public class ShoppingListItem : FingerPrintEntityBase
{
    public bool IsPicked { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ShoppingListId { get; set; }
    public ShoppingList? ShoppingList { get; set; }

    public ShoppingListItemDto ToDto()
    {
        return new ShoppingListItemDto
        {
            ShoppingListItemId = Id,
            Name = Name,
            IsPicked = IsPicked,
        };
    }
}