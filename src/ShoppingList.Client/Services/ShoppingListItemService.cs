using System.Net.Http.Json;
using ShoppingList.Shared.DTOs;
using ShoppingList.Shared.Services;

namespace ShoppingList.Client.Services;

public class ShoppingListItemService(HttpClient httpClient) : IShoppingListItemService
{
    public async Task<List<ShoppingListItemDto>> GetShoppingListItemsAsync(int shoppingListId)
    {
        return await httpClient.GetFromJsonAsync<List<ShoppingListItemDto>>(
            $"/api/ShoppingListItems/{shoppingListId}") ?? [];
    }
}