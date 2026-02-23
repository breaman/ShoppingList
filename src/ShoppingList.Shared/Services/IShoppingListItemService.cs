using ShoppingList.Shared.DTOs;

namespace ShoppingList.Shared.Services;

public interface IShoppingListItemService
{
    public Task<List<ShoppingListItemDto>> GetShoppingListItemsAsync(int shoppingListId);
}