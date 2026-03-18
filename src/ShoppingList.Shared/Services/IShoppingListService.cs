using ShoppingList.Shared.DTOs;

namespace ShoppingList.Shared.Services;

public interface IShoppingListService
{
    public Task<List<ShoppingListDto>> GetShoppingListsAsync();

    public Task<ShoppingListDto?> GetShoppingListByIdAsync(int id);

    public Task<ShoppingListDto?> UpdateShoppingListAsync(ShoppingListDto shoppingListDto);

    public Task DeleteShoppingListAsync(int id);
}